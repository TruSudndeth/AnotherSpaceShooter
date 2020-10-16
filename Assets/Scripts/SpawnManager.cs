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
    private GameObject Astroid;
    [SerializeField]
    private GameObject Health;
    [SerializeField]
    private GameObject Coins;
    [SerializeField]
    private GameObject Ammo;
    [SerializeField]
    private GameObject OPLaserPickup;
    [SerializeField]
    private GameObject Shields;
    [SerializeField]
    private GameObject Speed;
    [SerializeField]
    private GameObject TrippleShot;
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
    private int TotalCoins = 100;
    private IEnumerator CoinsCoins;
    private IEnumerator _SpawnEnemies;
    void Start()
    {
        Enemies = new GameObject[] { Enemy, CausticEnemy };
        Intervals = EnemySpawnDelay / AtWave;
        PlayerMoves.playerState += GameOver;
        EnemyMoves.Wave_EnemyCount += EnemyCount;
        _SpawnEnemies = SpawnEnemies();
        StartCoroutine(_SpawnEnemies);
        CoinsCoins = CoinsCollector();
        StartCoroutine(CoinsCoins);
        StartCoroutine(PowerUps());
        StartCoroutine(ItemSpawns());
        StartCoroutine(TerrainAstroids());
        NextWaveItems();
    }

    private void Update()
    {
        if(stopCoroutine)
        {
            Debug.Log("StopCoroutine is true");
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
            Debug.Log("ATWave is lesthen or = 0");
            StopAllCoroutines();
            GetComponent<GameController>().GameisOver();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            NextWaveItems();
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
                Instantiate(Enemies[RandomEnemies()], RandomXLocation(), new Quaternion());
            }
            NextWaveItems();
            StopCoroutine(_SpawnEnemies);
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

    private IEnumerator ItemSpawns()
    {
        int _health = 0;
        int _Amm0 = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            _health = Random.Range(0, 100);
            if (_health < 10) Instantiate(Health, RandomXLocation(), Quaternion.identity);
            yield return new WaitForSeconds(5);
            _Amm0 = Random.Range(0, 100);
            if (_Amm0 < 35) Instantiate(Ammo, RandomXLocation(), Quaternion.identity);

        }
    }
    private void NextWaveItems()
    {
        // spawn two ammo's with a %50 of health. and a 35 percent chance for two health.
        int _ammo = 2;
        int _health = 2;
        int _Shields = Random.Range(0, 101);
        if (_Shields < 45) Instantiate(Shields, RandomXLocation(), Quaternion.identity);
        for (int i = 0; i < _ammo; i++)
        {
            int _Amm01 = Random.Range(0, 101);
            if (_Amm01 < 80)
            {
                Instantiate(Ammo, RandomXLocation(), Quaternion.identity);  
            }
        }
        for(int i = 0; i < _health; i++)
        {
            int _Health = Random.Range(0, 100);
            if(_Health < 50)
            {
                Instantiate(Health, RandomXLocation(), Quaternion.identity);
            }
        }
    }

    private IEnumerator CoinsCollector()
    {
        int _Coins = 0;
        while(TotalCoins > 0)
        {
            yield return new WaitForSeconds(1f);
            _Coins = Random.Range(0, 100);
            if(_Coins < 25)
            {
                Instantiate(Coins, RandomXLocation(), Quaternion.identity);
                TotalCoins--;
            }
        }
        //stop this coroutine
        StopCoroutine(CoinsCoins);
    }

    private IEnumerator TerrainAstroids()
    {
        int _Astroids = 0;
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            _Astroids = Random.Range(0, 100);
            if (_Astroids < 20)
            {
                Instantiate(Astroid, RandomXLocation(), Quaternion.identity);
            }
        }
    }

    private IEnumerator PowerUps()
    {
        // Random pickup spawn with OPLaser rare spawn.
        int _Shields = 0;
        int _Speed = 0;
        int _OpLaser = 0;
        int _TrippleShot = 0;
        bool DidSpawnedSomething = true;
        while (true)
        {
            if (DidSpawnedSomething)
            {
                yield return new WaitForSeconds(10);
                DidSpawnedSomething = false;
            }
            _Shields        = Random.Range(0, 101);
            _Speed          = Random.Range(0, 101);
            _OpLaser        = Random.Range(0, 101);
            _TrippleShot    = Random.Range(0, 101);
            if (_Shields < 15)
            {
                Instantiate(Shields, RandomXLocation(), Quaternion.identity);
                DidSpawnedSomething = true;
            }
            else if (_Speed < 20)
            {
                Instantiate(Speed, RandomXLocation(), Quaternion.identity);
                DidSpawnedSomething = true;
            }
            else if (_OpLaser < 5)
            {
                Instantiate(OPLaserPickup, RandomXLocation(), Quaternion.identity);
                DidSpawnedSomething = true;
            }

            else if (_TrippleShot < 25)
            {
                Instantiate(TrippleShot, RandomXLocation(), Quaternion.identity);
                DidSpawnedSomething = true;
            }

            else DidSpawnedSomething = false;
            yield return null;
        }
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

    private Vector3 RandomXLocation()
    {
        return new Vector3(Random.Range(-10f, 10f), 7, 0);
    }
}
