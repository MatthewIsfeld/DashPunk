﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private Transform Player;
    public float speed;
    private GameObject playerObject;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            playerObject = GameObject.Find("Player");
            rb = this.GetComponent<Rigidbody2D>();
            direction = Player.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            direction = direction.normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject != null)
        {
            if (playerObject.GetComponent<PlayerController>().isHalting == 0)
            {                
                rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
            }
        }
        if (Spawner.totalEnemies == 0)
        {
            destroyBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        destroyBullet();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            destroyBullet();
        }
    }

    void destroyBullet()
    {
        Destroy(gameObject);
    }
}
