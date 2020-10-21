using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPartBehaviour : MonoBehaviour
{
    public delegate void PartDestroyed();
    public static event PartDestroyed _PartDestroyed;
    [SerializeField]
    private int Life = 10;
    private int _EShield = 10;
    private SpriteRenderer Parts;
    [SerializeField]
    private Color DamagedColor;
    [SerializeField]
    private GameObject EShield;
    private SpriteRenderer EShieldDamage;
    private float ColorShiftSpeed = 3;
    private bool DamagedIO = false;
    void Start()
    {
        EShieldDamage = EShield.GetComponent<SpriteRenderer>();
        Parts = transform.gameObject.GetComponent<SpriteRenderer>();
        DamagedColor = Parts.color;
        EShield.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBolts")
        {
            DamagedIO = true;
            if (!EShield.activeSelf)
            {
                Life--;
            }
            if(EShield.activeSelf && _EShield >= 0)
            {
                _EShield--;
                if (_EShield <= 0)
                {
                    DamagedColor = Color.white;
                    EShield.SetActive(false);
                }
            }
        }
    }

    private void Update()
    {
        if(DamagedIO)
        {
            DamagedColor.g -= Time.deltaTime * ColorShiftSpeed;
            DamagedColor.b -= Time.deltaTime * ColorShiftSpeed;

            if (DamagedColor.g <= 0 || DamagedColor.b <= 0)
            {
                DamagedColor = Color.white;
                DamagedIO = false;
            }

            if (EShield.activeSelf) EShieldDamage.color = DamagedColor;
            else Parts.color = DamagedColor;
        }
        if(Life <= 0)
        {
            _PartDestroyed?.Invoke();
            EShield.SetActive(true);
            Destroy(gameObject);
        }
    }
}
