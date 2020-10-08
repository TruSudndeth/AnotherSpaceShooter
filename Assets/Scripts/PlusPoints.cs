using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlusPoints : MonoBehaviour
{
    private int points = 000;
    public int Points { get { return points; } set { points = value; } }
    private Vector3 Offset;

    private void Start()
    {
        Offset = new Vector3(0, transform.position.y, 0);
        GetComponentInChildren<TextMeshProUGUI>().text = "+ " + points;
        Debug.Log("points" + points);
        Debug.Break();
        Destroy(gameObject, 1);
    }

    private void LateUpdate()
    {
        transform.position += Offset;
    }
}
