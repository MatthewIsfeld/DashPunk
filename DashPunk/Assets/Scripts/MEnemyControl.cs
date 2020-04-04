using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEnemyControl : MonoBehaviour
{
    // This code makes the enemy rotate to face the player and then move towards them.
    public Transform Player;
    private Rigidbody2D rb;
    public Vector2 movement;
    public float moveSpeed = 5f;
    public int playerBounceDashing;
    public int playerPierceDashing;
    public GameObject playerObject;
    public int hearts;
    private float invuln;
    private float invulnTime;
    public float invulnTimeStart;
    public int bounced;
    private Vector2 bounceDir;
    public ParticleSystem blood;
    public static bool isHalted = false;
    public GameObject HealthDrop;
    public GameObject HealthUpgrade;
    public GameObject clonesUpgrade;
    public GameObject dashCDDUpgrade;
    public GameObject haltUpgrade;
    public GameObject moveSpeedUpgrade;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Color defaultCol;
    public GameObject currency;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        invuln = 0;
        invulnTime = invulnTimeStart;
        bounced = 0;
        playerObject = GameObject.Find("Player");
        Player = playerObject.GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultCol = spriteRenderer.color;
    }

    // Update makes the enemy rotate to face the player
    void Update()
    {
        Vector3 direction = Player.position - transform.position;
       // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
       // if (isHalted == false)
        //{
           // rb.rotation = angle;
        //}
        direction.Normalize();
        movement = direction;

        if (playerObject.GetComponent<PlayerController>().isHalting == 0)
        {
            //Animation Code
            animator.enabled = true;
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        if (playerObject.GetComponent<PlayerController>().isHalting == 1)
        {
            animator.enabled = false;
        }
        if (playerObject != null)
        {
            playerBounceDashing = playerObject.GetComponent<PlayerController>().isBounceDashing;
            playerPierceDashing = playerObject.GetComponent<PlayerController>().isPierceDashing;
        }

        if (invuln == 1)
        {
            spriteRenderer.color = Color.red;
            FindObjectOfType<AudioManager>().Play("enemyHit");
            if (invulnTime <= 0)
            {
                spriteRenderer.color = defaultCol;
                invuln = 0;
                invulnTime = invulnTimeStart;
                bounced = 0;
            }
            else
            {
                invulnTime -= Time.deltaTime;
            }
        }
    }

    // FixedUpdate moves enemy towards player
    void FixedUpdate()
    {
        moveEnemy(movement);
    }

    // Move enemy with MovePosition
    void moveEnemy(Vector2 direction)
    {
        if (playerObject.GetComponent<PlayerController>().isHalting == 0)
        {
            rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PierceClone"))
        {
            if ((invuln == 0))
            {
                PlayerController.enemyHits++; // Increment the halting bar.
                hearts -= playerObject.GetComponent<PlayerController>().pierceCloneDamage;
                // Play blood animation
                // CreateBlood();
                invuln = 1;
                if (hearts <= 0)
                {
                    death();
                }
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (playerPierceDashing == 1 && invuln == 0)
            {
                PlayerController.enemyHits++; // Increment the halting bar.
                hearts -= playerObject.GetComponent<PlayerController>().pierceDamage;
                // Play blood animation
                //CreateBlood();
                invuln = 1;
                if (hearts <= 0)
                {
                    death();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {        
        if ((playerBounceDashing == 1) && (invuln == 0) && (other.gameObject.CompareTag("Player")))
        {
            PlayerController.enemyHits++; // Increment the halting bar.
            bounced = 1;
            hearts -= playerObject.GetComponent<PlayerController>().bounceDamage;
            // Play blood animation
            //CreateBlood();
            invuln = 1;
            if (hearts <= 0)
            {
                death();
            }
        }
        if ((invuln == 0) && (other.gameObject.CompareTag("BounceClone")) && (BounceCloneScript.cloneBouncing == 1))
        {
            PlayerController.enemyHits++; // Increment the halting bar.
            bounced = 1;
            hearts -= playerObject.GetComponent<PlayerController>().bounceCloneDamage;
            invuln = 1;
            if (hearts <= 0)
            {
                death();
            }
        }


        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<REnemyControl>() != null)
            {
                if ((other.gameObject.GetComponent<REnemyControl>().bounced == 1) && (invuln == 0))
                {
                    PlayerController.enemyHits++;
                    hearts -= 1;
                    //CreateBlood();
                    invuln = 1;
                    bounced = 1;
                    bounceDir = PlayerController.direction;
                    rb.AddForce(bounceDir * 15000);
                    if (hearts <= 0)
                    {
                        death();
                    }
                }
            }
            if (other.gameObject.GetComponent<MEnemyControl>() != null)
            {
                if ((other.gameObject.GetComponent<MEnemyControl>().bounced == 1) && (invuln == 0))
                {
                    PlayerController.enemyHits++;
                    hearts -= 1;
                    //CreateBlood();
                    invuln = 1;
                    bounced = 1;
                    bounceDir = PlayerController.direction;
                    rb.AddForce(bounceDir * 15000);
                    if (hearts <= 0)
                    {
                        death();
                    }
                }
            }
            if (other.gameObject.GetComponent<GEnemyControl>() != null)
            {
                if ((other.gameObject.GetComponent<GEnemyControl>().bounced == 1) && (invuln == 0))
                {
                    PlayerController.enemyHits++;
                    hearts -= 1;
                    //CreateBlood();
                    invuln = 1;
                    bounced = 1;
                    bounceDir = PlayerController.direction;
                    rb.AddForce(bounceDir * 15000);
                    if (hearts <= 0)
                    {
                        death();
                    }
                }
            }
            if (other.gameObject.GetComponent<BossControl>() != null)
            {
                if ((other.gameObject.GetComponent<BossControl>().bounced == 1) && (invuln == 0))
                {
                    PlayerController.enemyHits++;
                    hearts -= 1;
                    //CreateBlood();
                    invuln = 1;
                    bounced = 1;
                    bounceDir = PlayerController.direction;
                    rb.AddForce(bounceDir * 15000);
                    if (hearts <= 0)
                    {
                        death();
                    }
                }
            }
            if (other.gameObject.GetComponent<FinalBossControl>() != null)
            {
                if ((other.gameObject.GetComponent<FinalBossControl>().bounced == 1) && (invuln == 0))
                {
                    PlayerController.enemyHits++;
                    hearts -= 1;
                    //CreateBlood();
                    invuln = 1;
                    bounced = 1;
                    bounceDir = PlayerController.direction;
                    rb.AddForce(bounceDir * 15000);
                    if (hearts <= 0)
                    {
                        death();
                    }
                }
            }
        }
    }   
    void CreateBlood()
    {
        blood.Play();
    }

    void spawnHealthDrop()
    {
        Instantiate(HealthDrop, this.transform.position, new Quaternion(0, 0, 0, 0));
    }

    void spawnHealthUpgrade()
    {
        Instantiate(HealthUpgrade, this.transform.position, new Quaternion(0, 0, 0, 0));
    }

    void spawnClonesUpgrade()
    {
        Instantiate(clonesUpgrade, this.transform.position, new Quaternion(0, 0, 0, 0));
    }

    void spawnDashCDDUpgrade()
    {
        Instantiate(dashCDDUpgrade, this.transform.position, new Quaternion(0, 0, 0, 0));
    }

    void spawnHaltUpgrade()
    {
        Instantiate(haltUpgrade, this.transform.position, new Quaternion(0, 0, 0, 0));
    }

    void spawnMoveSpeedUpgrade()
    {
        Instantiate(moveSpeedUpgrade, this.transform.position, new Quaternion(0, 0, 0, 0));
    }

    void spawnCurrency()
    {
        Instantiate(currency, this.transform.position, new Quaternion(0, 0, 0, 0));
    }

    void death()
    {
        int randVal;
        randVal = Random.Range(0, 100);
        Spawner.totalEnemies -= 1;
        SpawnerEnd.totalEnemies -= 1;
        //CreateBlood();
        if (randVal <= 20)
        {
            spawnHealthDrop();
        }
        else if (randVal > 20 && randVal <= 25)
        {
            spawnHealthUpgrade();
        } 
        else if (randVal > 25 && randVal <= 30)
        {
            spawnClonesUpgrade();
        }
        else if (randVal > 30 && randVal <= 35)
        {
            spawnDashCDDUpgrade();
        }
        else if (randVal > 35 && randVal <= 40)
        {
            spawnHaltUpgrade();
        }
        else if (randVal > 40 && randVal <= 45)
        {
            spawnMoveSpeedUpgrade();
        }
        else if (randVal > 67)
        {
            spawnCurrency();
        }
        this.gameObject.SetActive(false);
    }
}
