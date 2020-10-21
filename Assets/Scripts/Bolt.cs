using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    //ToDo's
    // Play Bolt sounds here not player and enemy
    //? create an event playerBolt destroy Enemy
    //? make gamecontroller listen to that.
    [SerializeField]
    private Sprite PlayerBolt;
    [SerializeField]
    private Sprite EnemyBolt;
    [SerializeField]
    private Sprite PowerLaserBolt;
    private EnemyTag _enemyTag;
    public EnemyTag enemyTag { get { return _enemyTag; } set { _enemyTag = value; } }
    private IEnumerator CausticBeam;
    private void Awake()
    {
        transform.tag = transform.parent.tag;
    }


    private void Start()
    {
        if(transform.tag == "PlayerBolts")
        {
            GetComponentInChildren<SpriteRenderer>().sprite = PlayerBolt;
        }
        if (transform.tag == "EnemyBolts")
        {
            GetComponentInChildren<SpriteRenderer>().sprite = EnemyBolt;
        }
        if(transform.tag == "PowerLaser")
        {
            GetComponent<SpriteRenderer>().sprite = PowerLaserBolt;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Bounds")
        {
            // check parents children count if zero destroy it else destroy this
            DestroyBoltsParent();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if((transform.tag == "PlayerBolts" || transform.tag == "PowerLaser") && other.tag == "Enemy")
        {
            if(other.GetComponent<EnemyMoves>() != null)other.GetComponentInParent<EnemyMoves>().EnemyHit(gameObject);
            DestroyBoltsParent();
        }
        if(transform.tag == "EnemyBolts" && other.tag == "Player")
        {
            other.GetComponentInParent<PlayerMoves>().PlayerHit();
            if (enemyTag != EnemyTag.Caustic) DestroyBoltsParent(); 
        }
        if (transform.tag == "Obstacle")
        {
            if(enemyTag != EnemyTag.Caustic) DestroyBoltsParent();
        }
        if(other.tag == "PickUps" && transform.tag == "EnemyBolts")
        {
            Destroy(other.gameObject);
            DestroyBoltsParent();
        }
    }

    private void DestroyBoltsParent()
    {
        if (transform.parent.childCount <= 1) Destroy(transform.parent.gameObject);
        else Destroy(gameObject);
    }
}
