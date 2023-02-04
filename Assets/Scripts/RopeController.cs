using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public List<Vector2> points = new();
    public float speed = 5.0f;
    public bool swinging = false;
    public float timer = 0;
    public float angle = 70;
    public int pointIndex = 0;
    public int swingDirection = 0; // -1 = counterclockwise, 1 = clockwise
    public bool point1Locked = false; // points[0]
    public bool point2Locked = false; // points[points.Count - 1]

    private Vector2 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        MakePoints();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        UpdateSwing();
    }

    private void UpdateSwing()
    {
        if (transform.childCount == 0)
        {
            transform.rotation = initialRotation;
            transform.position = initialPosition;
        }
        if (point1Locked && point2Locked)
        {
            return;
        }

        if (swinging)
        {
            timer += Time.deltaTime;
            Vector2 rotationPoint = transform.position;
            if (point1Locked)
                rotationPoint -= (Vector2)(transform.rotation * points[points.Count - 1]);
            if (point2Locked)
                rotationPoint -= (Vector2)(transform.rotation * points[0]);
            transform.RotateAround(rotationPoint, Vector3.forward, swingDirection * angle * Mathf.PI / 360 * Mathf.Cos(timer));
        }
        else
        {
            timer = 0;
        }
    }

    private void MakePoints()
    {
        int numPoints = (int)transform.localScale.y + 1;
        Vector2 firstPoint = transform.localScale.y / 2 * Vector2.up;

        for (int i = 0; i < numPoints; i++)
        {
            points.Add(firstPoint + i * Vector2.down);
        }

        pointIndex = points.Count - 1;
    }

    // Only use this when getting a point to set the player's position
    public Vector2 GetEndPoint()
    {
        return GetPoint(points.Count - 1);
    }

    // Only use this when getting a point to set the player's position
    private Vector2 GetPoint(int index)
    {
        if (index < 0)
            index = 0;
        if (index > points.Count - 1)
            index = points.Count - 1;
        return points[index] / transform.localScale.y;
    }

    // Use this for any other point getting
    private Vector2 GetUnscaledPoint(int index)
    {
        if (index < 0)
            index = 0;
        if (index > points.Count - 1)
            index = points.Count - 1;
        return points[index];
    }

    public void MoveOnRope(GameObject player, Vector2 direction)
    {
        if (direction.x != 0)
        {
            SwingOnRope(player, direction);
        }
        if (direction.y != 0)
        {
            ClimbOnRope(player, direction);
        }
    }

    private void ClimbOnRope(GameObject player, Vector2 direction)
    {
        int dir = 0;
        if (direction.y > 0)
            dir = -1;
        if (direction.y < 0)
            dir = 1;
        pointIndex += dir;
        if (pointIndex == points.Count)
            pointIndex -= 1;
        if (pointIndex == -1)
            pointIndex += 1;
        player.transform.localPosition = GetPoint(pointIndex);
        Vector2 compare = transform.rotation * (GetUnscaledPoint(points.Count - 2) - GetUnscaledPoint(points.Count - 1));
        Vector2 playerComp = (transform.position + transform.rotation * GetUnscaledPoint(points.Count - 2)) - player.transform.position;
        float dotValue = Vector2.Dot(compare, playerComp);

        // i realized, only now after debugging all of this vector math, that there was an easier way to do this
        // just check the fuckin pointIndex.
        // whatever it stays now b/c i said so
        if (dotValue <= 0)
        {
            swinging = false;
            timer = 0;
            transform.rotation = initialRotation;
            transform.position = initialPosition;
        }
    }

    private void SwingOnRope(GameObject player, Vector2 direction)
    {
        // i don't think changing direction mid-swing is a good idea.
        if (swinging)
            return;
        Vector2 compare = GetPoint(points.Count - 2);
        if (player.transform.localPosition.y < compare.y)
        {
            swinging = true;
        }

        // if we have swinging roots that don't start from a downward position this'll feel weird as hell lol
        if (direction.x < 0)
            swingDirection = -1;
        if (direction.x > 0)
            swingDirection = 1;
    }

    public void OnJumpOffRope()
    {
        pointIndex = points.Count - 1;
    }
}

