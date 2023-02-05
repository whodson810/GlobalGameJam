using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public CharacterController cc;
    public List<Sprite> sprites;
    public float jumpTime = 0.4f;
    public float jumpRopeTime = 0.2f;
    public float landTime = 0.2f;
    public float frameTimer = 0;
    public float maxTime = 0;
    public int currentState = 0; // 0 = on ground/rope, 1 = jumping, 2 = falling, 3 = landing, 4 = flower
    public int lastState = 0;
    public float flowerTime = 0.25f;

    public SpriteRenderer sr;
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
    }

    public void OnRopeJump()
    {
    }


    public void DestroyRock()
    {

    }

    public void OnLand()
    {
    }

    private void Update()
    {
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
                        maxTime = jumpRopeTime;
                        frameTimer = 0;
                    }
                    else
                    {
                        // jumping off the ground
                        maxTime = jumpTime;
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
                    maxTime = landTime;
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
                    frameTimer += Time.deltaTime;
                    if (frameTimer >= maxTime)
                    {
                        currentState = 2;
                    }
                }
                break;
            case 2: // falling
                if (velocity.y == 0 && !onRope)
                {
                    // landing
                    maxTime = landTime;
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
                    maxTime = jumpTime;
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
                    frameTimer += Time.deltaTime;
                    if (frameTimer >= maxTime)
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
