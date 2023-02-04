using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event for turning on and off the pause menu
/// Created by Hon
/// </summary>
public class PauseMenuEvent
{
    public int playerIndex;

    public PauseMenuEvent(int playerIndex)
    {
        this.playerIndex = playerIndex;
        Debug.Log($"Player {playerIndex + 1} triggered Paused Menu Event!");
    }
}
