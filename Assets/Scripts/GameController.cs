using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //ToDo's
    // add an event called GameSpeed and update gameSpeed to whom it may concern
    // destroy Collectables when out of map
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
    private float PowerUpTime = 0;
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
                if (ApplyHitBy.GetComponentInChildren<BoldsFire>() != null) 
                {
                    ApplyHitBy.GetComponentInChildren<BoldsFire>().MultiShot(3);
                    PowerUpTime = 30;
                    MultiShot = true;
                }
                    break;
                case 2:
                // Player Shilds
                if (ApplyHitBy.GetComponent<PlayerMoves>() != null)
                {
                    ApplyHitBy.GetComponent<PlayerMoves>().Shields(true);
                }
                    Shilds = true;
                    break;
                case 3:
                if (ApplyHitBy.GetComponent<PlayerMoves>() != null)
                {
                    ApplyHitBy.GetComponent<PlayerMoves>().GameControllerSpeed(2);
                    PowerUpTime = 30;
                    Speed = true; 
                }
                break;
            case 4:
                playerCoins++;
                UpdateScores(0);
                break;
            case 5:
                if(ApplyHitBy.GetComponent<PlayerMoves>() != null)
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
            case 7:
                if(ApplyHitBy.GetComponent<PlayerMoves>() != null)
                {
                    ApplyHitBy.GetComponent<PlayerMoves>().EnablePowerLaser();
                }
                break;
            }
        if (!Shilds)    StartCoroutine(StartPowerUpT);
        if (Shilds)     Shilds = false;
    }

    IEnumerator StartPowerUpTimer(GameObject HitBy)
    {
        yield return new WaitForSeconds(PowerUpTime);
        if (MultiShot)
        {
            if (HitBy != null)
            {
                HitBy.GetComponentInChildren<BoldsFire>().MultiShot(1);
                HitBy.GetComponent<PlayerMoves>().NegativeEffects(NPlayerFX.SlowShooter); 
            }
            MultiShot = false;
        }
        if (Speed)
        {
            if (HitBy != null)
            {
                HitBy.GetComponent<PlayerMoves>().GameControllerSpeed(1);
                HitBy.GetComponent<PlayerMoves>().NegativeEffects(NPlayerFX.SlowMover);
            }
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

    public void GameisOver()
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
