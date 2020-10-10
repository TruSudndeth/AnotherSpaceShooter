using GameDevHQ.Filebase.DataModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Image = UnityEngine.UI.Image;
using UnityEngine;
using UnityEngine.UI;

public class ScoresNPoints : MonoBehaviour
{
    //ToDo's
    //
    [SerializeField]
    private TextMeshProUGUI Score;
    [SerializeField]
    private TextMeshProUGUI Coins;
    [SerializeField]
    private TextMeshProUGUI gameOver;
    private TextMeshProUGUI flashGameOver;
    [SerializeField]
    private Button Restart;
    [SerializeField]
    private Button Menu;
    [SerializeField]
    private Image NumOfLives;
    [SerializeField]
    private Sprite[] Lives;
    [SerializeField]
    private GameObject[] Shields;
    [SerializeField]
    private TextMeshProUGUI _NoAmo;
    private int ShieldCount = 0;
    private bool isGameOver = false;
    private IEnumerator gameOverCo;

    // Start is called before the first frame update
    void Awake()
    {
        GameController.PlayerDied += GameOver;
        PlayerMoves.ShieldCount += UpdateShields;
        PlayerMoves.LivesLeft += UpdateLives;
        GameController.AllScores += RefreshScores;
        GameController.playerOutOfAmo += OutOfAmo;
        NumOfLives.sprite = Lives[3];
        UpdateShields(0);
    }

    private void RefreshScores(int _score, int _coins)
    {
        if(_score > 9999)_score = 9999;
        if (_coins > 9999) _coins = 9999;
        Score.text = "Score: " + _score;
        Coins.text = "Coins: " + _coins;
    }

    private void UpdateLives(int _lives)
    {
        _lives = LifeCount(_lives);
        NumOfLives.sprite = Lives[_lives];
    }

    private void UpdateShields(int _ShieldCount)
    {

        _ShieldCount = LifeCount(_ShieldCount);
        switch(_ShieldCount)
        {
            case 0:
                for(int i = 0; i < Shields.Length; i++)
                {
                    Shields[i].SetActive(false);
                }
                break;
            case 1:
                Shields[0].SetActive(true);
                Shields[1].SetActive(false);
                Shields[2].SetActive(false);
                break;
            case 2:
                Shields[0].SetActive(true);
                Shields[1].SetActive(true);
                Shields[2].SetActive(false);
                break;
            case 3:
                for(int i = 0; i < Shields.Length; i++)
                {
                    Shields[i].SetActive(true);
                }
                break;
        }
    }

    private int LifeCount(int clamp)
    {
        if (clamp > 3) clamp =  3;
        if (clamp < 0) clamp =  0;
        return clamp;
    }

    private void GameOver()
    {
        Menu.gameObject.SetActive(true);
        Restart.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(true);
        isGameOver = true;
        gameOverCo = FlashGameOver();
        StartCoroutine(gameOverCo);
    }

    private IEnumerator FlashGameOver()
    {
        TextMeshProUGUI flashGameOver = gameOver.GetComponent<TextMeshProUGUI>();
        bool toggle = true;
        while (flashGameOver != null)
        {
            flashGameOver.enabled = toggle;
            yield return new WaitForSecondsRealtime(0.5f);
            toggle = !toggle;
        }
    }

    private void OutOfAmo(bool _IO)
    {
        if (_IO)
        {
            _NoAmo.gameObject.SetActive(true);
            IEnumerator OutOfAmo = FlashOutOfAmo();
            StartCoroutine(OutOfAmo);
        }
        else _NoAmo.gameObject.SetActive(false);
    }
    private IEnumerator FlashOutOfAmo()
    {
        bool enable = true;
        for(int i = 0; i <= 10; i ++)
        {
            _NoAmo.enabled = enable;
            yield return new WaitForSecondsRealtime(0.1f);
            enable = !enable;
        }
        _NoAmo.enabled = true;
        StopCoroutine(FlashOutOfAmo());
    }

    private void OnDestroy()
    {
        GameController.PlayerDied -= GameOver;
        PlayerMoves.ShieldCount -= UpdateShields;
        PlayerMoves.LivesLeft -= UpdateLives;
        GameController.AllScores -= RefreshScores;
        GameController.playerOutOfAmo -= OutOfAmo;
    }
}
