using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour
{
    //ToDo's
    // fix Ondestroy
    private Transform EnemyShip = null;
    [SerializeField]
    private GameObject Explosion;
    [SerializeField]
    private float Speed;
    private bool TargetLocked = false;
    void Update()
    {
        if (!TargetLocked)
        {
            RaycastHit2D hit2D;
            hit2D = Physics2D.CircleCast(transform.position, 10f, Vector2.up, 10f, 1 << 8);

            if (hit2D && hit2D.transform.tag == "Enemy")
            {
                EnemyShip = hit2D.transform;
                TargetLocked = true;
            } 
        }
        if(EnemyShip != null)
        {
            Quaternion CurrentRot = transform.rotation;
            if(EnemyShip != null)transform.LookAt(EnemyShip, -Vector3.forward);
            Quaternion LerpRotation = transform.rotation;
            transform.rotation = CurrentRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, LerpRotation, Time.deltaTime * Speed / 2);
        }
        else
        {
            if(TargetLocked)TargetLocked = false;
        }
        transform.position += transform.TransformDirection(Vector3.forward * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            EnemyMoves CollisionGetScript = collision.transform.gameObject.GetComponent<EnemyMoves>();
            if (CollisionGetScript != null)
            {
                CollisionGetScript.EnemyHit(gameObject); 
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // this causes an error with unity play (stopping gameplay)
        Instantiate(Explosion, transform.position, Quaternion.identity);
    }
}
