using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchDeath : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<OnDeath>())
        {
            collision.gameObject.GetComponent<OnDeath>().InvokeDeath();
        }
    }
}
