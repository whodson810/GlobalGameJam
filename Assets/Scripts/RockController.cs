using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    public bool cantBeBroken = false;
    public bool isSpike = false;

    GameObject highlight;
    public FallingRootController spike;

    public bool playerNearby { set; get; }

    private void Start()
    {
        playerNearby = false;
        highlight = transform.GetChild(0).gameObject;
        FindRoot();
    }

    private void FindRoot()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        List<RootController> roots = new();
        bool hit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RootController root = hits[i].GetComponent<RootController>();
            if (root != null)
                roots.Add(root);
            if (spike == null)
                spike = hits[i].GetComponent<FallingRootController>();
            hit = (root != null || spike != null) || hit;
        }

        if (spike != null)
        {
            spike.SetAnchorPoint(gameObject);
            return;
        }

        if (!hit)
        {
            Destroy(gameObject);
        }

        bool registered = false;
        foreach (RootController rc in roots)
        {
            registered = rc.RegisterRock(gameObject) || registered;
        }

        if (!registered)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        highlight.SetActive(playerNearby && !cantBeBroken);
    }
}


