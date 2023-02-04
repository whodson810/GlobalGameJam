using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Event for collecting items
// Created by Yuxuan
// Edited by Hon

public class CollectItemEvent
{
    public int playerIndex;
    public string itemName;
    public int stacksCount; // added stackCount variable for a certain item's stack count

    public Texture rawImage;

    public CollectItemEvent(int playerIndex, string itemName, int stacksCount, Texture rawImage)
    {
        this.playerIndex = playerIndex;
        this.itemName = itemName;
        this.stacksCount = stacksCount;
        this.rawImage = rawImage;
    }
}