using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud_Bar : MonoBehaviour
{
    private float _Boost_Perc;
    public float Boost_Perc { get { return _Boost_Perc; } set { _Boost_Perc = value; } }
    private bool _IsCoolingDown = false;
    public bool IsCoolingDown { get { return _IsCoolingDown; } }

    [SerializeField]
    private Sprite Full;
    [SerializeField]
    private Sprite Mid;
    [SerializeField]
    private Sprite Low;
    [SerializeField]
    private Sprite CoolDown;

    private SpriteRenderer _spriteRenderer;
    private Vector3 Offset;

    void Start()
    {
        transform.localScale = new Vector3(2, 2, 1);
        Offset = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsCoolingDown)
        {
            if (_Boost_Perc > 1.5f)
            {
                _spriteRenderer.sprite = Full;
            }
            if (_Boost_Perc < 1.5f && _Boost_Perc > 0.5f)
            {
                _spriteRenderer.sprite = Mid;
            }
            if (_Boost_Perc < 0.5f && _Boost_Perc > 0f)
            {
                _spriteRenderer.sprite = Low;
            }
            if (_Boost_Perc <= 0)
            {
                //Dissable all color above and fill cool down
                Boost_Perc = 0f;
                _spriteRenderer.sprite = CoolDown;
                _IsCoolingDown = true;
            } 
        }
        if(_Boost_Perc >= 2f)
        {
            Boost_Perc = 2f;
            _IsCoolingDown = false;
        }
        transform.localScale = new Vector3(Boost_Perc, Offset.y, Offset.z);
    }
}
