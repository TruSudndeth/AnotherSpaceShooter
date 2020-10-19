using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    private float speed = 10;

    private void Update()
    {
        transform.position += transform.TransformDirection(Vector3.forward * speed * Time.deltaTime);
    }
}
