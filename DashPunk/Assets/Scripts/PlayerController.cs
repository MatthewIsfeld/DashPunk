using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Vector2 cursorPos;
    public static Vector2 direction;
    public float dashSpeed;
    private float dashTime;
    public float initialDashTime;
    public int isPierceDashing;
    public int isBounceDashing;
    public int isHalting;
    public int hearts;
    public Text deadText;
    List<GameObject> enemyColliders = new List<GameObject>();
    Collider2D tempEnemyCollider;
    public float bouncePower;
    public float invulnTimeStart;
    private float invulnTime;
    private int invuln;
    public ParticleSystem dust;
    public ParticleSystem dust2;
    private Vector2 knockBackDir;
    public float knockBackPower;
    public Image[] heartsList;
    private bool bounceCooldown;
    private bool pierceCooldown;
    private bool haltCooldown;
    public static int enemyHits;
    public Text enemyHitsText;
    public float haltTimeStart;
    public float haltTime;
    public Rigidbody2D tempBody;
    private bool spaceDash; 

    void Start()
    {
        heartsList[0].enabled = true;
        heartsList[1].enabled = true;
        heartsList[2].enabled = true;
        rb = GetComponent<Rigidbody2D>();
        dashTime = initialDashTime;
        deadText.text = "";
        deadText.fontSize = 50;
        enemyColliders = GameObject.FindGameObjectsWithTag("Enemy").OfType<GameObject>().ToList();
        invuln = 0;
        invulnTime = invulnTimeStart;
        haltTime = haltTimeStart;
        isHalting = 0;
        haltCooldown = true;
        enemyHits = 0;
        pierceCooldown = false;
        bounceCooldown = false;
        isPierceDashing = 0;
        isBounceDashing = 0;
        spaceDash = false;
}

    // Update is called once per frame
    void Update()
    {
        // Get Mouse cursor position relative to player and turn it into a unit vector
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector2(cursorPos.x - transform.position.x, cursorPos.y - transform.position.y);
        direction = direction.normalized;

        // Get all enemies
        enemyColliders = GameObject.FindGameObjectsWithTag("Enemy").OfType<GameObject>().ToList();

        // Check how full your halt bar is
        if (enemyHits >= 0)
        {
            haltCooldown = false;
        }

        // Display the enemyHits counter on the canvas. Will change to a bar at a later point.
        enemyHitsText.text = "Halting: " + enemyHits.ToString();
        // Start dash when right and left mouse buttons are pressed
        if (Input.GetMouseButtonDown(1) && (isHalting == 0))
        {
            isPierceDashing = 1;
        }
        else if (Input.GetMouseButtonDown(0) && (isHalting == 0))
        {
            isBounceDashing = 1;
        }
        else if ((Input.GetKeyDown(KeyCode.Space)) && (haltCooldown == false) && spaceDash == false)
        {
            isHalting = 1;
        }

        if (invuln == 1)
        {
            if (invulnTime <= 0)
            {
                invuln = 0;
                invulnTime = invulnTimeStart;
            }
            else
            {
                invulnTime -= Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        // Move character when not dashing
        if ((isPierceDashing == 0) && (isBounceDashing == 0))
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            float verticalMove = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(horizontalMove, verticalMove);
            rb.velocity = movement * speed;
        }

        // Freeze time with the halt mechanic
        if (isHalting == 1)
        {
            if (haltTime <= 0)
            {
                isHalting = 0;
                MEnemyControl.isHalted = false;// Enemies unfreeze
                haltCooldown = true; // Halt bar goes back on cooldown
                haltTime = haltTimeStart;
                for (int i = 0; i < enemyColliders.Count; i++)
                {
                    tempBody = enemyColliders[i].GetComponent<Rigidbody2D>();
                    tempBody.isKinematic = false;
                }

            } else
            {
                enemyHits = 0; // Bring the halt bar back down to 0
                MEnemyControl.isHalted = true; 
                for (int i = 0; i < enemyColliders.Count; i++)
                {
                    tempBody = enemyColliders[i].GetComponent<Rigidbody2D>();
                    tempBody.isKinematic = true;
                    tempBody.velocity = Vector2.zero;
                    tempBody.angularVelocity = 0f;
                }
                haltTime -= Time.fixedDeltaTime;
            }
        }

        // Move character when pierceDashing and disable enemy collider so character can pass through
        else if (isPierceDashing == 1 && pierceCooldown == false)
        {
            for (int i = 0; i < enemyColliders.Count; i++)
            {
                tempEnemyCollider = enemyColliders[i].GetComponent<Collider2D>();
                tempEnemyCollider.enabled = !tempEnemyCollider.enabled;
            }
            CreateDust2();
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                dashTime = initialDashTime;
                isPierceDashing = 0;
                pierceCooldown = true;
                Invoke("pierceDashCD", 0.5f);
                spaceDash = false;
            }
            else
            {
                spaceDash = true;
                rb.velocity = direction * dashSpeed;
                dashTime -= Time.fixedDeltaTime;
            }


            // Move character when BounceDashing
        }
        else if (isBounceDashing == 1 && bounceCooldown == false)
        {
            CreateDust();
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                dashTime = initialDashTime;
                isBounceDashing = 0;
                bounceCooldown = true;
                Invoke("bounceDashCD", 0.5f);
                spaceDash = false;
            }
            else
            {
                spaceDash = true;
                rb.velocity = direction * dashSpeed;
                dashTime -= Time.fixedDeltaTime;
            }

        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        // If the player contacts an enemy, they take damage. If they're not dashing, display damage with simple text boxes
        if (other.gameObject.CompareTag("Enemy"))
        {
            if ((isPierceDashing == 0) && (isBounceDashing == 0) && (invuln == 0))
            {
                hearts -= 1;
                heartsList[0].enabled = false;
                heartsList = heartsList.Skip(1).ToArray();
                invuln = 1;
                knockBackDir = new Vector2(transform.position.x - other.gameObject.GetComponent<Transform>().position.x, transform.position.y - other.gameObject.GetComponent<Transform>().position.y);
                rb.AddForce(knockBackDir * knockBackPower);
                if (hearts < 1)
                {
                    deadText.text = "THE PUNK'S JOURNEY IS OVER";
                    this.gameObject.SetActive(false);
                }
            }
            else if ((isPierceDashing == 0) && (isBounceDashing == 0) && (invuln == 1))
            {
                knockBackDir = new Vector2(transform.position.x - other.gameObject.GetComponent<Transform>().position.x, transform.position.y - other.gameObject.GetComponent<Transform>().position.y);
                rb.AddForce(knockBackDir * knockBackPower);
            }
            // If player bounce dashes push enemy with force
            else if (isBounceDashing == 1)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce((direction * bouncePower));
            }
        }
    }

    void CreateDust()
    {
        dust.Play();
    }

    void CreateDust2()
    {
        dust2.Play();
    }

    void bounceDashCD()
    {
        bounceCooldown = false;
    }

    void pierceDashCD()
    {
        pierceCooldown = false;
    }
}
