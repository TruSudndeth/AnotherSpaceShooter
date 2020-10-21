using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TreeEditor;
using UnityEditorInternal;
using UnityEngine;

public class BossMovesInOut : MonoBehaviour
{
    private Vector3 MiddleScreen = new Vector3(0,4,0);
    private Vector3 OutOfScene = new Vector3(0,7.6f,0);
    [SerializeField]
    private GameObject EShield;
    private Vector3 CurrentPosition;
    private float LerpOfT = 0;
    [SerializeField]
    private float speed = 3;
    private bool _MiddleScreen = false;
    private bool _OutOfScreen = false;
    void Start()
    {
        CurrentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EShield.activeSelf && !_MiddleScreen)
        {
            LerpOfT += (Time.deltaTime / (Mathf.Abs(CurrentPosition.y) + Mathf.Abs(MiddleScreen.y))) * speed;
            transform.position = Vector3.Lerp(CurrentPosition, MiddleScreen, LerpOfT);
            if(LerpOfT >= 1)
            {
                LerpOfT = 0;
                CurrentPosition = transform.position;
                 _MiddleScreen = true;
                 _OutOfScreen = false;
            }
        }
        if (EShield.activeSelf && !_OutOfScreen)
        {
            LerpOfT += (Time.deltaTime / (Mathf.Abs(CurrentPosition.y) + Mathf.Abs(OutOfScene.y))) * speed;
            transform.position = Vector3.Lerp(CurrentPosition, OutOfScene, LerpOfT);
            if (LerpOfT >= 1)
            {
                LerpOfT = 0;
                CurrentPosition = transform.position;
                _MiddleScreen = false;
                _OutOfScreen = true; 
            }
        }

    }
}
