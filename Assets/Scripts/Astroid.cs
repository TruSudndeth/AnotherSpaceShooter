using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    public delegate void AstroidPoints(int worth);
    public static event AstroidPoints Points;

    [SerializeField]
    private float SpinSpeed = 2;
    [SerializeField]
    private int PointsWorth = 5;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioClip;

    private float gameSpeed = 1;
    private Animator Explodes;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Explodes = GetComponent<Animator>();
        SpinSpeed = Random.Range(20f, 40f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(Vector3.forward * SpinSpeed * Time.deltaTime);
        transform.position += Vector3.down * gameSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        AnimationClip[] ClipLength = Explodes.runtimeAnimatorController.animationClips;
        if (collision.tag == "Player")
        {
            PlayerMoves PlayerScript  = collision.GetComponent<PlayerMoves>();
            if (PlayerScript != null) PlayerScript.PlayerHit();
            Explods(ClipLength[0]);
        }
        if(collision.tag == "PlayerBolts" || collision.tag == "PowerLaser")
        {
            Destroy(collision.gameObject);
            Points(PointsWorth);
            Explods(ClipLength[0]);
        }
    }

    private void GameSpeed(int _gameSpeed)
    {
        gameSpeed = _gameSpeed;
    }

    private void Explods(AnimationClip ClipLength)
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        Explodes.SetTrigger("Explode");
        _audioSource.clip = _audioClip;
        _audioSource.Play();
        Destroy(gameObject, ClipLength.length);
    }
}
