using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBolts : MonoBehaviour
{
    [SerializeField]
    private ApplyEnemyMoves _ApplyEnemyMoves;
    [SerializeField]
    private EnemyMoves _EnemyMoves;
    private float gameSpeed = 3;
    [HideInInspector]
    public bool _DodgeBolts = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_DodgeBolts)
        {
            RaycastHit2D hit2D;
            hit2D = Physics2D.CircleCast(transform.position, 5f, Vector3.up, 1f, 1 << 11);
            if (hit2D && hit2D.transform.tag == "PlayerBolts")
            {
                    _ApplyEnemyMoves.MovePosition(Vector3.MoveTowards(transform.position, new Vector3(hit2D.transform.position.x, transform.position.y, 0), -Time.deltaTime * gameSpeed));
                    _EnemyMoves._EnemyMoves = false; 
            } 
            else
            {
                if (!_EnemyMoves._EnemyMoves)
                {
                    _EnemyMoves._EnemyMoves = true;
                }
            }
        }
    }
}
