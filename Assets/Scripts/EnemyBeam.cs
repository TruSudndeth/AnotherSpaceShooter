using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(BeamStreangh());
    }

    private IEnumerator BeamStreangh()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}