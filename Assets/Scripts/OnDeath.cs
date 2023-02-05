using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnDeath : MonoBehaviour
{
    //doesnt actually have health lolzies its just spikes
    [SerializeField] float fallDamageHeight;

    bool falling;
    float startHeight;

    private void Start()
    {
        falling = false;
    }
    public void InvokeDeath()
    {
        //DEATH
        GetComponentInChildren<Animator>().Play("DEATH");
        StartCoroutine(Death());
        
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }

    private void Update()
    {
        if (GetComponent<CharacterController>().velocity.y < 0 && falling == false)
        {
            falling = true;
            startHeight = transform.position.y;
        }   
        else if (GetComponent<CharacterController>().velocity.y == 0 && falling)
        {
            falling = false;
            if ((startHeight - transform.position.y >= fallDamageHeight))
            {
                InvokeDeath();
            }
        }

    }


}
