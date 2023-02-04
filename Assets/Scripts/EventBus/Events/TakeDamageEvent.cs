using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event that triggers when a player takes a damage
/// Created by Hon
/// </summary>
public class TakeDamageEvent
{
    public float damageAmount;
    public int playerIndex;

    public TakeDamageEvent(float damageAmount, int playerIndex)
    {
        this.damageAmount = damageAmount;
        this.playerIndex = playerIndex;
        Debug.Log($"Player {this.playerIndex + 1} took {this.damageAmount} amount of damage!");
    }
}
