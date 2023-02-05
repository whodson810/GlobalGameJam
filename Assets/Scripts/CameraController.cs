using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public List<Vector2> positions;
    public List<Vector2> scales;
    public int index = 0;
    public BoxCollider2D bc;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
    }

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, bc.size, 0);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
                return;
        }

        if (player.transform.position.x < transform.position.x)
        {
            index -= 1;
        }
        else
        {
            index += 1;
            if (index == positions.Count)
                index -= 1;
        }

        transform.position = positions[index];
        bc.size = scales[index];
    }
}
