using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //ToDo's
    //
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private GameObject CausticEnemy;
    [SerializeField]
    private float SpawnDelay = 1;
    private float EnemySpawnDelay = 2.5f;
    private float Intervals = 0;
    private int NumOfEnemy = 3;
    private int EnemiesSpawned;
    private int EnemiesKilled = 0;
    private int AtWave = 5;
    private bool gameOver = false;
    private bool stopCoroutine = false;
    private GameObject[] Enemies;
    void Start()
    {
        Enemies = new GameObject[] { Enemy, CausticEnemy };
        Intervals = EnemySpawnDelay / AtWave;
        PlayerMoves.playerState += GameOver;
        EnemyMoves.Wave_EnemyCount += EnemyCount;
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        if(stopCoroutine)
        {
            StopAllCoroutines();
            stopCoroutine = false;
        }
        if (EnemiesKilled >= NumOfEnemy)
        {
            EnemiesKilled = 0;
            AtWave--;
            SpawnDelay = 5;
            NumOfEnemy += 5;
            if(EnemySpawnDelay > Intervals)EnemySpawnDelay -= Intervals;
            if (EnemySpawnDelay <= Intervals)EnemySpawnDelay = Intervals;
            StartCoroutine(SpawnEnemies());
        } 
        if(AtWave <= 0)
        {
            gameOver = true;
            StopAllCoroutines();
            GetComponent<GameController>().GameisOver();
        }
    }

    private IEnumerator SpawnEnemies()
    {
            
            EnemiesSpawned = NumOfEnemy;
            yield return new WaitForSecondsRealtime(SpawnDelay);
            while (EnemiesSpawned > 0 && !gameOver)
            {
                EnemiesSpawned--;
                yield return new WaitForSeconds(EnemySpawnDelay);
                Instantiate(Enemies[RandomEnemies()], new Vector3(Random.Range(-10f, 10f), 7, 0), new Quaternion());
            }
            stopCoroutine = true;
    }

    private void GameOver()
    {
        gameOver = true;
        stopCoroutine = true;
    }

    private void EnemyCount(int currentDead)
    {
        EnemiesKilled += currentDead;
    }

    private int RandomEnemies()
    {
        int Temp = Random.Range(0,100);
        if(Temp > 25)
        {
            return 0;
        }
        else
        {
            return 1;
        }

    }
}
