using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionControl : MonoBehaviour
{
    private float explosionTime;
    public float startExplosionTime;
    private GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        explosionTime = startExplosionTime;
        playerObject = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
       if (explosionTime > 0)
        {
            explosionTime -= Time.deltaTime;
            if (playerObject != null)
            {
                if (playerObject.GetComponent<PlayerController>().isHalting == 1)
                {
                    this.gameObject.GetComponent<Animator>().enabled = false;
                }
                else
                {
                    this.gameObject.GetComponent<Animator>().enabled = true;
                }
            }
        } else
        {
            if (playerObject != null)
            {
                if (playerObject.GetComponent<PlayerController>() != null)
                {
                    if (playerObject.GetComponent<PlayerController>().isHalting == 0)
                    {
                        destroyExplosion();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //playerObject.GetComponent<PlayerController>().hearts -= 1; sort of works
        }
    }

    void destroyExplosion()
    {
        Destroy(gameObject);
    }
}
