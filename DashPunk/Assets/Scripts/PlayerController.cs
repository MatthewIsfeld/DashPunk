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
    private int isDashing = 0;
    private int wounds;
    public Text woundText;
    public Text deadText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = initialDashTime;
        wounds = 2;
        woundText.text = "Wounds: " + wounds.ToString();
        deadText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector2(cursorPos.x - transform.position.x, cursorPos.y - transform.position.y);
        direction = direction.normalized;
        if (Input.GetMouseButtonDown(0))
        {
            isDashing = 1;
        }
    }

    void FixedUpdate()
    {
        if (isDashing == 0)
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            float verticalMove = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(horizontalMove, verticalMove);
            rb.velocity = movement * speed;
        }
        else
        {
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                dashTime = initialDashTime;
                isDashing = 0;
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
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            wounds -= 1;
            woundText.text = "Wounds: " + wounds.ToString();
            if (wounds < 1)
            {
                deadText.text = "YOU HAVE DIED";
                this.gameObject.SetActive(false);
            }
        }
    }
}
