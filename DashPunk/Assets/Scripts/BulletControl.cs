using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private Transform Player;
    public float speed;
    //private Vector2 target; Grenade Code
    private GameObject playerObject;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerObject = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody2D>();
        //target = new Vector2(Player.position.x, Player.position.y); Grenade Code
        direction = Player.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject.GetComponent<PlayerController>().isHalting == 0)
        {
            rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
            //transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime); Grenade Code
        }
        
        /* Grande Code
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            destroyBullet();
        }
        */
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        destroyBullet();
    }

    void destroyBullet()
    {
        Destroy(gameObject);
    }
}
