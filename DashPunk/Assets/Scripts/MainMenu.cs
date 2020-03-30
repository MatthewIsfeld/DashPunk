using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class MainMenu : MonoBehaviour
{
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
    }

    public void PlayGame ()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(','); // Current length is 6
        readPerm.Close();
        PlayerUpgrades.maxHealthUp = 0 + Int32.Parse(tempCurrencyTxtList[1]);
        PlayerUpgrades.clonesUpgrade = 0 + Int32.Parse(tempCurrencyTxtList[2]);
        PlayerUpgrades.dashCooldownUpgrades = 0 + Int32.Parse(tempCurrencyTxtList[3]);
        PlayerUpgrades.haltUpgrades = 0 + Int32.Parse(tempCurrencyTxtList[4]);
        PlayerUpgrades.moveSpeedUpgrade = 0 + Int32.Parse(tempCurrencyTxtList[5]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
