using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ShopMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }   

    public void BuyMaxHealth()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(',');
        readPerm.Close();
        int tempCurrency = Int32.Parse(tempCurrencyTxtList[0]);
        int tempMaxHealth = Int32.Parse(tempCurrencyTxtList[1]);
        if (tempCurrency >= 50)
        {
            tempCurrency -= 50;
            tempMaxHealth += 1;
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write(tempCurrency.ToString() + "," + tempMaxHealth.ToString() + "," + tempCurrencyTxtList[2] + "," + tempCurrencyTxtList[3] + "," + tempCurrencyTxtList[4] + "," + tempCurrencyTxtList[5]);
            writePerm.Close();
        }
    }
}
