using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene: MonoBehaviour
{
    [SerializeField] string toLoad;
    public void LoadGame()
    {
        SceneManager.LoadScene(toLoad);
    }

    
}
