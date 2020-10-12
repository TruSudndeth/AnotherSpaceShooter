using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShook : MonoBehaviour
{
    private Animator CameraAnim;
    private bool Triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        CameraAnim = GetComponent<Animator>();
        PlayerMoves.PlayerGotHit += PlayerGotHit;
    }

    private void PlayerGotHit()
    {
        int HitAnim = Random.Range(1, 4);
        Triggered = true;
        switch (HitAnim)
        {
            case 1:
                CameraAnim.SetTrigger ("Hit001");
                break;
            case 2:
                CameraAnim.SetTrigger("Hit002");
                break;
            case 3:
                CameraAnim.SetTrigger("Hit003");
                break;
            default:
                Debug.Log("Camera Shake Failed");
                break;
        }
    }

    private void OnDestroy()
    {
        PlayerMoves.PlayerGotHit -= PlayerGotHit;
    }
}
