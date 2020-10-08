using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //ToDo's
    // add an event called GameSpeed and update gameSpeed to whom it may concern
    public delegate void UpdateCanvis(int Points, int Coins);
    public static event UpdateCanvis AllScores;
    public delegate void GameOver();
    public static event GameOver PlayerDied;
    public delegate void Player(bool NoAmo);
    public static event Player playerOutOfAmo;
    public delegate void AmoCollected(int PlusAmo);
    public static event AmoCollected PlusAmoCount;

    [SerializeField]
    private int playerCoins = 0;
    private int playerScore = 0;

    private IEnumerator StartPowerUpT;
    private GameObject applyHitBy;
    private bool MultiShot = false;
    private bool Shilds = false;
    private bool Speed = false;
    private float BenefitTime = 10;
    void Start()
    {
        PlayerMoves.playerState += GameisOver;
        PowerUp.Collected += EnablePowerUp;
        EnemyMoves.KillingEnemy += UpdateScores;
        Astroid.Points += UpdateScores;
        PlayerMoves.playerOutOfAmo += PlayerOutOfAmo;
        UpdateScores(playerScore);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void EnablePowerUp(GameObject ApplyHitBy, int PowerUpID) // 1 = tripple shot, 2 = shilds, 3 = speed, 4 = coins
    {
        //enable a timmer here for power up time
        //access bolts filed on hitBy
            StartPowerUpT = StartPowerUpTimer(ApplyHitBy);
            switch (PowerUpID)
            {
                case 1:
                if (ApplyHitBy.GetComponent<BoldsFire>() != null) 
                {
                    ApplyHitBy.GetComponentInChildren<BoldsFire>().MultiShot(Random.Range(2, 4));
                    BenefitTime = 30;
                    MultiShot = true;
                }
                    break;
                case 2:
                // Player Shilds
                if (ApplyHitBy.GetComponentInChildren<PlayerMoves>() != null)
                {

                    ApplyHitBy.GetComponentInChildren<PlayerMoves>().Shields(true); 
                }
                    Shilds = true;
                    break;
                case 3:
                if (ApplyHitBy.GetComponentInChildren<PlayerMoves>() != null)
                {
                    ApplyHitBy.GetComponentInChildren<PlayerMoves>().GameControllerSpeed(2);
                    BenefitTime = 60;
                    Speed = true; 
                }
                break;
            case 4:
                playerCoins++;
                UpdateScores(0);
                break;
            case 5:
                if(ApplyHitBy.GetComponentInChildren<PlayerMoves>() != null)
                {
                    int RangeAmo = Random.Range(15, 30);
                    PlusAmoCount?.Invoke(RangeAmo);
                    playerOutOfAmo(false);
                }
                break;
            case 6:
                if(ApplyHitBy.GetComponent<PlayerMoves>() != null)
                {
                    ApplyHitBy.GetComponent<PlayerMoves>().FirstAidKit();
                }
                break;
            }
        if (!Shilds)
        {
            StartCoroutine(StartPowerUpT);
        }
        if (Shilds) Shilds = false;
        //
    }

    IEnumerator StartPowerUpTimer(GameObject HitBy)
    {
        yield return new WaitForSecondsRealtime(10);
        if (MultiShot)
        {
            HitBy.GetComponentInChildren<BoldsFire>().MultiShot(1);
            MultiShot = false;
        }
        if (Speed)
        {
            HitBy.GetComponent<PlayerMoves>().GameControllerSpeed(1);
            Speed = false;
        }
        StopCoroutine(StartPowerUpT);
        
    }

    private void UpdateScores(int _pointsWorth)
    {
        playerScore += _pointsWorth;
        AllScores?.Invoke(playerScore, playerCoins);
    }

    private void OnDestroy()
    {
        PowerUp.Collected -= EnablePowerUp;
        EnemyMoves.KillingEnemy -= UpdateScores;
    }

    private void GameisOver()
    {
        PlayerDied?.Invoke();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    private void PlayerOutOfAmo()
    {
        playerOutOfAmo(true);
    }
}
