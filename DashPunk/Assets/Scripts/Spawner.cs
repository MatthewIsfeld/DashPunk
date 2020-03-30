using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public int waveState = 1; //1 is between waves, 2 is during wave, 3 is creating wave
    public OneWave[] waves;
    private int nextWave = 0;
    public Animator animator;

    public Transform[] spawnPoints;

    public float nextWaveTime = 5f;
    public float waveCountDown;
    private float searchCountDown = 1f;
    public Text enemiesText;
    public static int totalEnemies;
    public GameObject waveEnd;
    public Text goNext;
    private bool levelEnd;
    

    [System.Serializable]
    public class OneWave
    {
        public string name;
        public GameObject enemy1;
        public GameObject enemy2;
        public GameObject enemy3;
        public int enemy1Count;
        public int enemy2Count;
        public int enemy3Count;
        public float spawnRate;
    }

    // Start is called before the first frame update
    void Start()
    {
        waveCountDown = nextWaveTime;
        enemiesText.text = "";
        enemiesText.fontSize = 24;
        nextWave = 0;
        waveState = 1;
        nextWaveTime = 5f;
        searchCountDown = 1f;
        goNext.text = "";
        levelEnd = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (levelEnd == false)
        {
            if (waveState == 2)
            {
                //Check if enemies are still alive
                if (checkAlive() == false)
                {
                    waveCompleted();
                }
                else
                {
                    enemiesText.text = "Enemies Remaining: " + totalEnemies;
                    return;
                }
            }
            if (waveCountDown <= 0)
            {
                if (waveState != 3)
                {
                    //Spawn wave
                    StartCoroutine(spawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountDown -= Time.deltaTime;
            }
        } else if (levelEnd == true)
        {
            if (UpgradeScreen.isUpgrading == false)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    ChangeLevel();
                }
            }
        }
    }

    IEnumerator spawnWave(OneWave theWave)
    {
        waveState = 3;
        totalEnemies = theWave.enemy1Count + theWave.enemy2Count + theWave.enemy3Count;
        enemiesText.text = "Enemies Remaining: " + totalEnemies;

        for (int i = 0; i < theWave.enemy1Count; i++)
        {
            spawnEnemy(theWave.enemy1);
            yield return new WaitForSeconds(1f / theWave.spawnRate);
        }
        for (int i = 0; i < theWave.enemy2Count; i++)
        {
            spawnEnemy(theWave.enemy2);
            yield return new WaitForSeconds(1f / theWave.spawnRate);
        }
        for (int i = 0; i < theWave.enemy3Count; i++)
        {
            spawnEnemy(theWave.enemy3);
            yield return new WaitForSeconds(1f / theWave.spawnRate);
        }

        waveState = 2;
        yield break;
    }

    void spawnEnemy(GameObject enemy)
    {
        Transform spawnLoc = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnLoc.position, spawnLoc.rotation);
    }

    bool checkAlive()
    {
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    void waveCompleted()
    {
        if (nextWave + 1 > waves.Length - 1)
        {
            waveEnd.GetComponent<UpgradeScreen>().Pause();
            waveEnd.GetComponent<UpgradeScreen>().selectButtons();
            waveState = 2;
            waveCountDown = nextWaveTime;
            nextWave = waves.Length;
            Debug.Log("Changing to next stage!");

            goNext.text = "Press N to go to next Level!";
            levelEnd = true;
        }
        else
        {
            Debug.Log("game paused to go to upgrade screen.");
            waveEnd.GetComponent<UpgradeScreen>().Pause();
            waveEnd.GetComponent<UpgradeScreen>().selectButtons();
            Debug.Log("Wave Completed!");
            waveState = 1;
            waveCountDown = nextWaveTime;
            nextWave++;
        }
    }

    void ChangeLevel()
    {
        StartCoroutine(LevelCoordinate(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LevelCoordinate(int num)
    {
        animator.SetInteger("SceneTransition", 1);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(num);
    }
}


