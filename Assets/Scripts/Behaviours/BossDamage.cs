using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public delegate void GameIsOver();
    public static event GameIsOver _GameIsOver;
    private SpriteRenderer _SpriteRenderer;
    private Color DamageColor;
    private int TotalLife = 25;
    private int TotalOfParts = 5;
    private bool TakeDamage = false;
    [SerializeField]
    private GameObject _Explosion;
    private float ColorShiftSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        DamageColor = _SpriteRenderer.color;
        BossPartBehaviour._PartDestroyed += PartsDestroyed;
    }

    // Update is called once per frame
    void Update()
    {
        if(TotalOfParts <= 0)
        {
            TakeDamage = true;
        }
        if(TotalLife <= 0)
        {
            BossPartBehaviour._PartDestroyed -= PartsDestroyed;
            Destroy(gameObject);
            _GameIsOver?.Invoke();
        }
        if (TakeDamage)
        {
            DamageColor.g -= Time.deltaTime * ColorShiftSpeed;
            DamageColor.b -= Time.deltaTime * ColorShiftSpeed;

            if (DamageColor.g <= 0 || DamageColor.b <= 0)
            {
                DamageColor = Color.white;
                TakeDamage = false;
            }
            _SpriteRenderer.color = DamageColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBolts")
        {
            if (TotalOfParts <= 0)
            {
                TotalLife--;
                TakeDamage = true;
            }
        }
    }
    private void PartsDestroyed()
    {
        TotalOfParts--;
    }

    private void OnDestroy()
    {
        _Explosion  = Instantiate(_Explosion, transform.position, Quaternion.identity);
        _Explosion.transform.localScale *= 5;

    }
}
