﻿using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        invuln = 0;
        invulnTime = invulnTimeStart;
        bounced = 0;
        playerObject = GameObject.Find("Player");
        shootCooldown = startShootCooldown;
    }

    // Update makes the enemy rotate to face the player
    void Update()
    {       
        if (playerObject.GetComponent<PlayerController>().isHalting == 0)
        {
            Vector3 direction = Player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (isHalted == false)
            {
                rb.rotation = angle;
            }
            if (Vector2.Distance(transform.position, Player.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, Player.position, moveSpeed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, Player.position) < stoppingDistance && Vector2.Distance(transform.position, Player.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, Player.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, Player.position, -moveSpeed * Time.deltaTime);
            }
        }
        if (shootCooldown <= 0 && playerObject.GetComponent<PlayerController>().isHalting == 0)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            shootCooldown = startShootCooldown;
        } else
        {
            shootCooldown -= Time.deltaTime;
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
                this.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("Enemy"))
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
                    this.gameObject.SetActive(false);
                }
            }
        }
    }




    void CreateBlood()
    {
        blood.Play();
    }
}