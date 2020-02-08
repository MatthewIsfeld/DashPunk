using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEnemyControl : MonoBehaviour
{
    //This code makes the enemy rotate to face the player and then move towards them
    public Transform Player;
    private Rigidbody2D rb;
    public Vector2 movement;
    public float moveSpeed = 5f;
    public int playerBounceDashing;
    public int playerPierceDashing;
    public GameObject playerObject;
    private int hearts;
    private float invuln;
    private float invulnTime;
    public float invulnTimeStart;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        hearts = 3;
        invuln = 0;
        invulnTime = invulnTimeStart;
    }

    // Update makes the enemy rotate to face the player
    void Update()
    {
        Vector3 direction = Player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        direction.Normalize();
        movement = direction;
        playerObject = GameObject.Find("Player");
        playerBounceDashing = playerObject.GetComponent<PlayerController>().isBounceDashing;
        playerPierceDashing = playerObject.GetComponent<PlayerController>().isPierceDashing;

        if (invuln == 1)
        {
            if (invulnTime <= 0)
            {
                invuln = 0;
                invulnTime = invulnTimeStart;
            } else
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
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if ((playerBounceDashing == 1 || playerPierceDashing == 1) && (invuln == 0))
            {
                hearts -= 1;
                invuln = 1;
                if (hearts <= 0)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
