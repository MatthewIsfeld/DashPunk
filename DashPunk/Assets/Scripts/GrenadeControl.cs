using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeControl : MonoBehaviour
{
    private Transform Player;
    public float speed;
    private Vector2 target;
    private GameObject playerObject;
    public GameObject explosion;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            playerObject = GameObject.Find("Player");
            target = new Vector2(Player.position.x, Player.position.y);
            rb = this.GetComponent<Rigidbody2D>();
            rb.freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject != null)
        {
            if (playerObject.GetComponent<PlayerController>().isHalting == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            if (transform.position.x == target.x && transform.position.y == target.y)
            {
                destroyGrenade();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        destroyGrenade();
    }

    void destroyGrenade()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
