using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //ToDo's
    public delegate void PowerUps(GameObject HitBy, int PowerUpType);
    public static event PowerUps Collected;
    [SerializeField]
    private int PowerUpID; // 1 = tripple shot, 2 = shilds, 3 = speed, 4 = coins, 5 = Ammo, 6 = FirstAid, 7 = OP
    private float speed = 1;
    private float gameSpeed = 1;
    [HideInInspector]
    public bool IsMovingDown = true;
    void Update()
    {
        if (IsMovingDown)
        {
            transform.position += Vector3.down * Time.deltaTime * speed * gameSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collected?.Invoke(collision.gameObject, PowerUpID);
            Destroy(gameObject);
        }
    }

    public void GameControllerSpeed(float _speed)
    {
        gameSpeed = _speed;
    }
}
