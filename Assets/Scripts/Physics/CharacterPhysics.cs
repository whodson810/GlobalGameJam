using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPhysics : MonoBehaviour
{
    public bool jumping = false;
    public bool falling = false;
    public float gravityValue = 9.81f;
    public float jumpSpeed = 10f;
    public float moveSpeed = 5f;

    public Vector2 velocity;
    //private Subscription<JumpEvent> subJump;
   // private Subscription<Vec2InputEvent> subMove;

    private void Start()
    {
        //subJump = EventBus.Subscribe<JumpEvent>(OnJumpEvent);
        //subMove = EventBus.Subscribe<Vec2InputEvent>(OnMoveEvent);
    }

    private void OnDestroy()
    {
        //EventBus.Unsubscribe(subJump);
       // EventBus.Unsubscribe(subMove);
    }

    private void OnJump(InputValue v)
    {
        jumping = v.isPressed;
        if (!jumping)
            return;

        velocity.y = jumpSpeed;
    }

    private void OnMove(InputValue v)
    {
        float x = v.Get<Vector2>().x;
        velocity.x = x * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForFloor();
        UpdateVelocity();
        UpdatePosition();
        if (velocity.y != 0.0f)
        {
            jumping = false;
        }
    }

    private void UpdateVelocity()
    {
        if ((jumping && !falling) || falling)
        {
            velocity += gravityValue * Time.deltaTime * Vector2.down;
        }
        else
        {
            velocity.y = 0;
        }
    }

    private void UpdatePosition()
    {
        Vector2 pos = transform.position;
        pos += Time.deltaTime * velocity;
        transform.position = pos;
    }

    private void CheckForFloor()
    {
        float dist = transform.localScale.y / 2;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, dist);
        Debug.DrawRay(transform.position, dist * Vector2.down, Color.red);
        if (hit.collider != null)
        {
            falling = !hit.collider.CompareTag("Floor");
        }
        else
        {
            falling = true;
        }
    }
}
