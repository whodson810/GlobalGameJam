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
            transform.RotateAround(rotationPoint, Vector3.forward, angle * Mathf.PI / 360 * Mathf.Cos(timer));
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

        pointIndex = points.Count;
    }

    public Vector2 GetEndPoint()
    {
        return GetPoint(points.Count - 1);
    }

    private Vector2 GetPoint(int index)
    {
        return points[index] / transform.localScale.y;
    }

    public void MoveOnRope(GameObject player, Vector2 direction)
    {
        if (direction.x != 0)
        {
            SwingOnRope(player);
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
        Vector2 compare = transform.rotation * (GetPoint(points.Count - 2) - GetEndPoint());
        Vector2 playerComp = GetPoint(points.Count - 2) - (Vector2)player.transform.position;
        float dotValue = Vector2.Dot(compare, playerComp);
        Debug.Log(dotValue);
        if (dotValue > 1)
        {
            swinging = false;
            timer = 0;
            transform.rotation = initialRotation;
            transform.position = initialPosition;
        }
    }

    private void SwingOnRope(GameObject player)
    {
        Vector2 compare = GetPoint(points.Count - 2);
        if (player.transform.localPosition.y < compare.y)
        {
            swinging = true;
        }
    }
}

