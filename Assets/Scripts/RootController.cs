using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    public List<Vector2> points = new();
    public float speed = 5.0f;
    public bool swinging = false;
    public float timer = 0;
    public float swingAngle = 70;
    public float lerpTime = 0.5f;
    public int pointIndex = 0;
    public int swingDirection = 0; // -1 = counterclockwise, 1 = clockwise
    public bool point1Locked = false; // points[0]
    public bool point2Locked = false; // points[points.Count - 1]
    public bool lerping = false;
    public GameObject rock1;
    public GameObject rock2;

    private Vector2 initialPosition;
    private Quaternion initialRotation;
    private Vector2 rotationPoint;

    private void Start()
    {
        MakePoints();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        UpdatePoints();
        if (!point1Locked && !point2Locked)
        {
            if (transform.childCount != 0)
                transform.GetChild(0).parent = null;
            Destroy(gameObject);
        }
        UpdateSwing();
    }

    private void UpdateSwing()
    {
        if (!swinging)
            return;
        if (point1Locked && point2Locked)
            return;
        if (transform.childCount == 0)
        {
            swinging = false;
            timer = 0;
            StartCoroutine(LerpToRest(transform.position, transform.rotation));
            //transform.rotation = initialRotation;
            //transform.position = initialPosition;
        }

        if (swinging)
        {
            timer += Time.deltaTime;
            float angle = Mathf.Sin(timer);
            angle *= swingAngle * swingDirection;
            Quaternion swingRotation = Quaternion.Euler(0, 0, angle);
            Vector2 newPos = rotationPoint + (Vector2)(swingRotation * points[points.Count - 1]);
            transform.position = newPos;
            transform.rotation = swingRotation;
            //transform.RotateAround(rotationPoint, Vector3.forward, swingDirection * angle * Mathf.PI / 360 * Mathf.Cos(timer));
        }
    }


    IEnumerator LerpToRest(Vector2 position, Quaternion rotation)
    {
        if (lerping)
            yield break;
        lerping = true;
        float time = Time.time;
        float progress = 0;

        while (progress < 1)
        {
            if (swinging)
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                lerping = false;
                yield break;
            }
            progress = (Time.time - time) / lerpTime;
            transform.position = Vector2.Lerp(position, initialPosition, progress);
            transform.rotation = Quaternion.Lerp(rotation, initialRotation, progress);
            yield return null;
        }

        lerping = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    private void UpdatePoints()
    {
        if (point1Locked == (rock1 != null) && point2Locked == (rock2 != null))
            return;
        point1Locked = rock1 != null;
        point2Locked = rock2 != null;

        if (!point1Locked || !point2Locked)
        {
            tag = "Rope";
            RotateToRootPosition();
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void RotateToRootPosition()
    {
        Vector2 newPosition = transform.position;
        
        if (point1Locked)
            newPosition -= (Vector2)(Quaternion.Inverse(transform.rotation) * points[points.Count - 1]);
        if (point2Locked)
            newPosition -= (Vector2)(Quaternion.Inverse(transform.rotation) * points[0]);
        newPosition.y -= transform.localScale.y / 2;
        initialPosition = newPosition;
        initialRotation = Quaternion.identity;
        StartCoroutine(LerpToRest(transform.position, transform.rotation));
        //transform.rotation = initialRotation;
        //transform.position = newPosition;


        MakePoints();
    }

    private void MakePoints()
    {
        points.Clear();
        int numPoints = (int)transform.localScale.y + 1;
        Vector2 firstPoint = transform.localScale.y / 2 * Vector2.up;

        for (int i = 0; i < numPoints; i++)
        {
            points.Add(firstPoint + i * Vector2.down);
        }

        rotationPoint = (Vector2)transform.position + points[0];
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
        while (dist2.magnitude < dist1.magnitude && index <= points.Count)
        {
            dist1 = dist2;
            pos2 = GetPoint(index);
            dist2 = (Vector2)player.transform.localPosition - pos2;
            index += 1;
        }
        pointIndex = index - 2;
        if (pointIndex == points.Count - 1 && (point2Locked || point1Locked))
            pointIndex -= 1;
        if (pointIndex == 0 && point1Locked && point1Locked)
            pointIndex += 1;
        return GetPoint(pointIndex);
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
            return;
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
        int endPoint = points.Count - 1;
        if (direction.y > 0)
            dir = -1;
        if (direction.y < 0)
            dir = 1;
        pointIndex += dir;
        if (pointIndex == points.Count)
            pointIndex -= 1;
        if (pointIndex <= 0)
            pointIndex += 1;
        if (pointIndex == points.Count - 1 && point1Locked && point2Locked)
            pointIndex -= 1;
        player.transform.localPosition = GetPoint(pointIndex);

        if (pointIndex != endPoint)
        {
            swinging = false;
            timer = 0;
            transform.rotation = initialRotation;
            transform.position = initialPosition;
        }
    }

    private void SwingOnRope(GameObject player, Vector2 direction)
    {
        return;
        int endPoint = points.Count - 1;
        if (swinging)
            return;
        if (pointIndex == endPoint)
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
                rock.transform.position = transform.position + Quaternion.Inverse(transform.rotation) * pos1;
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
                rock.transform.position = transform.position + Quaternion.Inverse(transform.rotation) * pos1;
                return true;
            }
        }

        UpdatePoints();
        return false;
    }
}
