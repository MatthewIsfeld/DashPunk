using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
    public static bool isUpgrading = false;
    public GameObject upgradeScreenUI;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        upgradeScreenUI.SetActive(false);
        Time.timeScale = 1f;
        isUpgrading = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        isUpgrading = true;
        upgradeScreenUI.SetActive(true);
    }
}
