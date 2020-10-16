using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class EnemyMoves : MonoBehaviour
{
    //ToDo's
    // Enemy interpolates to player from enemy pool after death.
    //
    [HideInInspector]
    public bool aggressive = false;
    [HideInInspector]
    public bool AttackPlayer = false;
    [SerializeField]
    private SpriteRenderer Warning;
    private Color WarningAlpha;
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
    [SerializeField]
    private bool shipLerps = false;
    [SerializeField]
    private bool HardTracking = false;
    private GameObject PlayerShip;
    private GameObject PlayerShipPlaceHolder;
    private bool TargetLocked = false;
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
    [SerializeField]
    private EnemyTag enemyTag;
    private bool resetPosition = false;
    private IEnumerator StartFireing;
    [SerializeField]
    private GameObject EShield;
    [HideInInspector]
    public bool ShootsReverse = false;
    [HideInInspector]
    public bool DestroyPickUps = false;
    private bool AttacksPickUps = false;
    
    void Awake()
    {
        WarningAlpha = Warning.color;
        _audioSource = GetComponent<AudioSource>();
        Fire = gameObject.GetComponentInChildren<BoldsFire>();
        EnemyAnim = GetComponent<Animator>();
        lastPosition = transform.position;
        PlayerShip = GameObject.Find("Player");
        PlayerMoves.playerState += PlayerDied;
        StartFireing = StartShooting();
        StartCoroutine(StartFireing);
        ExtraLife();

    }
    private void Start()
    {
        PlayerShipPlaceHolder = PlayerShip;
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
        if(aggressive)
        {
            RaycastHit2D hit2D;
            if (!TargetLocked)
            {
                hit2D = Physics2D.CircleCast(transform.position, 3f, Vector3.down, 1, 1 << 9 | 1 << 10);
                if (hit2D)
                {
                    Debug.Log(hit2D.collider.name);
                    WarningAlpha.a += Time.deltaTime;
                    if (WarningAlpha.a > 1) WarningAlpha.a = 1;
                }
                else
                {
                    WarningAlpha.a -= Time.deltaTime;
                    if (WarningAlpha.a < 1) WarningAlpha.a = 0;
                }

                if (WarningAlpha.a >= 1)
                {
                    TargetLocked = true;
                    if (hit2D.collider.tag == "Player") gameSpeed = 5;
                    AttackPlayer = true;
                    if(DestroyPickUps)AttacksPickUps = true;
                }
                else
                {
                    gameSpeed = 1;
                    AttackPlayer = false;
                }
                if (DestroyPickUps)
                {
                    if (AttacksPickUps)
                    {
                        PlayerShip = hit2D.transform.gameObject;
                    }
                    else
                    {
                        PlayerShip = PlayerShipPlaceHolder;
                    } 
                }
            }
            if (PlayerShip == null)
            {
                AttacksPickUps = false;
                AttackPlayer = false;
                TargetLocked = false;
            }
            Warning.color = WarningAlpha;
        }

    }

    private Vector3 RandomPosition()
    {
        resetPosition = true;
        intervals = 7;
        Destroy(Fire.childOf);
        lastPosition =  new Vector3(Random.Range(-10.0f , 10.0f), intervals, 0);
        return lastPosition;
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
        StopCoroutine(StartFireing);
    }

    private void UnSubscribe()
    {
        PlayerMoves.playerState -= PlayerDied;
    }

    public void EnemyHit(GameObject KilledBy)
    {
        Lives--;
        if(Lives == 1)
        {
            EShield.SetActive(false);
        }
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
        float _time = Time.time + Random.Range(2.5f, 5.5f);
        bool _git = true;
        // random shoot 1 or 2 bolts
        if (enemyTag == EnemyTag.Basic)
        {
            while (true)
            {
                NumOfBolts = Random.Range(1, 3);
                if (!AttackPlayer)
                {
                    while(_git)
                    {
                        if (Time.time > _time || AttackPlayer)
                        {
                            _time = Time.time + (Random.Range(2.5f, 5.5f));
                            _git = false;
                        }
                        else yield return null;
                    }
                    _git = true;
                }
                else yield return null;
                // lerp speed random .rand in seconds
                //lastPosition = transform.position;
                lerpSpeed = Random.Range(2f, 5f) * gameSpeed;
                // lerp move twords player
                RandomEnemyMovement();
                while (NumOfBolts > 0 && StopShootingPlayer() && !_enemyDead && (!AttackPlayer || AttacksPickUps))
                {
                    NumOfBolts--;
                    _audioSource.clip = _Shoot;
                    _audioSource.Play();
                    Fire.Fire();
                    yield return new WaitForSecondsRealtime(.25f);
                }
            } 
        }
        if(enemyTag == EnemyTag.Caustic) 
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(Random.Range(3.5f, 5.5f));
                lerpSpeed = Random.Range(2f, 5f) * gameSpeed;
                RandomEnemyMovement();
                GetComponentInChildren<BoldsFire>().enemyTag = enemyTag;
                Fire.Fire();
                _audioSource.clip = _Shoot;
                _audioSource.Play();
                // shoot and move around towrds the player. till laser stops shooting. 
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
            if(t > 1 || resetPosition)
            {
                AttackPlayer = false;
                resetPosition = false;
                t = 0.0f;
                shipLerps = false;
                lastPosition = transform.position;
            }
        }
        if(HardTracking)
        {
            t2 += (lerpSpeed * Time.deltaTime) / (Mathf.Abs(transform.position.x) + Mathf.Abs(lastPlayerShip.x));
            if (PlayerShip != null) lastPlayerShip = PlayerShip.transform.position;
            trackInt = (lerpSpeed * Time.deltaTime * (gameSpeed / 4));
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
            if (t2 > 3 || resetPosition)
            {
                t2 = 0;
                AttackPlayer = false;
                resetPosition = false;
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
        if (AttackPlayer) RandomMove = 10;
        if(RandomMove >= 30)  // 7 out of 10 will not hardTrack 
        {
            t2 = 0;
            t = 0;
            lastPosition = transform.position;
            shipLerps = true;
            HardTracking = false;
            if (PlayerShip != null) lastPlayerShip = PlayerShip.transform.position;
        }
        else 
        {
            t2 = 0;
            t = 0;
            lastPosition = transform.position;
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
            if (PlayerShip.transform.position.y < transform.position.y)
            {
                fireAtPlayer = true;
                if (Fire.transform.rotation.z != 180) Fire.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
            else if (PlayerShip.transform.position.y > transform.position.y)
            {
                if(!ShootsReverse)fireAtPlayer = false;
                if (Fire.transform.rotation.z != 0 && ShootsReverse) Fire.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            }
        }
        else
        {
            fireAtPlayer = false;
        }
        return fireAtPlayer;
    }

    private void DestroyShip()
    {
        StopCoroutine(StartFireing);
        Destroy(Fire.childOf);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        EnemyAnim.SetBool("Destroyed", true);
        AnimationClip[] currentClip = EnemyAnim.runtimeAnimatorController.animationClips;
        UnSubscribe();
        _audioSource.clip = _Explode;
        _audioSource.Play();
        _enemyDead = true;
        Destroy(gameObject, currentClip[0].length);
    }

    private void ExtraLife()
    {
        int _XTraLife = Random.Range(0, 100);
        if(_XTraLife < 30)
        {
            Lives = 2;
            EShield.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        PlayerMoves.playerState -= PlayerDied;
    }
}

public enum EnemyTag
{
    player,
    Basic,
    Caustic

}
