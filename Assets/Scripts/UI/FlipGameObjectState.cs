using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipGameObjectState : MonoBehaviour
{
    [SerializeField] GameObject[] toFlip;
    
    public void Flip()
    {
        foreach (GameObject flip in toFlip)
        {
            flip.SetActive(!flip.activeSelf);
        }
    }
}
