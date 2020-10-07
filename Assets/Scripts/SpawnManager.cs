using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //ToDo's
    //
    [SerializeField]
    private GameObject Enemy;
    private int NumOfEnemy = 10;
    private bool gameOver = false;
    private bool stopCoroutine = false;
    void Start()
    {
        PlayerMoves.playerState += GameOver;
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        if(stopCoroutine)
        {
            StopAllCoroutines();
            stopCoroutine = false;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while(NumOfEnemy > 0 && !gameOver)
        {
            NumOfEnemy--;
            yield return new WaitForSeconds(2.5f);
            Instantiate(Enemy, new Vector3(Random.Range(-10f, 10f), 7, 0), new Quaternion());
        }
        if(NumOfEnemy <= 0)
        {
            stopCoroutine = true;
        }
    }

    private void GameOver()
    {
        gameOver = true;
        stopCoroutine = true;
    }
}
