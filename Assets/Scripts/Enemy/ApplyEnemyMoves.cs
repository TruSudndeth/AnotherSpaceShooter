using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyEnemyMoves : MonoBehaviour
{
    private Vector3 DefaultMovementa;
    // Start is called before the first frame update
    void Start()
    {
        DefaultMovementa = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = DefaultMovementa;
    }

    public void MovePosition(Vector3 _MoveEnemy) // Default are moves from EnemyMoves Script
    {
        DefaultMovementa = _MoveEnemy;
    }
}
