using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgressive : MonoBehaviour
{
    private bool aggressive= false;
    private bool shootsReverse = false;
    private bool DestroyPickups = false;
    private bool dodgeBolts = false;
    private EnemyMoves enemyMoves;
    private DodgeBolts _DodgeBolts;
    private void Awake()
    {
        float _Aggressive = Random.Range(0f,1f);
        if (_Aggressive < 0.45f) aggressive = true;
        int _ShootsReverse = Random.Range(0, 100);
        if (_ShootsReverse < 40) shootsReverse = true;
        int _DestroyPickups = Random.Range(0, 101);
        if (_DestroyPickups < 35) DestroyPickups = true;
        int _dodgeBolts = Random.Range(0, 101);
        if(_dodgeBolts < 45)
        {
            dodgeBolts = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyMoves = GetComponent<EnemyMoves>();
        _DodgeBolts = GetComponent<DodgeBolts>();
        if (enemyMoves != null)
        {
            enemyMoves.aggressive = aggressive;
            enemyMoves.ShootsReverse = shootsReverse;
            enemyMoves.DestroyPickUps = DestroyPickups;
            _DodgeBolts._DodgeBolts = dodgeBolts;
        }
    }
}
