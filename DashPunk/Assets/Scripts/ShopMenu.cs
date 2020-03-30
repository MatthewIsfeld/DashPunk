using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public Text creditNum;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(',');
        readPerm.Close();
        creditNum.text = "# of Chips: " + tempCurrencyTxtList[0].ToString();
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
            creditNum.text = "# of Chips: " + tempCurrency.ToString();
            tempMaxHealth += 1;
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write(tempCurrency.ToString() + "," + tempMaxHealth.ToString() + "," + tempCurrencyTxtList[2] + "," + tempCurrencyTxtList[3] + "," + tempCurrencyTxtList[4] + "," + tempCurrencyTxtList[5]);
            writePerm.Close();
        }
    }

    public void BuyClones()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(',');
        readPerm.Close();
        int tempCurrency = Int32.Parse(tempCurrencyTxtList[0]);
        int tempClones = Int32.Parse(tempCurrencyTxtList[2]);
        if (tempCurrency >= 50)
        {
            tempCurrency -= 50;
            creditNum.text = "# of Chips: " + tempCurrency.ToString();
            tempClones += 1;
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write(tempCurrency.ToString() + "," + tempCurrencyTxtList[1] + "," + tempClones.ToString() + "," + tempCurrencyTxtList[3] + "," + tempCurrencyTxtList[4] + "," + tempCurrencyTxtList[5]);
            writePerm.Close();
        }
    }

    public void BuyDashCooldown()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(',');
        readPerm.Close();
        int tempCurrency = Int32.Parse(tempCurrencyTxtList[0]);
        int tempCooldown = Int32.Parse(tempCurrencyTxtList[3]);
        if (tempCurrency >= 50)
        {
            tempCurrency -= 50;
            creditNum.text = "# of Chips: " + tempCurrency.ToString();
            tempCooldown += 1;
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write(tempCurrency.ToString() + "," + tempCurrencyTxtList[1] + "," + tempCurrencyTxtList[2] + "," + tempCooldown.ToString() + "," + tempCurrencyTxtList[4] + "," + tempCurrencyTxtList[5]);
            writePerm.Close();
        }
    }

    public void BuyHaltBar()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(',');
        readPerm.Close();
        int tempCurrency = Int32.Parse(tempCurrencyTxtList[0]);
        int tempHaltBar = Int32.Parse(tempCurrencyTxtList[4]);
        if (tempCurrency >= 50 && tempHaltBar < 5)
        {
            tempCurrency -= 50;
            creditNum.text = "# of Chips: " + tempCurrency.ToString();
            tempHaltBar += 1;
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write(tempCurrency.ToString() + "," + tempCurrencyTxtList[1] + "," + tempCurrencyTxtList[2] + "," + tempCurrencyTxtList[3] + "," + tempHaltBar.ToString() + "," + tempCurrencyTxtList[5]);
            writePerm.Close();
        }
    }

    public void BuyMoveSpeed()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        StreamReader readPerm = new StreamReader(path);
        string tempCurrencyTxt = readPerm.ReadLine();
        string[] tempCurrencyTxtList = tempCurrencyTxt.Split(',');
        readPerm.Close();
        int tempCurrency = Int32.Parse(tempCurrencyTxtList[0]);
        int tempMoveSpeed = Int32.Parse(tempCurrencyTxtList[5]);
        if (tempCurrency >= 50)
        {
            tempCurrency -= 50;
            creditNum.text = "# of Chips: " + tempCurrency.ToString();
            tempMoveSpeed += 1;
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write(tempCurrency.ToString() + "," + tempCurrencyTxtList[1] + "," + tempCurrencyTxtList[2] + "," + tempCurrencyTxtList[3] + "," + tempCurrencyTxtList[4] + "," + tempMoveSpeed.ToString());
            writePerm.Close();
        }
    }
}
