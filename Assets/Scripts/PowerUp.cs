using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //ToDo's
    //
    public delegate void PowerUps(GameObject HitBy, int PowerUpType);
    public static event PowerUps Collected;
    [SerializeField]
    private int PowerUpType = 1;
    [SerializeField]
    private int PowerUpID; // 1 = tripple shot, 2 = shilds, 3 = speed, 4 = coins
    private float speed = 1;
    private float gameSpeed = 1;
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * speed * gameSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Player")
        {
            Collected(collision.gameObject, PowerUpID);
            Destroy(gameObject); 
        }
    }

    private void GameControllerSpeed(float _speed)
    {
        gameSpeed = _speed / 2;
    }
}
