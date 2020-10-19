using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator Anim;
    private void Start()
    {
        Anim = GetComponent<Animator>();
        AnimationClip[] CurrentClip = Anim.runtimeAnimatorController.animationClips;
        Destroy(gameObject, CurrentClip[0].length);
    }
}
