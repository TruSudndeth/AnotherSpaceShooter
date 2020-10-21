using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySworm : MonoBehaviour
{
    [SerializeField]
    private GameObject SmallEnemyShip;
    private GameObject[] EShips;
    [SerializeField]
    private int EnemyCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        EShips = new GameObject[EnemyCount];
        SpawnSmallShips();
    }

    // Update is called once per frame
    void Update()
    {
        MoveShips();
    }

    private void SpawnSmallShips()
    {
        Vector3 TempPosition;
        for(int i = 0; i < EnemyCount; i++)
        {
            TempPosition = new Vector3((-EnemyCount/2) + i, transform.position.y, transform.position.z);
            EShips[i] = Instantiate(SmallEnemyShip, TempPosition, transform.rotation);
            EShips[i].transform.localScale = Vector3.one / 2;
        }
    }

    private void MoveShips()
    {
        foreach(GameObject Ship in EShips)
        {
            if (Ship.transform.gameObject != null)
            {
                Ship.transform.position += new Vector3(0, Time.deltaTime, 0); 
            }
        }
    }
}
