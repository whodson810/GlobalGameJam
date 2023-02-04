using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPhysics : MonoBehaviour
{
    public bool jumping = false;
    public bool falling = false;
    public bool onRope = false;
    public bool offRope = false;
    public float gravityValue = 9.81f;
    public float jumpSpeed = 10f;
    public float moveSpeed = 5f;

    public Vector2 velocity;
    private RopeController rc;
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
        if (onRope)
        {
            RopeJump();
            return;
        }
        jumping = v.isPressed;
        if (velocity.y != 0)
            jumping = false;
        if (!jumping)
            return;

        velocity.y = jumpSpeed;
    }

    private void RopeJump()
    {
        onRope = false;
        offRope = true;
        transform.parent = null;
        rc = null;
        velocity.y = jumpSpeed;
        transform.rotation = Quaternion.identity;
    }

    private void OnMove(InputValue v)
    {
        if (onRope)
        {
            RopeMove(v.Get<Vector2>());
            return;
        }
        float x = v.Get<Vector2>().x;
        velocity.x = x * moveSpeed;
    }

    private void RopeMove(Vector2 direction)
    {
        rc.MoveOnRope(gameObject, direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (onRope)
        {
            OnRopeUpdate();
        }
        else
        {
            OffRopeUpdate();
        }
    }

    private void OffRopeUpdate()
    {
        CheckForRope();
        CheckForFloor();
        UpdateVelocity();
        UpdatePosition();
        if (jumping)
        {
            jumping = false;
        }
    }

    private void OnRopeUpdate()
    {

    }

    private void UpdateVelocity()
    {
        if (falling)
        {
            velocity += gravityValue * Time.deltaTime * Vector2.down;
        }
        else
        {
            if (!jumping)
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

    private void CheckForRope()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        bool hit = false;
        Transform rope = null;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Rope"))
            {
                rope = hits[i].transform;
                hit = true;
            }
        }

        if (offRope && !hit)
        {
            offRope = false;
        }
        else if (!offRope && hit)
        {
            Debug.Log(rope);
            onRope = true;
            AttachToRope(rope);
        }
    }

    private void AttachToRope(Transform rope)
    {
        transform.parent = rope;
        velocity = Vector2.zero;
        rc = rope.GetComponent<RopeController>();
        transform.localPosition = rc.GetEndPoint();
    }
}
