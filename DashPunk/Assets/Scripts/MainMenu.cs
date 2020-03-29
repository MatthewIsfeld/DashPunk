using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        string path = Application.dataPath + "/PermanentUpgrades.txt";
        if (!File.Exists(path))
        {
            StreamWriter writePerm = new StreamWriter(path);
            writePerm.Write("0");
            writePerm.Close();
        }
        PlayerUpgrades.maxHealthUp = 0;
        PlayerUpgrades.clonesUpgrade = 0;
        PlayerUpgrades.dashCooldownUpgrades = 0;
        PlayerUpgrades.haltUpgrades = 0;
        PlayerUpgrades.moveSpeedUpgrade = 0;
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
