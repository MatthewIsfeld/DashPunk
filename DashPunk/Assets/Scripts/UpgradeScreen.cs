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
    public GameObject[] upgrades;
    public GameObject playerTracker;

    void Start()
    {
        playerTracker = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        playerTracker.GetComponent<PlayerController>().bounceLine.SetActive(false);
        playerTracker.GetComponent<PlayerController>().pierceLine.SetActive(false);
        Cursor.visible = false;
        upgradeScreenUI.SetActive(false);
        Time.timeScale = 1f;
        isUpgrading = false;
        PauseMenu.isPaused = false;
        inUpgradeMenu = false;
        playerTracker.GetComponent<PlayerController>().dashCooldown = true;
        playerTracker.GetComponent<PlayerController>().Invoke("dashCD", 0.1f);
    }

    public void Pause()
    {
        Cursor.visible = true;
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
                    buttons[i].GetComponent<Image>().sprite = upgrades[randVal - 1].GetComponent<SpriteRenderer>().sprite;
                    buttons[i].GetComponent<Image>().color = upgrades[randVal - 1].GetComponent<SpriteRenderer>().color;
                    if (randVal == 1)
                    {
                        buttons[i].GetComponentInChildren<Text>().text = "Halt Clones increase";
                        buttons[i].onClick.RemoveAllListeners();
                        buttons[i].onClick.AddListener(Resume);
                        buttons[i].onClick.AddListener(SpawnUpgrade1);
                    } 
                    else if (randVal == 2)
                    {
                        buttons[i].GetComponentInChildren<Text>().text = "Dash CD reduction";
                        buttons[i].onClick.RemoveAllListeners();
                        buttons[i].onClick.AddListener(Resume);
                        buttons[i].onClick.AddListener(SpawnUpgrade2);
                    }
                    else if (randVal == 3)
                    {
                        buttons[i].GetComponentInChildren<Text>().text = "Halt Bar reduction";
                        buttons[i].onClick.RemoveAllListeners();
                        buttons[i].onClick.AddListener(Resume);
                        buttons[i].onClick.AddListener(SpawnUpgrade3);
                    }
                    else if (randVal == 4)
                    {
                        buttons[i].GetComponentInChildren<Text>().text = "Max HP increase";
                        buttons[i].onClick.RemoveAllListeners();
                        buttons[i].onClick.AddListener(Resume);
                        buttons[i].onClick.AddListener(SpawnUpgrade4);
                    }
                    else if (randVal == 5)
                    {
                        buttons[i].GetComponentInChildren<Text>().text = "Movement speed increase";
                        buttons[i].onClick.RemoveAllListeners();
                        buttons[i].onClick.AddListener(Resume);
                        buttons[i].onClick.AddListener(SpawnUpgrade5);
                    }
                }
            }
        }
    }

    public void SpawnUpgrade1()
    {
        Instantiate(upgrades[0], playerTracker.transform.position, new Quaternion(0, 0, 0, 0));
    }

    public void SpawnUpgrade2()
    {
        Instantiate(upgrades[1], playerTracker.transform.position, new Quaternion(0, 0, 0, 0));
    }

    public void SpawnUpgrade3()
    {
        Instantiate(upgrades[2], playerTracker.transform.position, new Quaternion(0, 0, 0, 0));
    }

    public void SpawnUpgrade4()
    {
        Instantiate(upgrades[3], playerTracker.transform.position, new Quaternion(0, 0, 0, 0));
    }

    public void SpawnUpgrade5()
    {
        Instantiate(upgrades[4], playerTracker.transform.position, new Quaternion(0, 0, 0, 0));
    }
}
