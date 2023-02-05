using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingRootController : MonoBehaviour
{
    public GameObject anchorPoint;
    public UnityEvent Fall;

    // Update is called once per frame
    void Update()
    {
        if (anchorPoint != null)
        {
            return;
        }
        else
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY;
            }
            GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY;
            Fall?.Invoke();
        }
    }

    public void SetAnchorPoint(GameObject point)
    {
        anchorPoint = point;
    }
}


