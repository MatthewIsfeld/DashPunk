using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HaltingBarScript : MonoBehaviour
{
    public float enemyHits;
    public Slider haltingBar;

    // Update is called once per frame
    void Update()
    {
        haltingBar.maxValue = PlayerController.haltBarMax;
        enemyHits = PlayerController.enemyHits;
        if (enemyHits < PlayerController.haltBarMax+1)
        {
            haltingBar.value = enemyHits;
        }
    }
}
