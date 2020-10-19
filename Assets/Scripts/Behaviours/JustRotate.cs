using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustRotate : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5;

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0,Time.deltaTime * (Speed / 2),Time.deltaTime * Speed);
    }
}
