using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryDisplayUI;
    public static bool inInventoryMenu = false;
    public GameObject playerTracker;

    void Start()
    {
        playerTracker = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.inPauseMenu == false && UpgradeScreen.inUpgradeMenu == false)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (PauseMenu.isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        inventoryDisplayUI.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        inInventoryMenu = false;
        playerTracker.GetComponent<PlayerController>().dashCooldown = true;
        playerTracker.GetComponent<PlayerController>().Invoke("dashCD", 0.1f);
    }

    void Pause()
    {
        inventoryDisplayUI.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
        inInventoryMenu = true;
    }
}
