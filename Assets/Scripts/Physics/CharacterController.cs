using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public bool jumping = false;
    public bool falling = false;
    public bool onRope = false;
    public bool offRope = false;
    public float gravityValue = 9.81f;
    public float jumpSpeed = 10f;
    public float moveSpeed = 5f;
    public float terminalVelocity = 10f;
    public float checkArea = 1.5f;

    public Vector2 velocity;

    public GameObject rock;
    private RootController rc;
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

    // this function is called by the input component on the player object
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

    // this function is called by the input component on the player object
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

    private void OnBreakRock()
    {
        if (rock != null)
        {
            Destroy(rock);
            rock = null;
        }
    }
    private void RopeMove(Vector2 direction)
    {
        rc.MoveOnRope(gameObject, direction);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForRock();
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
        if (onRope)
        {
            jumping = false;
            return;
        }
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
        // i don't think we need to update anything for the character physics while they're on the rope
        // but hey! maybe we will later. idk
    }

    private void UpdateVelocity()
    {
        if (falling)
        {
            float fallSpeed = velocity.y;
            fallSpeed = Mathf.Clamp(fallSpeed - gravityValue * Time.deltaTime, -terminalVelocity, Mathf.Infinity);
            velocity.y = fallSpeed;
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
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        bool hit = false;
        Transform floor = null;
        for (int i = 0; i < hits.Length && !hit; i++)
        {
            if (hits[i].CompareTag("Floor"))
            {
                floor = hits[i].transform;
                hit = true;
            }
        }

        falling = !hit;
        /*
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        bool hit = false;
        Transform floor = null;
        for (int i = 0; i < hits.Length && !hit; i++)
        {
            if (hits[i].CompareTag("Floor"))
            {
                floor = hits[i].transform;
                hit = true;
            }
        }

        if (floor != null)
        {
            CheckCollision(floor);
            falling = false;
        }
        else
        {
            falling = true;
        }
        //*/
    }

    private void CheckCollision(Transform floor)
    {
        
        /*
        // don't worry about this.

        Vector2 xAxis = (floor.rotation * (floor.localScale / 2)).normalized;
        Vector2 yAxis = floor.localScale / 2;
        yAxis.x *= -1;
        yAxis = (floor.rotation * yAxis).normalized;
        Vector2 floorToPlayer = transform.position - floor.position;

        float referenceAngle = Vector2.Angle(Vector2.right, xAxis);
        Debug.Log("ref: " + referenceAngle);
        Vector2 playerCheck = floorToPlayer;
        float xVal = playerCheck.x;
        float yVal = playerCheck.y;
        playerCheck.x = Mathf.Cos(referenceAngle) * xVal - Mathf.Cos(referenceAngle) * yVal;
        playerCheck.y = Mathf.Sin(referenceAngle) * xVal + Mathf.Sin(referenceAngle) * yVal;
        playerCheck.Normalize();
        float angle = Vector2.Angle(xAxis, playerCheck);
        Vector3 cross = Vector3.Cross(xAxis, playerCheck);
        if (cross.z <= 0)
        {
            angle = 360 - angle;
        }
        Debug.Log("angle: " + angle);

        if (Application.isEditor)
        {
            Debug.DrawRay(floor.position, floorToPlayer, Color.black);
            Debug.DrawRay(floor.position, xAxis, Color.black);
            Debug.DrawRay(Vector2.zero, xAxis, Color.blue);
            Debug.DrawRay(Vector2.zero, yAxis, Color.blue);
            Debug.DrawRay(Vector2.zero, playerCheck, Color.green);
            Vector2 bottomLeftCorner = floor.position - floor.rotation * floor.localScale / 2;
            Vector2 bottomRightCorner = bottomLeftCorner;
            bottomRightCorner.x += (floor.rotation * (floor.localScale)).x;
            Vector2 topRightCorner = floor.position + floor.rotation * floor.localScale / 2;
            Vector2 topLeftCorner = topRightCorner;
            topLeftCorner.x -= (floor.rotation * (floor.localScale)).x;
            Debug.DrawRay(bottomLeftCorner, bottomRightCorner - bottomLeftCorner, Color.red);
            Debug.DrawRay(bottomRightCorner, topRightCorner - bottomRightCorner, Color.red);
            Debug.DrawRay(topRightCorner, topLeftCorner - topRightCorner, Color.red);
            Debug.DrawRay(topLeftCorner, bottomLeftCorner - topLeftCorner, Color.red);
        }
        //*/
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
            onRope = true;
            AttachToRope(rope);
        }
    }

    private void AttachToRope(Transform rope)
    {
        transform.parent = rope;
        velocity = Vector2.zero;
        rc = rope.GetComponent<RootController>();
        transform.localPosition = rc.GetEndPoint(gameObject);
    }

    private void CheckForRock()
    {
        Vector3 check = transform.localScale;
        if (transform.parent != null)
        {
            check.x *= transform.parent.localScale.x;
            check.y *= transform.parent.localScale.y;
        }
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, checkArea * check, 0);
        bool hit = false;
        Collider2D rockCollider = null;
        for (int i = 0; i < hits.Length && !hit; i++)
        {
            if (hits[i].CompareTag("Rock"))
            {
                hit = true;
                rockCollider = hits[i];
            }
        }

        if (hit)
        {
            if (rock != null)
                rock.GetComponent<RockController>().playerNearby = false;

            rock = rockCollider.gameObject;
            rock.GetComponent<RockController>().playerNearby = true;
        }
        else if (rock != null)
        {
            rock.GetComponent<RockController>().playerNearby = false;
        }
    }
}
