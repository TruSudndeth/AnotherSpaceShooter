using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpMove2Player : MonoBehaviour
{
    private Transform PlayerTransform;
    private bool IsPickUpMove2Player = false;
    private bool IsMovingDown = true;
    private float gameSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        if (IsPickUpMove2Player)
        {
            if (IsMovingDown)
            {
                IsMovingDown = false;
                GetComponent<PowerUp>().IsMovingDown = false;
                gameSpeed = 2.5f;
            }
            transform.position = Vector2.MoveTowards(transform.position, PlayerTransform.position, Time.deltaTime * gameSpeed);
        }
    }

    public void PlayerPushedC(Transform _Playertransfrom)
    {
        PlayerTransform = _Playertransfrom;
        IsPickUpMove2Player = true;
    }
}
