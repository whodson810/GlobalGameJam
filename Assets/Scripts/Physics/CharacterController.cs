using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip cutSound;
    public AudioClip climbSound;
    public bool jumping = false;
    public bool falling = false;
    public bool onRope = false;
    public bool offRope = false;
    public float gravityValue = 9.81f;
    public float jumpSpeed = 10f;
    public float moveSpeed = 5f;
    public float terminalVelocity = 10f;
    public float checkArea = 1.5f;
    public int offRootFrames = 0;
    public int frameMax = 4;


    public Vector2 velocity;
    public Vector2 direction;

    public GameObject rock;
    private RootController rc;

    private AnimatorController animator;

    private void Start()
    {
        // 1920 x 1080 60fps in fullscreen
        // yes this is the wrong place to put this no i don't care
        Screen.SetResolution(1920, 1080, true, 60);
        animator = GetComponentInChildren<AnimatorController>();
        direction = Vector2.zero;
    }

    // this function is called by the input component on the player object
    private void OnJump(InputValue v)
    {
        jumping = v.isPressed;
        if (velocity.y != 0)
            jumping = false;
        Camera.main.GetComponent<AudioSource>().PlayOneShot(jumpSound);
        if (onRope)
        {
            RopeJump();
            return;
        }
        if (!jumping)
        {
            return;
        }

        velocity.y = jumpSpeed;

        if (animator)
        {
            animator.OnJump();
        }
    }

    private void RopeJump()
    {
        onRope = false;
        offRope = true;
        transform.parent = null;
        velocity.y = jumpSpeed;
        transform.rotation = Quaternion.identity;


        if (animator)
        {
            animator.OnRopeJump();
        }
    }

    // this function is called by the input component on the player object
    private void OnMove(InputValue v)
    {
        direction = v.Get<Vector2>();
        direction.y = 0;
        if (onRope)
        {
            RopeMove(v.Get<Vector2>());
            return;
        }
        float x = v.Get<Vector2>().x;

        if (animator)
        {
            //animator.OnMove(x);
        }
    }

    private void OnBreakRock()
    {
        CheckForRock();
        if (rock != null)
        {
            if (rock.GetComponent<RockController>().cantBeBroken)
                return;
            Destroy(rock);
            Camera.main.GetComponent<AudioSource>().PlayOneShot(cutSound);
            if (animator)
            {
                animator.DestroyRock();
            }
            rock = null;
        }

    }
    private void RopeMove(Vector2 direction)
    {
        if (animator)
        {
            animator.OnRope();
        }

        Camera.main.GetComponent<AudioSource>().PlayOneShot(climbSound);
        rc.MoveOnRope(gameObject, direction);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForRock();
        if (onRope)
        {
            velocity = Vector2.zero;
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
        UpdateVelocity();
        UpdatePosition();
        CheckForFloor();

        if (jumping)
        {
            jumping = false;
        }
    }

    private void OnRopeUpdate()
    {
        if (rc == null)
        {
            onRope = false;
            offRope = false;
        }
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
            if (velocity.y < 0)
                velocity.y = 0;
        }
        velocity.x = moveSpeed * direction.x;
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
        List<Transform> floors = new();
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Floor"))
            {
                floors.Add(hits[i].transform);
            }
        }

        if (floors.Count != 0)
        {
            foreach(Transform f in floors)
            {
                CheckCollision(f);
            }
        }
        else
        {
            falling = true;
        }
    }

    private void CheckCollision(Transform floor)
    {
        if (offRope)
            return;

        int side = 0; // 0 = right, 1 = top, 2 = left, 3 = bottom
        Vector2 xAxis = (floor.rotation * (floor.localScale / 2)).normalized;
        if (xAxis.y < 0)
            xAxis.y *= -1;
        if (xAxis.x < 0)
            xAxis.x *= -1;
        Vector2 yAxis = xAxis;
        yAxis.y *= -1;
        Vector2 floorToPlayer = transform.position - floor.position;

        float referenceAngle = Vector2.Angle(Vector2.right, xAxis);
        float vertAngle = referenceAngle * 2;
        float horizAngle = 180 - vertAngle;
        float playerAngle = Vector2.Angle(yAxis, floorToPlayer);
        Vector3 cross = Vector3.Cross(yAxis, floorToPlayer);
        if (cross.z <= 0)
        {
            playerAngle = 360 - playerAngle;
        }
        float angle = vertAngle;
        while (playerAngle > angle)
        {
            playerAngle -= angle;
            if (side % 2 == 0)
                angle = horizAngle;
            else
                angle = vertAngle;
            side += 1;
        }

        Vector2 newPos = floor.position;
        switch (side)
        {
            case 0:
                newPos.y = transform.position.y;
                newPos.x += Mathf.Abs((floor.rotation * floor.localScale).x) / 2 + transform.localScale.x / 2;
                falling = true;
                if (velocity.x < 0)
                    velocity.x = 0;
                break;
            case 1:
                newPos.x = transform.position.x;
                newPos.y += Mathf.Abs((floor.rotation * floor.localScale).y) / 2 + transform.localScale.y / 2;
                if (velocity.y < 0)
                {
                    falling = false;
                    velocity.y = 0;
                }
                break;
            case 2:
                newPos.y = transform.position.y;
                newPos.x -= Mathf.Abs((floor.rotation * floor.localScale).x) / 2 + transform.localScale.x / 2;
                falling = true;
                if (velocity.x > 0)
                    velocity.x = 0;
                break;
            case 3:
                newPos.x = transform.position.x;
                newPos.y -= Mathf.Abs((floor.rotation * floor.localScale).y) / 2 + transform.localScale.y / 2;
                falling = true;
                if (velocity.y > 0)
                    velocity.y = 0;
                break;
            default:
                Debug.Log("something broke.");
                return;
        }

        transform.position = newPos;
        if (Application.isEditor)
        {
            Debug.DrawRay(floor.position, floorToPlayer, Color.black);
            Debug.DrawRay(floor.position, xAxis, Color.black);
            Debug.DrawRay(Vector2.zero, xAxis, Color.blue);
            Debug.DrawRay(Vector2.zero, yAxis, Color.red);
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
        bool floor = false;
        Transform rope = null;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Floor"))
                floor = true;
            if (hits[i].CompareTag("Rope"))
            {
                rope = hits[i].transform;
                hit = true;
            }
        }

        if (!hit)
        {
            if (offRope && !floor)
            {
                offRootFrames += 1;
                if (offRootFrames == frameMax)
                    offRope = false;
            }
            else
            {
                offRootFrames = 0;
            }
            onRope = false;
        }
        else if (!offRope && hit)
        {
            onRope = true;
            AttachToRope(rope);
            offRootFrames = 0;
        }
    }

    private void AttachToRope(Transform rope)
    {
        if (animator)
        {
            animator.OnRope();
        }

        transform.parent = rope;
        velocity = Vector2.zero;
        rc = rope.GetComponent<RootController>();
        transform.localPosition = rc.GetEndPoint(gameObject);
    }

    private void CheckForRock()
    {
        if (rock != null)
            rock.GetComponent<RockController>().playerNearby = false;
        rock = null;
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
            rock = rockCollider.gameObject;
            rock.GetComponent<RockController>().playerNearby = true;
        }
    }
}
