using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    bool jumpUp;
    bool jumpFall;
    void Start()
    {
        animator = GetComponent<Animator>();
        jumpUp = false;
        jumpFall = false;
    }

    public void OnJump()
    {
        //animator.Play("jump");
        animator.SetBool("PlayerJump", true);
        //animator.SetBool("PlayerJump", true);
    }

    public void OnRopeJump()
    {
        animator.Play("Standing");
        //animator.Play("Standing");
    }

    /*
     *     public void OnMove(float horizontal)
    {
        
    }
     */
    public void DestroyRock()
    {
        animator.Play("FlowerPick");
    }

    public void OnRope()
    {
        animator.Play("Standing");
    }

    public void OnLand()
    {
        animator.Play("landjump");
        // animator.Play("landjump");
        //animator.SetBool("hitGround", true);
    }

    private void Update()
    {
        if (transform.parent.gameObject.GetComponent<CharacterController>().velocity.y > 0 && !jumpUp)
        {
            OnJump();
            jumpUp = true;
            animator.SetBool("hitGround", false);
        }
        if (transform.parent.gameObject.GetComponent<CharacterController>().velocity.y < 0 && jumpUp && !jumpFall)
        {
            animator.SetBool("PlayerJump", false);
            jumpFall = true;
            jumpUp = false;
        }
        if (transform.parent.gameObject.GetComponent<CharacterController>().velocity.y == 0 && jumpFall && !jumpUp)
        {
            jumpFall = false;
            jumpUp = false;
            OnLand();
        }
    }
}