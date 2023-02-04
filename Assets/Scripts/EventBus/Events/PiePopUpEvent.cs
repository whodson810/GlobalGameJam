using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pie Pop Up Event for collecting a pie on the ground
/// </summary>
public class PiePopUpEvent
{
    public int playerIndex;
    public int pieImageIndex;

    public PiePopUpEvent(int playerIndex, int pieImageIndex)
    {
        Debug.Log($"Collected pie Index of {pieImageIndex}");
        this.playerIndex = playerIndex;
        this.pieImageIndex = pieImageIndex;
     }

}
