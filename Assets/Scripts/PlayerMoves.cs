using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
    //Todo's
    // player needs to explode on last life with laser hit
    // Player cool down fire's
    // play sound for shields down and hit
    public delegate void PlayerState();
    public static event PlayerState playerState;
    public delegate void NumberOfLives(int LivesLeft);
    public static event NumberOfLives LivesLeft;
    public static event NumberOfLives ShieldCount;
    public static event PlayerState playerOutOfAmo;
    public delegate void PlayerGotDamaged();
    public static event PlayerGotDamaged PlayerGotHit;


    [SerializeField]
    private int Lives = 3;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private GameObject shieldsEnabled;
    [SerializeField]
    private GameObject PowerLaser;
    [SerializeField]
    private GameObject PowerLaserFX;
    [SerializeField]
    private GameObject LeftDam;
    [SerializeField]
    private GameObject RightDam;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private AudioClip _playerdied;
    [SerializeField]
    private GameObject PlusPoints;
    [SerializeField]
    private HUD_Boost HudBoost;

    private int shieldCount = 0;
    private Vector3 bounds;
    private float inputX;
    private float inputY;
    private Vector3 applyInput;
    private float speedMulti = 1;
    private float shiftSpeed = 1f;
    private bool shields = false;
    private bool invulnerable = false;
    private IEnumerator InvulnerableCoroutine;
    private IEnumerator powerLaserCoolDown;
    private Animator playerAnim;
    private int amoCount = 15;
    void Start()
    {
        GameController.PlusAmoCount += AmoCollected;
        _audioSource = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        if (playerAnim == null) Debug.Log("this is emplty");
        inputX = transform.position.x;
        inputY = transform.position.y;
        CheckBounds();
        ApplyInputs();
        LivesLeft?.Invoke(Lives);
        ShieldCount?.Invoke(shieldCount);
        PlayerDamaged();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            BasicMovementInput();
            CheckBounds();
            ApplyInputs();
        }
        if(Input.GetKeyDown(KeyCode.Space) && amoCount >= 0)
        {
            GetComponentInChildren<BoldsFire>().Fire();
            _audioSource.clip = _audioClip;
            _audioSource.Play();
            amoCount--;
            if(amoCount == 0)
            {
                // let game controller know im out of amo and spawn some
                playerOutOfAmo?.Invoke();
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(!HudBoost.IsCoolingDown)shiftSpeed = 1.5f;
            HudBoost.IsBoosting = true;
        }
        else
        {
            shiftSpeed = 1;
            HudBoost.IsBoosting = false;
        }
    }

    private void BasicMovementInput()
    {

        inputX += Input.GetAxis("Horizontal") * speedMulti * speed * Time.deltaTime * shiftSpeed;
        inputY += Input.GetAxis("Vertical") * speedMulti * speed * Time.deltaTime * shiftSpeed;
    }
    private void CheckBounds()
    {
        inputY = Mathf.Clamp(inputY, -5.5f, 4.5f);
        if (inputX >  10.8f) inputX = -10.8f;
        if (inputX < -10.8f) inputX = 10.8f;
        //inputX = Mathf.Clamp(inputX, -10.8f, 10.8f);
        applyInput = new Vector3(inputX, inputY, 0);
    }
    private void ApplyInputs()
    {
        transform.position = applyInput;
    }
    public void PlayerHit()
    {
        if (shields)
        {
            shieldCount--;
            ShieldCount(shieldCount);
            if (shieldCount == 0)
            {
                shields = false;
                shieldsEnabled.SetActive(false);
            }
        }

        else if (!invulnerable && !shields)
        {
            InvulnerableCoroutine = HitCoolDown();
            StartCoroutine(InvulnerableCoroutine);
            Lives--;
            PlayerGotHit?.Invoke();
            PlayerDamaged();
            LivesLeft(Lives);
        }
        if(Lives <= 0)
        {
            playerState?.Invoke();
            _audioSource.clip = _playerdied;
            _audioSource.Play();
            GameController.PlusAmoCount -= AmoCollected;
            HudBoost.IsBoosting = false;
            Destroy(gameObject);
        }
    }
    public void Shields(bool _shilds)
    {
        shieldCount = 3;
        ShieldCount(shieldCount);
        shieldsEnabled.SetActive(true);
        shields = _shilds;
    }
    public void GameControllerSpeed(float _speed)
    {
        speedMulti = _speed;
    }

    private IEnumerator HitCoolDown()
    {
        BoxCollider2D playerHurt = gameObject.GetComponent<BoxCollider2D>();
        playerHurt.enabled = false;
        playerAnim.SetBool("Injured", true);
        invulnerable = true;
        yield return new WaitForSecondsRealtime(2);
        invulnerable = false;
        playerAnim.SetBool("Injured", false);
        playerHurt.enabled = true;
        StopCoroutine(InvulnerableCoroutine);
    }

    private void PlayerDamaged()
    {
        switch(Lives)
        {
            case 2:
                LeftDam.SetActive(true);
                RightDam.SetActive(false);
                break;
            case 1:
                RightDam.SetActive(true);
                LeftDam.SetActive(true);
                break;
            case 3:
                LeftDam.SetActive(false);
                RightDam.SetActive(false);
                break;
        }

    }
    public void AmoCollected(int _NewAmoCount)
    {
        amoCount += _NewAmoCount;
        GameObject _PlusPoints = Instantiate(PlusPoints, transform.position, Quaternion.identity);
        _PlusPoints.GetComponent<PlusPoints>().Points = _NewAmoCount;
    }
    private IEnumerator PowerLaserCoolDown()
    {
        yield return new WaitForSecondsRealtime(5);
        PowerLaser.SetActive(false);
        PowerLaserFX.SetActive(false);
        GetComponentInChildren<BoldsFire>().gameObject.tag = "PlayerBolts";
        StopCoroutine(powerLaserCoolDown);
    }

    public void FirstAidKit()
    {
        Lives++;
        if (Lives > 3) Lives = 3;
        LivesLeft(Lives);
        PlayerDamaged();
    }
    public void EnablePowerLaser()
    {
        GetComponentInChildren<BoldsFire>().gameObject.tag = "PowerLaser";
        PowerLaser.gameObject.SetActive(true);
        PowerLaserFX.SetActive(true);
        powerLaserCoolDown = PowerLaserCoolDown();
        StartCoroutine(powerLaserCoolDown);
    }
}
