﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bounceClone;
    public GameObject pierceClone;
    public GameObject playerAccess;
    public List<GameObject> bounceList = new List<GameObject>();
    public List<GameObject> pierceList = new List<GameObject>();
    public List<Vector2> bDirList = new List<Vector2>();
    public List<Vector2> pDirList = new List<Vector2>();

    void Start()
    {
        playerAccess = GameObject.Find("Player");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerAccess.GetComponent<PlayerController>().isHalting == 1) // Code for setting up bounce clones
        {
            BounceDash();
        }
        else if (Input.GetMouseButtonDown(1) && playerAccess.GetComponent<PlayerController>().isHalting == 1) // Code for setting up pierce clones
        {
            PierceDash();
        }
        if (playerAccess.GetComponent<PlayerController>().haltTime <= 0)
        {
            ExecuteClones();
        }
    }

    void BounceDash()
    {
        GameObject bClone = Instantiate(bounceClone, firePoint.position, firePoint.rotation);
        bounceList.Add(bClone);
        bDirList.Add(PlayerController.direction);
    }

    void PierceDash()
    {
        GameObject pClone = Instantiate(pierceClone, firePoint.position, firePoint.rotation);
        pierceList.Add(pClone);
        pDirList.Add(PlayerController.direction);
    }

    void ExecuteClones()
    {
        for (int i = 0; i < bounceList.Count; i++)
        {
            bounceList[i].GetComponent<Rigidbody2D>().AddForce(bDirList[i] * 70f, ForceMode2D.Impulse);
        }
        for (int i = 0; i < pierceList.Count; i++)
        {
            pierceList[i].GetComponent<Rigidbody2D>().AddForce(pDirList[i] * 70f, ForceMode2D.Impulse);
        }
        Invoke("clearClones", 0.8f);
        


    }

    void clearClones()
    {
        for (int i = 0; i < bounceList.Count; i++)
        {
            Destroy(bounceList[i]);
        }
        for (int i = 0; i < pierceList.Count; i++)
        {
            Destroy(pierceList[i]);
        }
        bounceList.Clear();
        pierceList.Clear();
        bDirList.Clear();
        pDirList.Clear();
    }


}