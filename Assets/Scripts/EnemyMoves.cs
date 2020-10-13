using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class EnemyMoves : MonoBehaviour
{
    //ToDo's
    // Enemy interpolates to player from enemy pool after death.
    //
    public delegate void PointsFrom(int Worth);
    public static event PointsFrom KillingEnemy;
    public static event PointsFrom Wave_EnemyCount;
    [SerializeField]
    private int PointsWorth = 10;
    [SerializeField]
    private float TrackDuration = 3f;
    private float t = 0;
    private float t2 = 0;
    private float trackInt = 0f;
    [SerializeField]
    private int Lives = 1;
    private float intervals = 7;
    private float speed = 1;
    private float gameSpeed = 1;
    private float lerpSpeed = 1;
    private bool shipLerps = false;
    private bool HardTracking = false;
    private GameObject PlayerShip;
    [SerializeField]
    private Vector3 lastPlayerShip;
    private Vector3 lastPosition = Vector3.zero;
    private int NumOfBolts = 0;
    [SerializeField]
    private Vector3 applyNewPosition;
    private bool _enemyWins = false;
    private bool _enemyDead = false;
    private bool enemyStoped = false;
    private bool fireAtPlayer = true;
    private Animator EnemyAnim;
    private BoldsFire Fire;
    private bool stopShooting = false;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _Shoot;
    [SerializeField]
    private AudioClip _Explode;
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        Fire = gameObject.GetComponentInChildren<BoldsFire>();
        EnemyAnim = GetComponent<Animator>();
        lastPosition = transform.position;
        PlayerShip = GameObject.Find("Player");
        PlayerMoves.playerState += PlayerDied;
        StartCoroutine(StartShooting());
        //transform.position = RandomPosition();

    }
    void Update()
    {
        if (!enemyStoped) //Update Ship Position
        {
            transform.position = MoveShip();
        }
        if (transform.position.y < -7 && !_enemyWins)
        {
            transform.position = RandomPosition();
        }
        if(_enemyWins && transform.position.y < -7)
        {
            transform.position = new Vector3(12, 7, 0);
            enemyStoped = true;
        }
    }

    private Vector3 RandomPosition()
    {
        intervals = 7;
        return new Vector3(Random.Range(-10.0f , 10.0f), intervals, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            PlayerMoves temp = other.GetComponent<PlayerMoves>();
            if(temp != null)
            {
                other.GetComponentInParent<PlayerMoves>().PlayerHit();
            }
            Wave_EnemyCount?.Invoke(1);
            DestroyShip();
        }
    }

    private void PlayerDied()
    {
        _enemyWins = true;
        StopAllCoroutines();
    }

    private void UnSubscribe()
    {
        PlayerMoves.playerState -= PlayerDied;
    }

    public void EnemyHit(GameObject KilledBy)
    {
        Lives--;
        if(Lives <= 0)
        {
            if(KilledBy != null)
            {
                KillingEnemy?.Invoke(PointsWorth);
            }
            Wave_EnemyCount?.Invoke(1);
            DestroyShip();
        }
    }
    IEnumerator StartShooting()
    {
        // random shoot 1 or 2 bolts
        while (true)
        {
            NumOfBolts = Random.Range(1, 3);
            yield return new WaitForSecondsRealtime(Random.Range(3.5f, 5.5f));
            // lerp speed random .rand in seconds
            //lastPosition = transform.position;
            lerpSpeed = Random.Range(2f, 5f) * gameSpeed;
            // lerp move twords player
            RandomEnemyMovement();
            while (NumOfBolts > 0 && StopShootingPlayer() && !_enemyDead)
            {
                NumOfBolts--;
                _audioSource.clip = _Shoot;
                _audioSource.Play();
                Fire.Fire();
                yield return new WaitForSecondsRealtime(.25f);
            } 
        }
    }

    private Vector3 MoveShip()
    {
        // player ship reference 
        intervals += -1 * speed * Time.deltaTime * gameSpeed;

        if (shipLerps)
        {
            t +=  (lerpSpeed * Time.deltaTime) / (Mathf.Abs(lastPosition.x) + Mathf.Abs(lastPlayerShip.x));
            if(t > 1)
            {
                t = 0.0f;
                shipLerps = false;
                lastPosition = transform.position;
            }
        }
        if(HardTracking)
        {
            t2 += (lerpSpeed * Time.deltaTime) / (Mathf.Abs(transform.position.x) + Mathf.Abs(lastPlayerShip.x));
            if (PlayerShip != null) lastPlayerShip = PlayerShip.transform.position;
            trackInt = (lerpSpeed * Time.deltaTime);
            if(lastPlayerShip.x - transform.position.x < -(trackInt))
            {
                if(trackInt > 0)trackInt *= -1;
                lastPosition.x += trackInt;
            }
            if (lastPlayerShip.x - transform.position.x > trackInt)
            {
                if (trackInt < 0) trackInt *= -1;
                lastPosition.x += trackInt;
            }
            if (t2 > 10)
            {
                HardTracking = false;
                trackInt = 0f;
                lastPosition = transform.position;
            }
        }
        return new Vector3(Mathf.Lerp(lastPosition.x, lastPlayerShip.x, t), intervals, 0);
    }

    private void RandomEnemyMovement()
    {
        int RandomMove = Random.Range(1, 101);
        if(RandomMove >= 30)  // 7 out of 10 will not hardTrack 
        {
            shipLerps = true;
            HardTracking = false;
            if (PlayerShip != null) lastPlayerShip = PlayerShip.transform.position;
        }
        else 
        {
            lerpSpeed = 5f;
            HardTracking = true;
            shipLerps = false;
        }
    }

    public void GameControllerSpeed(float _speed)
    {
        gameSpeed = _speed;
    }

    private bool StopShootingPlayer()
    {
        if (PlayerShip != null)
        {
            if (PlayerShip.transform.position.y < transform.position.y) fireAtPlayer = true; 
        }
        else fireAtPlayer = false;
        return fireAtPlayer;
    }

    private void DestroyShip()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        EnemyAnim.SetBool("Destroyed", true);
        AnimationClip[] currentClip = EnemyAnim.runtimeAnimatorController.animationClips;
        UnSubscribe();
        _audioSource.clip = _Explode;
        _audioSource.Play();
        _enemyDead = true;
        Destroy(gameObject, currentClip[0].length);
    }

    private void OnDestroy()
    {
        PlayerMoves.playerState -= PlayerDied;
    }
}
