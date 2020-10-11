using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustRotateRight : MonoBehaviour
{
    [SerializeField]
    private float RotationMultiplyer = 1;
    private float RotationSpeed;

    private void Start()
    {
        RotationSpeed = Random.Range(1f, 5f) * RotationMultiplyer;
        
    }
    void Update()
    {
        transform.rotation *= Quaternion.Euler(new Vector3(0,0,(RotationSpeed * Time.deltaTime)));
    }
}
