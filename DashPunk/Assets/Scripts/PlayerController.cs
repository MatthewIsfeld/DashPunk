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
    Vector2 movement;
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
    public static float enemyHits;
    public Rigidbody2D tempBody;
    private bool spaceDash;
    public HealthBar healthbar;
    public GameObject bounceLine;
    public GameObject pierceLine;
    public GameObject mouse;
    public Animator animator;
    public int[] inventoryCount = new int[5]; // Array size is # of upgrade types, 0 - Max Health, 1 - Clones Up, 2 - Dash CD
                                              // 3 - Halt Up, 4 - Move Speed Up
    public Text maxHealthUpTxt;
    public Text clonesUpTxt;
    public Text dashCDDUpTxt;
    public Text haltUpTxt;
    public Text moveSpeedUpTxt;
    public SpriteRenderer spriteRenderer;

    //Parameters for upgrades
    public int bounceDamage;
    public int pierceDamage;
    public int bounceCloneDamage;
    public int pierceCloneDamage;
    public float bounceDashCooldown;
    public float pierceDashCooldown;
    public float genericDashCooldown; //A cooldown that applies to both dashes
    public int hearts;
    private int maxHealth;
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
    public int clonesAllowed;
    public static int haltBarMax;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialDashTime = 0.10f;
        dashTime = initialDashTime;
        deadText.text = "";
        deadText.fontSize = 50;
        enemyColliders = GameObject.FindGameObjectsWithTag("Enemy").OfType<GameObject>().ToList();
        invuln = 0;
        invulnTimeStart = 1;
        invulnTime = invulnTimeStart;
        haltTimeStart = 5;
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
        maxHealth = hearts;
        healthbar.setMaxHealth(maxHealth);
        healthbar.setHealth(hearts);
        clonesAllowed = 4;
        maxHealthUpTxt.text = inventoryCount[0].ToString();
        clonesUpTxt.text = inventoryCount[1].ToString();
        dashCDDUpTxt.text = inventoryCount[2].ToString();
        haltUpTxt.text = inventoryCount[3].ToString();
        moveSpeedUpTxt.text = inventoryCount[4].ToString();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockBackPower = 5000;
        bouncePower = 20000;
        dashSpeed = 70;
        speed = 10;
        genericDashCooldown = 0.6f;
        pierceDashCooldown = 0.8f;
        bounceDashCooldown = 0.8f;
        bounceDamage = 1;
        pierceDamage = 1;
        bounceCloneDamage = 1;
        pierceCloneDamage = 1;
        haltBarMax = 10;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Mouse cursor position relative to player and turn it into a unit vector
        playerPos = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector2(cursorPos.x - transform.position.x, cursorPos.y - transform.position.y);
        direction = direction.normalized;

        var dir = cursorPos - playerPos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Animation stuff
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Get all enemies
        enemyColliders = GameObject.FindGameObjectsWithTag("Enemy").OfType<GameObject>().ToList();

        // Check how full your halt bar is
        if (enemyHits >= haltBarMax)
        {
            enemyHits = haltBarMax;
            haltCooldown = false;
        }

        // Start dash when right and left mouse buttons are pressed
        if (PauseMenu.isPaused == false) // These things can only happen when the game is not paused
        {
            if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
            {
                pierceLine.transform.position = this.transform.position;
                pierceLine.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
                pierceLine.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
            {
                pierceLine.SetActive(false);
                if ((isHalting == 0) && (isBounceDashing == 0) && pierceCooldown == false && dashCooldown == false)
                {
                    invuln = 1;
                    isPierceDashing = 1;
                }
            }
            if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                bounceLine.transform.position = this.transform.position;
                bounceLine.transform.rotation = Quaternion.AngleAxis(angle + 45, Vector3.forward);
                bounceLine.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
            {
                bounceLine.SetActive(false);
                if ((isHalting == 0) && (isPierceDashing == 0) && bounceCooldown == false && dashCooldown == false)
                {
                    invuln = 1;
                    isBounceDashing = 1;
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Space)) && (haltCooldown == false) && spaceDash == false)
            {
                CreateDust3();
                isHalting = 1;
                for (int i = 1; i < 6; i++)     // Make the bar count down
                {
                    Invoke("reduceHits", i);
                }
            }
        }

        if (invuln == 1)
        {
            spriteRenderer.color = Color.gray; 
            if (invulnTime <= 0)
            {
                spriteRenderer.color = Color.white;
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
                enemyHits = 0;

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
        else if (isPierceDashing == 1)
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
                for (int i = 0; i < enemyColliders.Count; i++)
                {
                    tempEnemyCollider = enemyColliders[i].GetComponent<Collider2D>();
                    tempEnemyCollider.enabled = true;
                }
            }
            else
            {
                spaceDash = true;
                rb.velocity = direction * dashSpeed;
                dashTime -= Time.fixedDeltaTime;
            }


            // Move character when BounceDashing
        }
        else if (isBounceDashing == 1)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HealthDrop"))
        {
            if ((isBounceDashing == 0) && (isPierceDashing == 0))
            {
                if (hearts < maxHealth)
                {
                    hearts += 1;
                    healthbar.setHealth(hearts);
                }
                other.gameObject.SetActive(false);
            }
        }
        if (other.gameObject.CompareTag("MaxHealthUpgrade"))
        {
            if ((isBounceDashing == 0) && (isPierceDashing == 0))
            {
                maxHealth += 1;
                healthbar.setMaxHealth(maxHealth);
                hearts += 1;
                healthbar.setHealth(hearts);
                inventoryCount[0] += 1;
                maxHealthUpTxt.text = inventoryCount[0].ToString();
                other.gameObject.SetActive(false);
            }
        }
        if (other.gameObject.CompareTag("+ClonesUpgrade"))
        {
            clonesAllowed += 1;
            inventoryCount[1] += 1;
            clonesUpTxt.text = inventoryCount[1].ToString();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("DashCDDUpgrade"))
        {
            pierceDashCooldown = 0.8f * pierceDashCooldown;
            bounceDashCooldown = 0.8f * bounceDashCooldown;
            inventoryCount[2] += 1;
            dashCDDUpTxt.text = inventoryCount[2].ToString();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("HaltUpgrade"))
        {
            if (haltBarMax > 5)
            {
                haltBarMax -= 1;
            } else
            {
                clonesAllowed += 1;
            }
            inventoryCount[3] += 1;
            haltUpTxt.text = inventoryCount[3].ToString();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("MoveSpeedUpgrade"))
        {
            speed = 1.05f * speed;
            inventoryCount[4] += 1;
            moveSpeedUpTxt.text = inventoryCount[4].ToString();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Explosion"))
        {
            if ((isPierceDashing == 0) && (isBounceDashing == 0) && (invuln == 0))
            {
                hearts -= 1;
                healthbar.setHealth(hearts);
                invuln = 1;
                if (hearts < 1)
                {
                    Invoke("mainMenu", 2.5f);
                    deadText.text = "THE PUNK'S JOURNEY IS OVER";
                    this.gameObject.SetActive(false);
                }
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
        float reduction = haltBarMax / 5;
        enemyHits -= reduction;
    }


}
