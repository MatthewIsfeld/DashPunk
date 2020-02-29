using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCloneScript : MonoBehaviour
{
    public static int cloneBouncing = 1;
    public ParticleSystem dust;
    public GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        Physics2D.IgnoreCollision(playerObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
