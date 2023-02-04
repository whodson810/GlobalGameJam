using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event for when a golden NPC spawns in the scene
/// Created by Hon
/// </summary>
public class GoldenNPCSpawnEvent
{
    public GameObject GoldenNPCObject;

    public GoldenNPCSpawnEvent(GameObject goldenNPCObject)
    {
        Debug.Log("Golden NPC Spawn Event!");
        this.GoldenNPCObject = goldenNPCObject;
    }
}
