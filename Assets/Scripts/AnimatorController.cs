using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{

    Animator animator;
    bool jumpUp;
    bool jumpFall;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        jumpUp = false;
        jumpFall = false;
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

    private void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.y > 0 && !jumpUp)
        {
            OnJump();
            jumpUp = true;
        }


        if (GetComponent<Rigidbody2D>().velocity.y < 0 && jumpUp && !jumpFall)
        {
            jumpFall = true;
            jumpUp = false;
        }


        if (GetComponent<Rigidbody2D>().velocity.y == 0 && jumpFall && !jumpUp)
        {
            jumpFall = false;
            jumpUp = false;
            OnLand();
        }

    }
}
