using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class REnemyControl : MonoBehaviour
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
    public float stoppingDistance;
    public float retreatDistance;
    private float shootCooldown;
    public float startShootCooldown;
    public GameObject bullet;
    public Transform firePoint;
    public Transform topWall;
    public Transform leftWall;
    public Transform rightWall;
    public Transform bottomWall;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        invuln = 0;
        invulnTime = invulnTimeStart;
        bounced = 0;
        playerObject = GameObject.Find("Player");
        shootCooldown = startShootCooldown;
        Player = playerObject.GetComponent<Transform>();
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
        if (playerObject != null)
        {
            if (shootCooldown <= 0 && playerObject.GetComponent<PlayerController>().isHalting == 0)
            {
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                shootCooldown = startShootCooldown;
            }
            else
            {
                shootCooldown -= Time.deltaTime;
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
    }

    // FixedUpdate moves enemy towards player
    void FixedUpdate()
    {
        moveEnemy(movement); 
    }

    void moveEnemy(Vector2 direction)
    {
        if (playerObject != null)
        {
            if (playerObject.GetComponent<PlayerController>().isHalting == 0)
            {
                if (Vector2.Distance(transform.position, Player.position) > stoppingDistance)
                {
                    rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
                }
                else if (Vector2.Distance(transform.position, Player.position) < stoppingDistance && Vector2.Distance(transform.position, Player.position) > retreatDistance)
                {
                    rb.MovePosition(this.transform.position);
                }
                else if (Vector2.Distance(transform.position, Player.position) < retreatDistance && playerObject.GetComponent<PlayerController>().isPierceDashing == 0)
                {
                    if ((Vector2.Distance(transform.position, topWall.localPosition) > 15) && (Vector2.Distance(transform.position, leftWall.localPosition) > 15) && (Vector2.Distance(transform.position, rightWall.localPosition) > 15) && (Vector2.Distance(transform.position, bottomWall.localPosition) > 15))
                    {
                        rb.MovePosition((Vector2)transform.position + (direction * -moveSpeed * Time.deltaTime));
                    } else
                    {
                        rb.MovePosition(this.transform.position);
                    }
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
                PlayerController.enemyHits++; // Increment the halting bar.
                hearts -= 1;
                // Play blood animation
                // CreateBlood();
                invuln = 1;
                if (hearts <= 0)
                {
                    //CreateBlood();
                    WaveSpawner.totalEnemies -= 1;
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
            }
            hearts -= 1;
            // Play blood animation
            //CreateBlood();
            invuln = 1;
            if (hearts <= 0)
            {
                //CreateBlood();
                WaveSpawner.totalEnemies -= 1;
                this.gameObject.SetActive(false);
            }
        }

        if ((invuln == 0) && (other.gameObject.CompareTag("BounceClone")) && (BounceCloneScript.cloneBouncing == 1))
        {
            PlayerController.enemyHits++;
            bounced = 1;
            hearts -= 1;
            invuln = 1;
            if (hearts <= 0)
            {
                WaveSpawner.totalEnemies -= 1;
                this.gameObject.SetActive(false);
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
                        //CreateBlood();
                        WaveSpawner.totalEnemies -= 1;
                        this.gameObject.SetActive(false);
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
