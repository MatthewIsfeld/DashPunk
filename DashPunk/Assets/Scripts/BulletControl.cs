using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private Transform Player;
    public float speed;
    private Vector2 target;
    private GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerObject = GameObject.Find("Player");
        target = new Vector2(Player.position.x, Player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject.GetComponent<PlayerController>().isHalting == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
      
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            destroyBullet();
        }      
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            destroyBullet();
        }
        if (other.CompareTag("Enemy"))
        {
            destroyBullet();
        }
    }

    void destroyBullet()
    {
        Destroy(gameObject);
    }
}
