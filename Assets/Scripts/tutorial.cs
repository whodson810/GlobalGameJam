using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    GameObject player;
    private void Start()
    {
        if (transform.position.y > 0 && transform.position.y < 70)
        {
            enabled = false;
        }

        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (player == null)
            return;
        if (transform.position.y < 0)
            transform.GetChild(0).gameObject.SetActive(player.transform.position.y < 0);
        if (transform.position.y > 70)
            transform.GetChild(0).gameObject.SetActive(player.transform.position.y > 70);

    }
}
