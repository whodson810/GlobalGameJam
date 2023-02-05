using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingRootController : MonoBehaviour
{
    public GameObject anchorPoint;
    public UnityEvent Fall;

    // Start is called before the first frame update
    void Start()
    {
        if(anchorPoint == null)
        {
            Debug.LogError("No anchor point detected, self-destruct process initiated");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(anchorPoint != null)
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
}


