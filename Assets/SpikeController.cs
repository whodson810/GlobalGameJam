using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SpikeController : MonoBehaviour
{
    public UnityEvent DestroyParent;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if (collision.transform.CompareTag("Thorns"))
        {
            StartCoroutine(DestroyParentCoroutine());
            Destroy(gameObject, 0.5f);
        }
    }

    IEnumerator DestroyParentCoroutine()
    {
        yield return new WaitForSeconds(0.48f);
        DestroyParent?.Invoke();
    }
}
