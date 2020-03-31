using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekingControl : MonoBehaviour
{
    private Transform Player;
    public float speed;
    private Vector2 target;
    private GameObject playerObject;
    private Rigidbody2D rb;
    private Vector3 direction;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            playerObject = GameObject.Find("Player");
            target = new Vector2(Player.position.x, Player.position.y);
            rb = this.GetComponent<Rigidbody2D>();
            direction = Player.position - transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject != null)
        {
            target = new Vector2(Player.position.x, Player.position.y);
            direction = Player.position - transform.position;
            if (playerObject.GetComponent<PlayerController>().isHalting == 0)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle + 90;
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }            
        }       
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        destroyGrenade();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            destroyGrenade();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            destroyGrenade();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            destroyGrenade();
        }
    }

    void destroyGrenade()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
