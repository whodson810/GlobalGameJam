using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnDeath : MonoBehaviour
{
    //doesnt actually have health lolzies its just spikes


    public void InvokeDeath()
    {
        //DEATH
        StartCoroutine(Death());
        
    }

    IEnumerator Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        return null;
    }
}
