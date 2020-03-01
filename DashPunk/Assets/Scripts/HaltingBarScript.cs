using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HaltingBarScript : MonoBehaviour
{
    public int enemyHits;
    public Slider haltingBar;

    // Update is called once per frame
    void Update()
    {
        enemyHits = PlayerController.enemyHits;
        if (enemyHits < 6)
        {
            haltingBar.value = enemyHits;
        }
    }
}
