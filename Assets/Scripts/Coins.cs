using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public Vector3 offSet = Vector3.zero;
    public Quaternion ROffSet;
    // Start is called before the first frame update
    void Awake()
    {
        ROffSet = transform.localRotation;
        offSet = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // pop the coin with particles
        // make coin collection sound
        // trigger listener coin collected.
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        transform.localRotation *= ROffSet;
        //transform.localPosition = offSet;
    }
}
