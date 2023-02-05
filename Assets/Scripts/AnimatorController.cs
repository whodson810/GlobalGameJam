using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public CharacterController cc;
    public List<Sprite> sprites;
    public int jumpFrames = 4;
    public int jumpRopeFrames = 4;
    public int landFrames = 4;
    public int frameTimer = 0;
    public int maxFrames = 0;
    public int currentState = 0; // 0 = on ground/rope, 1 = jumping, 2 = falling, 3 = landing

    public SpriteRenderer sr;
    //Animator animator;
    // Start is called before the first frame update
    Vector2 velocity;
    bool jumping = false;
    bool falling = false;
    bool onRope = false;
    bool offRope = false;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (cc == null)
            cc = GameObject.Find("Player").GetComponent<CharacterController>();
    }

    public void OnJump()
    {
        //animator.Play("jump");
        //animator.SetBool("PlayerJump", true);
    }

    public void OnRopeJump()
    {
        //animator.Play("Standing");
    }

    /*
     *     public void OnMove(float horizontal)
    {
        
    }
     */


    public void DestroyRock()
    {

    }

    public void OnLand()
    {
       // animator.Play("landjump");
        //animator.SetBool("hitGround", true);
    }

    private void Update()
    {

        /*
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
        //*/
        GetPlayerState();
        UpdateSprite();
    }


    private void GetPlayerState()
    {
        velocity = cc.velocity;
        jumping = cc.jumping;
        falling = cc.falling;
        onRope = cc.onRope;
        offRope = cc.offRope;
    }

    private void UpdateSprite()
    {
        switch (currentState)
        {
            case 0: // on ground/rope
                if (jumping)
                {
                    currentState = 1;
                    if (offRope)
                    {
                        // jumping off a rope
                        maxFrames = jumpRopeFrames;
                        frameTimer = 0;
                    }
                    else
                    {
                        // jumping off the ground
                        maxFrames = jumpFrames;
                        frameTimer = 0;
                    }
                }
                else if (velocity.y != 0)
                {
                    // falling off a platform
                    currentState = 2;
                }
                break;
            case 1: // jumping
                if (velocity.y == 0 && !onRope)
                {
                    // landing
                    maxFrames = landFrames;
                    currentState = 3;
                    frameTimer = 0;
                }
                else if (onRope)
                {
                    // attaching to rope
                    currentState = 0;
                    frameTimer = 0;
                }
                else
                {
                    // frame countdown
                    frameTimer += 1;
                    if (frameTimer >= maxFrames)
                    {
                        currentState = 2;
                    }
                }
                break;
            case 2: // falling
                if (velocity.y == 0 && !onRope)
                {
                    // landing
                    maxFrames = landFrames;
                    currentState = 3;
                    frameTimer = 0;
                }
                else if (onRope)
                {
                    // attaching to rope
                    currentState = 0;
                    frameTimer = 0;
                }
                break;
            case 3: // landing
                if (jumping)
                {
                    // jumping off the ground
                    currentState = 1;
                    maxFrames = jumpFrames;
                    frameTimer = 0;
                }
                else if (velocity.y != 0)
                {
                    // falling off a platform
                    currentState = 2;
                }
                else
                {
                    // frame countdown
                    frameTimer += 1;
                    if (frameTimer >= maxFrames)
                    {
                        currentState = 0;
                    }
                }
                break;
            default:
                Debug.Log("animator broke.");
                return;
        }

        // gotta make sure there are 4 sprites in the array lol
        sr.sprite = sprites[currentState];
        sr.flipX = velocity.x < 0;
    }
}
