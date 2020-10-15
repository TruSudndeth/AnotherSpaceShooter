using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BoldsFire : MonoBehaviour
{
    //ToDo's
    // make an enumerator for number of shots 1,2,3 shots also missles or plasma bolts
    // Unity Caution missing gameObject reference when destroyed in Bolts.Script
    public EnemyTag enemyTag;
    [SerializeField]
    private GameObject Bolts;
    private float speed = 10;
    [SerializeField]
    private GameObject Bolts_2;
    [SerializeField]
    private GameObject Bolts_3;
    [SerializeField]
    private GameObject Beam;
    public GameObject childOf;
    private GameObject ThisShoots;
    private bool doubleShot = false;
    private void Awake()
    {
        ThisShoots = Bolts;
    }

    public void Fire()
    {
        if(enemyTag != EnemyTag.Caustic)ThisShoots.tag = transform.tag;
        if (enemyTag == EnemyTag.Caustic)
        {
            MultiShot(4); // Beam shot
        }

            childOf = Instantiate(ThisShoots, transform.position, transform.rotation);
    }

    private void Update()
    {
        if (enemyTag == EnemyTag.Caustic)
        {
            if(childOf != null)childOf.transform.position = new Vector3(transform.position.x, 0, 0);
        }
    }


    public void MultiShot(int MultiShot)
    {
        switch(MultiShot)
        {
            case 1:
                ThisShoots = Bolts;
                break;
            case 2:
                ThisShoots = Bolts_2;
                break;
            case 3:
                ThisShoots = Bolts_3;
                break;
            case 4:
                ThisShoots = Beam;
                break;
            default:
                ThisShoots = Bolts;
                break;
        }
    }
}
