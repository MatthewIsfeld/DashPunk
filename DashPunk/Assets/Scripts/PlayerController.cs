using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 playerPos;
    private Vector2 cursorPos;
    public static Vector2 direction;
    public int isPierceDashing;
    public int isBounceDashing;
    public int isHalting;
    public Text deadText;
    List<GameObject> enemyColliders = new List<GameObject>();
    Collider2D tempEnemyCollider;
    private int invuln;
    public ParticleSystem dust;
    public ParticleSystem dust2;
    public ParticleSystem haltDust;
    private Vector2 knockBackDir;
    private bool bounceCooldown;
    private bool pierceCooldown;
    private bool dashCooldown;
    private bool haltCooldown;
    public static int enemyHits;
    public Rigidbody2D tempBody;
    private bool spaceDash;
    public HealthBar healthbar;
    public GameObject bounceLine;
    public GameObject pierceLine;
    public GameObject mouse;

    //Parameters for upgrades
    public int bounceDamage;
    public int pierceDamage;
    public int bounceCloneDamage;
    public int pierceCloneDamage;
    public float bounceDashCooldown;
    public float pierceDashCooldown;
    public float genericDashCooldown; //A cooldown that applies to both dashes
    public int hearts;
    public float speed; //Movement speed with WASD
    public float dashSpeed; //This determines the speed of the dash combined with initialDashTime
    private float dashTime;
    public float initialDashTime; //This controls how long the dash lasts and is combined with dashSpeed
    public float bouncePower; //KnockBack on bounceDash.
    public float haltTimeStart;//This determines how long the halt lasts.
    public float knockBackPower; //Determines how far the player is knocked away from enemies when they take damage.
    public float haltTime;
    public float invulnTimeStart;
    private float invulnTime;

    void Start()
    {
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
        dashCooldown = false;
        isPierceDashing = 0;
        isBounceDashing = 0;
        spaceDash = false;
        hearts = 5;
        healthbar.setMaxHealth(hearts);
        healthbar.setHealth(hearts);
    }

    // Update is called once per frame
    void Update()
    {
        // Get Mouse cursor position relative to player and turn it into a unit vector
        playerPos = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector3(cursorPos.x - transform.position.x, cursorPos.y - transform.position.y);
        direction = direction.normalized;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        // Get all enemies
        enemyColliders = GameObject.FindGameObjectsWithTag("Enemy").OfType<GameObject>().ToList();

        // Check how full your halt bar is
        if (enemyHits >= 5)
        {
            enemyHits = 5;
            haltCooldown = false;
        }

        // Start dash when right and left mouse buttons are pressed
        if (PauseMenu.isPaused == false) // These things can only happen when the game is not paused
        {
            if (Input.GetMouseButtonDown(1) && (isHalting == 0) && (isBounceDashing == 0))
            {
                //GameObject bLine = Instantiate(bounceLine, GameObject.FindGameObjectsWithTag("Player")[0].transform.position, rotation);
                //if (Input.GetMouseButtonUp(1))
                //{
                //Destroy(bLine);
                isPierceDashing = 1;
               // }
            }
            else if (Input.GetMouseButtonDown(0) && (isHalting == 0) && (isPierceDashing == 0))
            {

                //if (Input.GetMouseButtonUp(0))
                //{
                    isBounceDashing = 1;
                //}
            }
            else if ((Input.GetKeyDown(KeyCode.Space)) && (haltCooldown == false) && spaceDash == false)
            {
                CreateDust3();
                isHalting = 1;
                for (int i = 1; i < 6; i++)     // Make the bar count down for 5s
                {
                    Invoke("reduceHits", i);
                }
            }
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
        else if (isPierceDashing == 1 && pierceCooldown == false && dashCooldown == false)
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
                dashCooldown = true;
                Invoke("pierceDashCD", pierceDashCooldown);
                Invoke("dashCD", genericDashCooldown);
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
        else if (isBounceDashing == 1 && bounceCooldown == false && dashCooldown == false)
        {
            CreateDust();
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                dashTime = initialDashTime;
                isBounceDashing = 0;
                bounceCooldown = true;
                dashCooldown = true;
                Invoke("bounceDashCD", bounceDashCooldown);
                Invoke("dashCD", genericDashCooldown);
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
                healthbar.setHealth(hearts);
                invuln = 1;
                knockBackDir = new Vector2(transform.position.x - other.gameObject.GetComponent<Transform>().position.x, transform.position.y - other.gameObject.GetComponent<Transform>().position.y);
                rb.AddForce(knockBackDir * knockBackPower);
                if (hearts < 1)
                {
                    Invoke("mainMenu", 2.5f);
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

    void CreateDust3()
    {
        haltDust.Play();
    }

    void bounceDashCD()
    {
        bounceCooldown = false;
    }

    void pierceDashCD()
    {
        pierceCooldown = false;
    }

    void dashCD()
    {
        dashCooldown = false;
    }

    void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void reduceHits()
    {
        enemyHits--;
    }

}
