using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event for when a golden NPC dies
/// Created by Hon
/// </summary>
public class GoldenNPCDeathEvent
{
    public int scorePlayerIndex;

    public GoldenNPCDeathEvent(int scorePlayerIndex)
    {
        this.scorePlayerIndex = scorePlayerIndex;
        Debug.Log($"Golden NPC has died event: {this.scorePlayerIndex}");
    }
}
