using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Boost : MonoBehaviour
{
    private bool _IsBoosting = false;
    public bool IsBoosting { get { return _IsBoosting; } set { _IsBoosting = value; } }
    private bool _isCoolingDown = false;
    public bool IsCoolingDown { get { return _isCoolingDown; } }
    [SerializeField]
    private Hud_Bar _Hud_Bar;
    [SerializeField]
    private float BoostPercent;
    private Vector3 Offset;
    [SerializeField]
    private float CoolDownTime = 5;
    private float Inc_CoolDownTime;
    [SerializeField]
    private float BoostTime = 10;
    private float Inc_BoostTime;

    // Start is called before the first frame update
    void Start()
    {
        Offset = _Hud_Bar.transform.localScale;
        Inc_BoostTime = Offset.x / BoostTime;
        Inc_CoolDownTime = Offset.x / CoolDownTime;
        BoostPercent = Offset.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsBoosting && BoostPercent < 2)
        {
            BoostPercent += Inc_BoostTime * Time.deltaTime;
        }
        if(_Hud_Bar.IsCoolingDown)
        {
            BoostPercent += Inc_CoolDownTime * Time.deltaTime;
        }
        if(IsBoosting)
        {
            BoostPercent -= Inc_BoostTime * Time.deltaTime;
        }
        BoostPercent = Mathf.Clamp(BoostPercent, 0f, 2f);
        _Hud_Bar.Boost_Perc = BoostPercent;

        _isCoolingDown = _Hud_Bar.IsCoolingDown;

    }
}
