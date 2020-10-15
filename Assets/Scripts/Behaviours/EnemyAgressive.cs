using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgressive : MonoBehaviour
{
    private bool aggressive= false;
    private void Awake()
    {
        float _Aggressive = Random.Range(0f,1f);
        if (_Aggressive < 0.45f) aggressive = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EnemyMoves>().aggressive = aggressive;
    }
}
