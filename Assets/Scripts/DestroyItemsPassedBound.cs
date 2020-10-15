using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItemsPassedBound : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Bounds")
        {
            Destroy(gameObject);
        }
    }
}
