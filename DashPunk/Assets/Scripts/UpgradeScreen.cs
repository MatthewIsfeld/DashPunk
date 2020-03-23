using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScreen : MonoBehaviour
{
    public static bool isUpgrading = false;
    public GameObject upgradeScreenUI;
    public static bool inUpgradeMenu = false;
    public Button[] buttons;
    public Sprite[] upgrades;

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

    public void selectButtons()
    {
        int[] chosenNumbers = new int[3];
        int randVal;
        bool duplicate = true;

        for (int i = 0; i < 3; i++)
        {
            duplicate = true;
            while (duplicate == true)
            {
                duplicate = false;
                randVal = Random.Range(1, 6);
                for (int j = 0; j < 3; j++)
                {
                    if(chosenNumbers[j] == randVal)
                    {
                        duplicate = true;
                    }
                }

                if (duplicate == false)
                {
                    chosenNumbers[i] = randVal;
                    buttons[i].GetComponent<Image>().sprite = upgrades[randVal - 1];
                }
            }
        }
    }
}
