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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Bounds")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(transform.tag == "PlayerBolts" && other.tag == "Enemy")
        {
            other.GetComponentInParent<EnemyMoves>().EnemyHit(gameObject);
            Destroy(gameObject);
        }
        if(transform.tag == "EnemyBolts" && other.tag == "Player")
        {
            other.GetComponentInParent<PlayerMoves>().PlayerHit();
            Destroy(gameObject);
        }
    }
}
