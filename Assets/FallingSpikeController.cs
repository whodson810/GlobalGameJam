using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingSpikeController : MonoBehaviour
{
    public GameObject anchorPoint;

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
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if (collision.transform.CompareTag("Thorns"))
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
