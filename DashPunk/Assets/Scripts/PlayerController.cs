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
    public Vector2 direction;
    public float dashSpeed;
    private float dashTime;
    public float initialDashTime;
    public int isPierceDashing = 0;
    public int isBounceDashing = 0;
    public int isHalting = 0;
    private int hearts;
    public Text deadText;
    List<GameObject> enemyColliders = new List<GameObject>();
    Collider2D tempEnemyCollider;
    Rigidbody2D tempRigidBody;
    public float bouncePower;
    public float invulnTimeStart;
    private float invulnTime;
    public float haltTimeStart;
    private float haltTime;
    private int invuln;
    public ParticleSystem dust;
    public ParticleSystem dust2;
    private Vector2 knockBackDir;
    public float knockBackPower;
    public Image[] heartsList;
    private bool bounceCooldown = false;
    private bool pierceCooldown = false;
    private bool haltCooldown = true;
    public static int enemyHits = 0;

    void Start()
    {
        heartsList[0].enabled = true;
        heartsList[1].enabled = true;
        heartsList[2].enabled = true;
        rb = GetComponent<Rigidbody2D>();
        dashTime = initialDashTime;
        hearts = 3;
        deadText.text = "";
        deadText.fontSize = 50;
        enemyColliders = GameObject.FindGameObjectsWithTag("Enemy").OfType<GameObject>().ToList();
        invuln = 0;
        invulnTime = invulnTimeStart;
        haltTime = haltTimeStart;
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
        if (enemyHits > 4)
        {
            haltCooldown = false;
        }

        // We want to display the enemyHits counter on the canvas.

        // Start dash when right and left mouse buttons are pressed
        if (Input.GetMouseButtonDown(1) && isHalting == 0)
        {
            isPierceDashing = 1;
        }
        else if (Input.GetMouseButtonDown(0) && isHalting == 0)
        {
            isBounceDashing = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && haltCooldown == false)
        {
            isHalting = 1;
        }
        
        if (invuln == 1)
        {
            if (invulnTime <= 0)
            {
                invuln = 0;
                invulnTime = invulnTimeStart;
            } else
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

            enemyHits = 0; // Bring the halt bar back down to 0
            MEnemyControl.isHalted = true; // Enemies are frozen
            // Set up the clone dash.
                 // Have a duplicate player object (without health) spawn in between the mouse and the player every time you dash.
            // Wait 5 seconds 
            isHalting = 0;
            MEnemyControl.isHalted = false; // Enemies unfreeze
            // All the set-up clone dashes fire at this point.
            haltCooldown = true; // Halt bar goes back on cooldown.

            if (haltTime <= 0)
            {
                isHalting = 0;
                haltTime = haltTimeStart;
                for (int i = 0; i < enemyColliders.Count; i++)
                {
                    tempRigidBody = enemyColliders[i].GetComponent<Rigidbody2D>();
                    tempRigidBody.isKinematic = false;
                }
                MEnemyControl.isHalted = false; // Enemies unfreeze
                // All the set-up clone dashes fire at this point.
                haltCooldown = true; // Halt bar goes back on cooldown.
            } else
            {
                enemyHits = 0; // Bring the halt bar back down to 0
                MEnemyControl.isHalted = true; // Enemies are frozen
                for (int i = 0; i < enemyColliders.Count; i++)
                {
                    tempRigidBody = enemyColliders[i].GetComponent<Rigidbody2D>();
                    tempRigidBody.velocity = Vector2.zero;
                    tempRigidBody.angularVelocity = 0f;
                    tempRigidBody.isKinematic = true;
                }
                // Set up the clone dash.
                   // Have a duplicate player object (without health) spawn in between the mouse and the player every time you dash.
                // Wait 5 seconds 
                haltTime -= Time.deltaTime;
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
            }
            else
            {
                rb.velocity = direction * dashSpeed;
                dashTime -= Time.fixedDeltaTime;
            }


        // Move character when BounceDashing
        } else if (isBounceDashing == 1 && bounceCooldown == false)
        {
            CreateDust();
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                dashTime = initialDashTime;
                isBounceDashing = 0;
                bounceCooldown = true;
                Invoke("bounceDashCD", 0.5f);
            }
            else
            {
                rb.velocity = direction * dashSpeed;
                dashTime -= Time.fixedDeltaTime;
            }
            
        }
    }

 
    void OnTriggerEnter2D(Collider2D other)
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
