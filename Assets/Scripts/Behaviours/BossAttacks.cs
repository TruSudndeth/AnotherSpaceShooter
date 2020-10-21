using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    [SerializeField]
    private GameObject Astroids;
    [SerializeField]
    private GameObject TEnemies;
    [SerializeField]
    private GameObject EShield;
    private int AttackType = 0;
    private IEnumerator _AstroidAttack;
    private IEnumerator _TinyEnemies;
    private bool IsAttacking = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EShield.activeSelf && !IsAttacking)
        {
            Debug.Log("EShield.ActiveSelf is ture;");
            AttackType = Random.Range(0, 100);
            if (AttackType < 60 && AttackType > 40)
            {
                _TinyEnemies = TinyEnemies(1);
                StartCoroutine(_TinyEnemies);
                IsAttacking = true;
            }
            else if (AttackType < 40)
            {
                Debug.Log("Step 2");
                _AstroidAttack = AstroidAttack(0.3f);
                StartCoroutine(_AstroidAttack);
                IsAttacking = true;
            }
            else
            {
                Debug.Log("Step Default");
                _AstroidAttack = AstroidAttack(.8f);
                _TinyEnemies = TinyEnemies(3f);
                StartCoroutine(_AstroidAttack);
                StartCoroutine(_TinyEnemies);
                IsAttacking = true;
            }
        }
    }

    private IEnumerator AstroidAttack(float _AttackSpeed)
    {
        while (EShield.activeSelf)
        {
            yield return new WaitForSeconds(_AttackSpeed);
            Instantiate(Astroids, RandomLocation(), Quaternion.identity);
        }
        Debug.Log("Astroid Attack this Ran for some reason");
        IsAttacking = false;
        StopCoroutine(_AstroidAttack);
    }
    private IEnumerator TinyEnemies(float _AttackSpeed)
    {
        GameObject _TEnemies = null;
        while(EShield.activeSelf)
        {
            yield return new WaitForSeconds(_AttackSpeed);
            _TEnemies = Instantiate(TEnemies, RandomLocation(), Quaternion.identity);
            _TEnemies.transform.localScale /= 2;
        }
        Debug.Log("TinyEnemies Attack this Ran for some reason");
        IsAttacking = false;
        StopCoroutine(_TinyEnemies);
    }

    private Vector3 RandomLocation()
    {
        Vector3 _RandomRand = new Vector3(Random.Range(-10, 10), 7, 0);
        return _RandomRand;
    }

}
