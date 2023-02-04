using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For winning the game
// Created by Yuxuan

public class WinEvent
{
    public int winningPlayerIndex;

    public WinEvent(int winningPlayerIndex)
    {
        Debug.Log(winningPlayerIndex + " won");
        this.winningPlayerIndex = winningPlayerIndex;
    }
}
