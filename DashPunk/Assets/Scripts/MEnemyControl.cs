using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MEnemyControl : MonoBehaviour
{
    public Transform Player;
    private Rigidbody2D rb;
    public Vector2 movement;
    public float moveSpeed = 5f;
    public int isKnockedBack;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        isKnockedBack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        direction.Normalize();
        movement = direction;
    }

    void FixedUpdate()
    {
        moveEnemy(movement);
    }

    void moveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    void enemyKnockBack()
    {
        
    }
}
