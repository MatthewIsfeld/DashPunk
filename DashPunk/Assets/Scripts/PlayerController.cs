using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using System;

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
    public bool dashCooldown;
    private bool haltCooldown;
    public static float enemyHits;
    public Rigidbody2D tempBody;
    private bool spaceDash;
    public HealthBar healthbar;
    public GameObject bounceLine;
    public GameObject pierceLine;
    public GameObject mouse;
    public Animator animator;
    public Text maxHealthUpTxt;
    public Text clonesUpTxt;
    public Text dashCDDUpTxt;
    public Text haltUpTxt;
    public Text moveSpeedUpTxt;
    public SpriteRenderer spriteRenderer;
    public bool bossFight;

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
        FindObjectOfType<AudioManager>().Play("bgm");
        rb = GetComponent<Rigidbody2D>();
        initialDashTime = 0.10f;
        dashTime = initialDashTime;
        deadText.text = "";
        deadText.fontSize = 50;
        enemyColliders = GameObject.FindGameObjectsWithTag("Enemy").OfType<GameObject>().ToList();
        invuln = 0;
        invulnTimeStart = 0.3f;
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
        hearts = 10 + PlayerUpgrades.maxHealthUp;
        maxHealth = hearts;
        healthbar.setMaxHealth(maxHealth);
        healthbar.setHealth(hearts);
        clonesAllowed = calcClonesAllowed(); //Function should work
        maxHealthUpTxt.text = PlayerUpgrades.maxHealthUp.ToString();
        clonesUpTxt.text = PlayerUpgrades.clonesUpgrade.ToString();
        dashCDDUpTxt.text = PlayerUpgrades.dashCooldownUpgrades.ToString();
        haltUpTxt.text = PlayerUpgrades.haltUpgrades.ToString();
        moveSpeedUpTxt.text = PlayerUpgrades.moveSpeedUpgrade.ToString();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockBackPower = 5000;
        bouncePower = 20000;
        dashSpeed = 70;
        speed = calcSpeed(); //Function should work
        genericDashCooldown = 0.6f;
        pierceDashCooldown = 0.8f * Mathf.Pow(0.95f, PlayerUpgrades.dashCooldownUpgrades);
        bounceDashCooldown = 0.8f * Mathf.Pow(0.95f, PlayerUpgrades.dashCooldownUpgrades);
        bounceDamage = 1;
        pierceDamage = 1;
        bounceCloneDamage = 1;
        pierceCloneDamage = 1;
        haltBarMax = calcHaltBarMax(); //Function should work
        bossFight = false;
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
                bounceLine.SetActive(false);
                pierceLine.transform.position = this.transform.position;
                pierceLine.transform.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
                pierceLine.SetActive(true);
            }
            if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
            {
                pierceLine.SetActive(false);
                if ((isHalting == 0) && (isBounceDashing == 0) && pierceCooldown == false && dashCooldown == false)
                {
                    invuln = 1;
                    FindObjectOfType<AudioManager>().Play("pierceSound");
                    isPierceDashing = 1;
                }
            }
            if (Input.GetMouseButton(0) && !Input.GetMouseButton(1)) // Can only happen when right click is not held.
            {
                pierceLine.SetActive(false);
                bounceLine.transform.position = this.transform.position;
                bounceLine.transform.rotation = Quaternion.AngleAxis(angle + 45, Vector3.forward);
                bounceLine.SetActive(true);
            }
            if (Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1)) // Can only happen when right click is not held.
            {
                bounceLine.SetActive(false);
                if ((isHalting == 0) && (isPierceDashing == 0) && bounceCooldown == false && dashCooldown == false)
                {
                    invuln = 1;
                    FindObjectOfType<AudioManager>().Play("bounceSound");
                    isBounceDashing = 1;
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Space)) && (haltCooldown == false) && spaceDash == false)
            {
                FindObjectOfType<AudioManager>().Play("halting");
                if (bossFight)
                {
                    Debug.Log("paused boss music because bossFight is true");
                    FindObjectOfType<AudioManager>().Pause("boss");
                }
                else
                {
                    Debug.Log("paused bgm music because bossFight is false");
                    FindObjectOfType<AudioManager>().Pause("bgm");
                }
                CreateDust3();
                isHalting = 1;
                Invoke("resumeHalt", 3);
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
                 tempEnemyCollider.isTrigger = true;
             } 
            //this.GetComponent<Collider2D>().isTrigger = true;
            CreateDust2();
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                for (int i = 0; i < enemyColliders.Count; i++)
                {
                    tempEnemyCollider = enemyColliders[i].GetComponent<Collider2D>();
                    tempEnemyCollider.isTrigger = false;
                }
                //this.GetComponent<Collider2D>().isTrigger = false;
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
                FindObjectOfType<AudioManager>().Play("playerHit");
                healthbar.setHealth(hearts);
                invuln = 1;
                knockBackDir = new Vector2(transform.position.x - other.gameObject.GetComponent<Transform>().position.x, transform.position.y - other.gameObject.GetComponent<Transform>().position.y);
                rb.AddForce(knockBackDir * knockBackPower);
                if (hearts < 1)
                {
                    Invoke("mainMenu", 2.5f);
                    deadText.color = Color.white;
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
            FindObjectOfType<AudioManager>().Play("collect");
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
            FindObjectOfType<AudioManager>().Play("collect");
            if ((isBounceDashing == 0) && (isPierceDashing == 0))
            {
                maxHealth += 1;
                healthbar.setMaxHealth(maxHealth);
                hearts += 1;
                healthbar.setHealth(hearts);
                PlayerUpgrades.maxHealthUp += 1;
                maxHealthUpTxt.text = PlayerUpgrades.maxHealthUp.ToString();
                other.gameObject.SetActive(false);
            }
        }
        if (other.gameObject.CompareTag("+ClonesUpgrade"))
        {
            FindObjectOfType<AudioManager>().Play("collect");
            clonesAllowed += 1;
            PlayerUpgrades.clonesUpgrade += 1;
            clonesUpTxt.text = PlayerUpgrades.clonesUpgrade.ToString();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("DashCDDUpgrade"))
        {
            FindObjectOfType<AudioManager>().Play("collect");
            pierceDashCooldown = 0.95f * pierceDashCooldown;
            bounceDashCooldown = 0.95f * bounceDashCooldown;
            PlayerUpgrades.dashCooldownUpgrades += 1;
            dashCDDUpTxt.text = PlayerUpgrades.dashCooldownUpgrades.ToString();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("HaltUpgrade"))
        {
            FindObjectOfType<AudioManager>().Play("collect");
            if (haltBarMax > 5)
            {
                haltBarMax -= 1;
            } else
            {
                clonesAllowed += 1;
            }
            PlayerUpgrades.haltUpgrades += 1;
            haltUpTxt.text = PlayerUpgrades.haltUpgrades.ToString();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("MoveSpeedUpgrade"))
        {
            FindObjectOfType<AudioManager>().Play("collect");
            speed = 1.05f * speed;
            PlayerUpgrades.moveSpeedUpgrade += 1;
            moveSpeedUpTxt.text = PlayerUpgrades.moveSpeedUpgrade.ToString();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("BossCurrency"))
        {
            FindObjectOfType<AudioManager>().Play("collect");
            string path = Application.dataPath + "/PermanentUpgrades.txt";
            StreamReader readPerm = new StreamReader(path);
            string tempCurrencyTxt = readPerm.ReadLine();
            string[] tempCurrencyTxtList = tempCurrencyTxt.Split(','); // Current length is 6
            readPerm.Close();
            int tempCurrency = Int32.Parse(tempCurrencyTxtList[0]);
            tempCurrency += 100;
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write(tempCurrency.ToString() + "," + tempCurrencyTxtList[1] + "," + tempCurrencyTxtList[2] + "," + tempCurrencyTxtList[3] + "," + tempCurrencyTxtList[4] + "," + tempCurrencyTxtList[5]);
            writePerm.Close();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Explosion"))
        {
            FindObjectOfType<AudioManager>().Play("playerHit");
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

    float calcSpeed()
    {
        float temp = 10;
        int length = PlayerUpgrades.moveSpeedUpgrade;

        for (int i = 0; i < length; i++)
        {
            temp = temp * 1.05f;
        }

        return temp;
    }

    int calcHaltBarMax()
    {
        int temp = PlayerUpgrades.haltUpgrades;
        int retVal;

        if (temp <= 5)
        {
            retVal = 10 - temp;
        } else
        {
            retVal = 5;
        }

        return retVal;
    }

    int calcClonesAllowed()
    {
        int retVal;
        int temp = PlayerUpgrades.haltUpgrades;

        if (temp > 5)
        {
            retVal = 4 + PlayerUpgrades.clonesUpgrade;
            retVal += (temp - 5);
        } else
        {
            retVal = 4 + PlayerUpgrades.clonesUpgrade;
        }

        return retVal;
    }

    public void resumeHalt()
    {
        FindObjectOfType<AudioManager>().Play("resume");
    }
}
