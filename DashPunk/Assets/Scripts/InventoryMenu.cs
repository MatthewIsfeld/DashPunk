using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryDisplayUI;

    // Update is called once per frame
    void Update()
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

    public void Resume()
    {
        inventoryDisplayUI.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }

    void Pause()
    {
        inventoryDisplayUI.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
    }
}
