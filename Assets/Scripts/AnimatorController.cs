using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    bool jumping;
    void Start()
    {
        animator = GetComponent<Animator>();
        jumping = false;
    }

    public void OnJump()
    {
        //animator.Play("jump");
        //jumpUp = true;
        jumping = true;
        animator.SetBool("PlayerJump", true);

    }

    public void OnRopeJump()
    {

        jumping = true;
        animator.SetBool("PlayerJump", true);
        //animator.Play("Standing");
    }

    public void DestroyRock()
    {
        animator.Play("FlowerPick");
    }

    public void OnRope()
    {
        //jumpFall = false;
        jumping = false;
        animator.SetBool("PlayerJump", false);
        animator.Play("Standing");
    }

    public void OnLand()
    {
        jumping = false;
        animator.SetBool("PlayerJump", false);
        animator.Play("landjump");

    }

    private void Update()
    {
        //update
        if (transform.parent.gameObject.GetComponent<CharacterController>().velocity.y == 0 && jumping)
        {
            OnLand();
        }
    }
}