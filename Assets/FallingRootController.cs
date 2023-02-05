using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingRootController : MonoBehaviour
{
    public AudioClip fallSound;
    public GameObject anchorPoint;
    public UnityEvent Fall;

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
            Destroy(gameObject);
        if (anchorPoint != null)
        {
            return;
        }
        else
        {
            foreach (Transform child in gameObject.transform)
            {
                Rigidbody2D rb = child.gameObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;
            }
            Camera.main.transform.GetChild(0).GetComponent<AudioSource>().PlayOneShot(fallSound);
            GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY;
            Fall?.Invoke();
        }
    }

    public void SetAnchorPoint(GameObject point)
    {
        anchorPoint = point;
    }
}


