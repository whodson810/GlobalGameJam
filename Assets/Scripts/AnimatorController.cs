using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnJump()
    {
        animator.Play("jump");
    }

    public void OnRoapJump()
    {

    }

    public void OnMove(float horizontal)
    {
        
    }

    public void DestroyRock()
    {

    }

    public void OnLand()
    {
        animator.Play("landjump");
    }
}
