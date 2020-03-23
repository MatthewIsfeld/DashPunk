using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScreen : MonoBehaviour
{
    public static bool isUpgrading = false;
    public GameObject upgradeScreenUI;
    public static bool inUpgradeMenu = false;
    public Button upgrade1;
    public Button upgrade2;
    public Button upgrade3;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        upgradeScreenUI.SetActive(false);
        Time.timeScale = 1f;
        isUpgrading = false;
        PauseMenu.isPaused = false;
        inUpgradeMenu = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        isUpgrading = true;
        upgradeScreenUI.SetActive(true);
        PauseMenu.isPaused = true;
        inUpgradeMenu = true;
    }
}
