using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum state { SPAWNING, WAITING, COUNTING };

    public state waveState = state.COUNTING;
    public OneWave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float nextWaveTime = 5f;
    public float waveCountDown;
    private float searchCountDown = 1f;

    [System.Serializable]
    public class OneWave
    {
        public string name;
        public GameObject enemy1;
        public int enemyCount;
        public float spawnRate;
    }

    // Start is called before the first frame update
    void Start()
    {
        waveCountDown = nextWaveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveState == state.WAITING)
        {
            //Check if enemies are still alive
            if (checkAlive() == false)
            {
                waveCompleted();
            }
            else
            {
                return;
            }
        }
        if (waveCountDown <= 0)
        {
            if (waveState != state.SPAWNING)
            {
                //Spawn wave
                StartCoroutine(spawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    IEnumerator spawnWave(OneWave theWave)
    {
        waveState = state.SPAWNING;

        for (int i = 0; i < theWave.enemyCount; i++)
        {
            spawnEnemy(theWave.enemy1);
            yield return new WaitForSeconds(1f / theWave.spawnRate);
        }

        waveState = state.WAITING;
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
        Debug.Log("Wave Completed!");
        waveState = state.COUNTING;
        waveCountDown = nextWaveTime;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("Completed All Waves! Returning to initial wave");
        }
        else
        {
            nextWave++;
        }
    }
}


