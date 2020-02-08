using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Vector2 cursorPos;
    private Vector2 direction;
    public float dashSpeed;
    private float dashTime;
    public float initialDashTime;
    public int isPierceDashing = 0;
    public int isBounceDashing = 0;
    private int hearts;
    public Text heartsText;
    public Text deadText;
    Collider2D mEnemyCollider;
    public float bouncePower;
    public float invulnTimeStart;
    private float invulnTime;
    private int invuln;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = initialDashTime;
        hearts = 2;
        heartsText.text = "Hearts: " + hearts.ToString();
        deadText.text = "";
        mEnemyCollider = GameObject.Find("MeleeEnemy").GetComponent<Collider2D>();
        invuln = 0;
        invulnTime = invulnTimeStart;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Mouse cursor position relative to player and turn it into a unit vector
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector2(cursorPos.x - transform.position.x, cursorPos.y - transform.position.y);
        direction = direction.normalized;
       
        // Start dash when right and left mouse buttons are pressed
        if (Input.GetMouseButtonDown(1))
        {
            isPierceDashing = 1;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            isBounceDashing = 1;
        }
        
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

    void FixedUpdate()
    {
        // Move character when not dashing
        if ((isPierceDashing == 0) && (isBounceDashing == 0))
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            float verticalMove = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(horizontalMove, verticalMove);
            rb.velocity = movement * speed;
        }

        // Move character when pierceDashing and disable enemy collider so character can pass through
        else if (isPierceDashing == 1)
        {
            mEnemyCollider.enabled = !mEnemyCollider.enabled;
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                dashTime = initialDashTime;
                isPierceDashing = 0;             
            }
            else
            {
                rb.velocity = direction * dashSpeed;
                dashTime -= Time.fixedDeltaTime;
            }

        //Move character when BounceDashing
        } else if (isBounceDashing == 1)
        {
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                dashTime = initialDashTime;
                isBounceDashing = 0;
            }
            else
            {
                rb.velocity = direction * dashSpeed;
                dashTime -= Time.fixedDeltaTime;
            }
        }
    }

 
    void OnTriggerEnter2D(Collider2D other)
    {
        //If the player contacts an emeny they take damage if they are not dashing display damage with simple text boxes
        if (other.gameObject.CompareTag("Enemy"))
        {
            if ((isPierceDashing == 0) && (isBounceDashing == 0) && (invuln == 0))
            {
                hearts -= 1;
                heartsText.text = "Hearts: " + hearts.ToString();
                invuln = 1;
                if (hearts < 1)
                {
                    deadText.text = "YOU HAVE DIED";
                    this.gameObject.SetActive(false);
                }
            } 
            //If player bounce dashes push enemy with force
            else if (isBounceDashing == 1)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * bouncePower);
            }
        }
    }
}
