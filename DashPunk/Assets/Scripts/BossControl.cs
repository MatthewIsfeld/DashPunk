using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
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
    private int bounced;
    private Vector2 bounceDir;
    public ParticleSystem blood;
    public static bool isHalted = false;
    public GameObject bullet;
    private GameObject shotBullet;
    public GameObject grenade;
    private GameObject shotGrenade;
    private float shootCooldown;
    public float startShootCooldown;
    private float grenadeCooldown;
    public float startGrenadeCooldown;
    public Transform firePoint;
    public Transform firePoint2;
    public HealthBar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        invuln = 0;
        invulnTime = invulnTimeStart;
        bounced = 0;
        playerObject = GameObject.Find("Player");
        Player = playerObject.GetComponent<Transform>();
        shootCooldown = startShootCooldown;
        grenadeCooldown = startGrenadeCooldown;
        hearts = 5;
        healthbar.setMaxHealth(hearts);
        healthbar.setActive();
    }

    // Update makes the enemy rotate to face the player
    void Update()
    {
        if (playerObject != null)
        {
            if (playerObject.GetComponent<PlayerController>().isHalting == 0)
            {
                Vector3 direction = Player.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (isHalted == false)
                {
                    rb.rotation = angle;
                }
                direction.Normalize();
                movement = direction;
            }
        }
        playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            playerBounceDashing = playerObject.GetComponent<PlayerController>().isBounceDashing;
            playerPierceDashing = playerObject.GetComponent<PlayerController>().isPierceDashing;
        }

        if (invuln == 1)
        {
            if (invulnTime <= 0)
            {
                invuln = 0;
                invulnTime = invulnTimeStart;
                bounced = 0;
            }
            else
            {
                invulnTime -= Time.deltaTime;
            }
        }

        if (playerObject != null)
        {
            if (shootCooldown <= 0 && playerObject.GetComponent<PlayerController>().isHalting == 0)
            {                                    
                shotBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                Physics2D.IgnoreCollision(shotBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                shootCooldown = startShootCooldown;                                                                             
            }
            else
            {
                shootCooldown -= Time.deltaTime;
            }
        }
        
        if (playerObject != null)
        {
            if (grenadeCooldown <= 0 && playerObject.GetComponent<PlayerController>().isHalting == 0 && shootCooldown > 0)
            {
                shotGrenade = Instantiate(grenade, firePoint2.position, firePoint2.rotation);
                Physics2D.IgnoreCollision(shotGrenade.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                grenadeCooldown = startGrenadeCooldown;
            }
            else
            {
                grenadeCooldown -= Time.deltaTime;
            }
        }
       
        healthbar.setHealth(hearts);
    }

    // FixedUpdate moves enemy towards player
    void FixedUpdate()
    {
        moveEnemy(movement);
    }

    // Move enemy with MovePosition
    void moveEnemy(Vector2 direction)
    {
        if (playerObject != null)
        {
            if (playerObject.GetComponent<PlayerController>().isHalting == 0)
            {
                if (isHalted == false)
                {
                    rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PierceClone"))
        {
            if ((invuln == 0))
            {
                hearts -= playerObject.GetComponent<PlayerController>().pierceCloneDamage;
                // Play blood animation
                // CreateBlood();
                invuln = 1;
                if (hearts <= 0)
                {
                    healthbar.setUnactive();
                    //CreateBlood();
                    Spawner.totalEnemies -= 1;
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((playerBounceDashing == 1 || playerPierceDashing == 1) && (invuln == 0) && (other.gameObject.CompareTag("Player")))
        {
            PlayerController.enemyHits++; // Increment the halting bar.
            if (playerBounceDashing == 1)
            {
                bounced = 1;
                hearts -= playerObject.GetComponent<PlayerController>().bounceDamage;
            }
            else
            {
                hearts -= playerObject.GetComponent<PlayerController>().pierceDamage;
            }
            // Play blood animation
            //CreateBlood();
            invuln = 1;
            if (hearts <= 0)
            {
                //CreateBlood();
                Spawner.totalEnemies -= 1;
                this.gameObject.SetActive(false);
                healthbar.setUnactive();
            }
        }
        if ((invuln == 0) && (other.gameObject.CompareTag("BounceClone")) && (BounceCloneScript.cloneBouncing == 1))
        {
            bounced = 1;
            hearts -= playerObject.GetComponent<PlayerController>().bounceCloneDamage;
            invuln = 1;
            if (hearts <= 0)
            {
                Spawner.totalEnemies -= 1;
                this.gameObject.SetActive(false);
                healthbar.setUnactive();
            }
        }


        if (other.gameObject.CompareTag("Enemy"))
        {
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
                        Spawner.totalEnemies -= 1;
                        //CreateBlood();
                        this.gameObject.SetActive(false);
                        healthbar.setUnactive();
                    }
                }
            }
        }
    }
    void CreateBlood()
    {
        blood.Play();
    }
}
