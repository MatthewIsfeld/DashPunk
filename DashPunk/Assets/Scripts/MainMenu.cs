using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Boolean invincibility;
    public Text invincibilityTxt;
    void Start()
    {
        Cursor.visible = true;
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        if (!File.Exists(path))
        {
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write("0,0,0,0,0,0"); //Add a 0, for each upgrade
            writePerm.Close();
        }
        invincibility = false;
        invincibilityTxt.color = Color.red;
    }

    public void PlayGame ()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(','); // Current length is 6
        readPerm.Close();
        if (invincibility == false)
        {
            PlayerUpgrades.maxHealthUp = 0 + Int32.Parse(tempCurrencyTxtList[1]);
        } else
        {
            PlayerUpgrades.maxHealthUp = 2000 + Int32.Parse(tempCurrencyTxtList[1]);
        }
        PlayerUpgrades.clonesUpgrade = 0 + Int32.Parse(tempCurrencyTxtList[2]);
        PlayerUpgrades.dashCooldownUpgrades = 0 + Int32.Parse(tempCurrencyTxtList[3]);
        PlayerUpgrades.haltUpgrades = 0 + Int32.Parse(tempCurrencyTxtList[4]);
        PlayerUpgrades.moveSpeedUpgrade = 0 + Int32.Parse(tempCurrencyTxtList[5]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void toggleInvincibility()
    {
        if (invincibility == false)
        {
            invincibility = true;
            invincibilityTxt.color = Color.green;
        } else
        {
            invincibility = false;
            invincibilityTxt.color = Color.red;
        }
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
