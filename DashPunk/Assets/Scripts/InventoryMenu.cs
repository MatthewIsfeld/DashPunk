using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryDisplayUI;
    public static bool inInventoryMenu = false;
    public GameObject playerTracker;
    public Text currencyText;

    void Start()
    {
        playerTracker = GameObject.Find("Player");
        string path = Application.persistentDataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(','); // Current length is 6
        readPerm.Close();
        currencyText.text = "# of Chips: " + tempCurrencyTxtList[0];
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
        if (inInventoryMenu == true)
        {
            string path = Application.persistentDataPath + "/PermanentUpgrades.txt";
            StreamReader readPerm = new StreamReader(path);
            string tempCurrencyTxt = readPerm.ReadLine();
            string[] tempCurrencyTxtList = tempCurrencyTxt.Split(','); // Current length is 6
            readPerm.Close();
            currencyText.text = "# of Chips: " + tempCurrencyTxtList[0];
        }
    }

    public void Resume()
    {
        playerTracker.GetComponent<PlayerController>().bounceLine.SetActive(false);
        playerTracker.GetComponent<PlayerController>().pierceLine.SetActive(false);
        Cursor.visible = false;
        inventoryDisplayUI.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        inInventoryMenu = false;
        playerTracker.GetComponent<PlayerController>().dashCooldown = true;
        playerTracker.GetComponent<PlayerController>().Invoke("dashCD", 0.1f);
    }

    void Pause()
    {
        Cursor.visible = true;
        inventoryDisplayUI.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
        inInventoryMenu = true;
    }
}
