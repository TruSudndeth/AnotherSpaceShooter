﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
    //Todo's
    // Player cool down fire's
    // play sound for shields down and hit
    public delegate void PlayerState();
    public static event PlayerState playerState;
    public delegate void NumberOfLives(int LivesLeft);
    public static event NumberOfLives LivesLeft;
    public static event NumberOfLives ShieldCount;

    [SerializeField]
    private int Lives = 3;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private GameObject shieldsEnabled;
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

    private int shieldCount = 0;
    private Vector3 bounds;
    private float inputX;
    private float inputY;
    private Vector3 applyInput;
    private float speedMulti = 1;
    private bool shields = false;
    private bool invulnerable = false;
    private IEnumerator InvulnerableCoroutine;
    private Animator playerAnim;
    void Start()
    {
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponentInChildren<BoldsFire>().Fire();
            _audioSource.clip = _audioClip;
            _audioSource.Play();
        }
    }

    private void BasicMovementInput()
    {

        inputX += Input.GetAxis("Horizontal") * speedMulti * speed * Time.deltaTime;
        inputY += Input.GetAxis("Vertical") * speedMulti * speed * Time.deltaTime;
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
            PlayerDamaged();
            LivesLeft(Lives);
            Debug.Log("this ran lives --");
        }
        if(Lives <= 0)
        {
            playerState?.Invoke();
            _audioSource.clip = _playerdied;
            _audioSource.Play();
            Destroy(gameObject);
        }
    }
    public void Shields(bool _shilds)
    {
        shieldCount++;
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
}
