using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    GameObject highlight;
    RootController root;

    public bool playerNearby { set; get; }

    private void Start()
    {
        playerNearby = false;
        highlight = transform.GetChild(0).gameObject;
        FindRoot();
        root.RegisterRock(gameObject);
    }

    private void FindRoot()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        bool hit = false;
        for (int i = 0; i < hits.Length && !hit; i++)
        {
            root = hits[i].GetComponent<RootController>();
            hit = root != null;
        }

        if (!hit)
            Destroy(gameObject);
    }

    private void Update()
    {
        highlight.SetActive(playerNearby);
    }
}
