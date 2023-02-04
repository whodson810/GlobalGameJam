using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
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
    public GameObject rock1;
    public GameObject rock2;

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
        UpdatePoints();
        UpdateSwing();
    }

    private void UpdateSwing()
    {
        if (transform.childCount == 0)
        {
            transform.rotation = initialRotation;
            transform.position = initialPosition;
            swinging = false;
            timer = 0;
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


    private void UpdatePoints()
    {
        point1Locked = rock1 != null;
        point2Locked = rock2 != null;

        if (!point1Locked && !point2Locked)
        {
            Destroy(gameObject);
        }
        else if (!point1Locked || !point2Locked)
        {
            tag = "Rope";
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            tag = "Floor";
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
    private void MakePoints()
    {
        if (points.Count != 0)
            return;
        int numPoints = (int)transform.localScale.y + 1;
        Vector2 firstPoint = transform.localScale.y / 2 * Vector2.up;

        for (int i = 0; i < numPoints; i++)
        {
            points.Add(firstPoint + i * Vector2.down);
        }

        pointIndex = points.Count - 1;
    }

    // Only use this when getting a point to set the player's position
    public Vector2 GetEndPoint(GameObject player)
    {
        int index = 0;
        Vector2 pos1 = GetPoint(index);
        index += 1;
        Vector2 pos2 = GetPoint(index);
        index += 1;
        Vector2 dist1 = (Vector2)player.transform.localPosition - pos1;
        Vector2 dist2 = (Vector2)player.transform.localPosition - pos2;
        Vector2 closest = pos1;
        while (dist2.magnitude < dist1.magnitude && index <= points.Count)
        {
            closest = pos2;
            dist1 = dist2;
            pos2 = GetPoint(index);
            dist2 = (Vector2)player.transform.localPosition - pos2;
            index += 1;
        }
        pointIndex = index - 2;
        return closest;
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
        timer = 0;
        swinging = false;
    }

    public bool RegisterRock(GameObject rock)
    {
        if (points.Count == 0)
            MakePoints();
        Vector2 rockPos = transform.rotation * (rock.transform.position - transform.position);
        if (rock1 == null)
        {
            Vector2 pos1 = GetUnscaledPoint(0);
            Vector2 pos2 = GetUnscaledPoint(1);

            Vector2 dist1 = pos2 - pos1;
            Vector2 dist2 = rockPos - pos1;
            if (dist1.magnitude > dist2.magnitude)
            {
                rock1 = rock;
                rock.transform.position = transform.position + transform.rotation * GetUnscaledPoint(0);
                return true;
            }
        }

        if (rock2 == null)
        {
            Vector2 pos1 = GetUnscaledPoint(points.Count - 1);
            Vector2 pos2 = GetUnscaledPoint(points.Count - 2);

            Vector2 dist1 = pos2 - pos1;
            Vector2 dist2 = rockPos - pos1;
            if (dist1.magnitude > dist2.magnitude)
            {
                rock2 = rock;
                rock.transform.position = transform.position + transform.rotation * GetUnscaledPoint(points.Count - 1);
                return true;
            }
        }

        return false;
    }
}

