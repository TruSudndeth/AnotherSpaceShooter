using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class PowerLaserAutoTracking : MonoBehaviour
{
    //ToDo's
    // tripple shot + powerLasers must track only one enemy ship. perbolt
    // powerlaser bolt Enemy already tracked, second powerlaser Bolt ignores.
    // each bold has its own particle spark FX (Child of)
    [SerializeField]
    private ParticleSystem LeftConnect;
    private GameObject BoltBehavior;
    private GameObject TargetLocked;
    private bool Seeking = true;
    private float speed = 10f;
    private int angleOffset = 0;
    
    private void Awake()
    {
        if (transform.tag != "PowerLaser") LeftConnect.Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        BoltBehavior = GetComponent<MoveUp>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.tag == "PowerLaser" && Seeking)
        {
            RaycastHit2D HitEnemyL = Physics2D.Raycast(transform.position - (transform.right/2), -transform.right, 100f, 1000);
            RaycastHit2D HitEnemyR = Physics2D.Raycast(transform.position + (transform.right/2), transform.right, 100f, 1000);
            Debug.DrawRay(transform.position - (transform.right/2), -transform.right * 10, Color.red);
            Debug.DrawRay(transform.position - (transform.right / 2), transform.right * 10, Color.red);
            if (HitEnemyL && HitEnemyL.collider.tag == "Enemy")
            {
                TargetLocked = HitEnemyL.transform.gameObject;
                angleOffset = 1;
                Seeking = false;
            }
            if(HitEnemyR && HitEnemyR.collider.tag == "Enemy")
            {
                TargetLocked = HitEnemyR.transform.gameObject;
                angleOffset = -1;
                Seeking = false;
            }
        }
    }

    private void LateUpdate()
    {
            if(!Seeking && transform.tag == "PowerLaser")
            {
            if (TargetLocked != null )
            {
                Vector3 bolt = transform.position;
                Vector3 target = TargetLocked.transform.position;
                Vector3 InjectArcTan = VectorDelta(bolt, target);
                float ArcTan = (90 - Mathf.Atan2(InjectArcTan.y, InjectArcTan.x)) * angleOffset;
                InjectArcTan.z = ArcTan * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(InjectArcTan);
                var main = LeftConnect.main;
                main.startSizeY = new ParticleSystem.MinMaxCurve(InjectArcTan.x, InjectArcTan.x);
            }
            }
    }

    private Vector3 VectorDelta(Vector3 A, Vector3 B)
    {
        Vector3 _Temp = Vector3.zero;
        if (A.x < B.x)
        {
            _Temp = A;
            A = new Vector3(B.x, A.y, A.z);
            B = new Vector3(_Temp.x, B.y, B.z);
        }
        if(A.y < B.y)
        {
            _Temp = A;
            A = new Vector3(A.x, B.y, A.z);
            B = new Vector3(B.x, _Temp.y, B.z);
        }
        if(A.z < B.z)
        {
            _Temp = A;
            A = new Vector3(A.x, A.y, B.z);
            B = new Vector3(B.x, B.y, _Temp.z);
        }

        _Temp = A - B;

        return _Temp;

    }
}
