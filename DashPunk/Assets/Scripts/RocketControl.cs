﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControl : MonoBehaviour
{
    private Transform Player;
    public float speed;
    private GameObject playerObject;
    private Rigidbody2D rb;
    private Vector3 direction;
    public GameObject explosion;
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
            rb.rotation = angle + 90;
            direction = direction.normalized;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (playerObject != null)
        {
            if (playerObject.GetComponent<PlayerController>().isHalting == 0)
            {
                rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
            }
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            destroyBullet();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            destroyBullet();
        }
    }

    void destroyBullet()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
