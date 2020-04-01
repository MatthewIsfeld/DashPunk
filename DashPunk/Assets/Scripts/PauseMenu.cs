using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool inPauseMenu = false;
    public GameObject pauseMenuUI;
    public GameObject playerTracker;

    void Start()
    {
        playerTracker = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryMenu.inInventoryMenu == false && UpgradeScreen.inUpgradeMenu == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
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
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        inPauseMenu = false;
        playerTracker.GetComponent<PlayerController>().dashCooldown = true;
        playerTracker.GetComponent<PlayerController>().Invoke("dashCD", 0.1f);
    }

    void Pause()
    {
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        inPauseMenu = true;
        playerTracker.GetComponent<PlayerController>().dashCooldown = true;
        playerTracker.GetComponent<PlayerController>().Invoke("dashCD", 0.1f);
    }

    public void LoadMenu()
    {
        isPaused = false;
        inPauseMenu = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

