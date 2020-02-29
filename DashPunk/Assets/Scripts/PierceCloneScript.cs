using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceCloneScript : MonoBehaviour
{
    public static Collider2D pCloneCollider;
    public ParticleSystem dust;

    // Start is called before the first frame update
    void Start()
    {
        pCloneCollider = this.GetComponent<Collider2D>();
        pCloneCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

}
