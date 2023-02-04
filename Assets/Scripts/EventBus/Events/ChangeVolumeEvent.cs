using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolumeEvent : MonoBehaviour
{
    public int playerIndex;
    public float direction;

    public ChangeVolumeEvent(int playerIndex, float direction)
    {
        this.playerIndex = playerIndex;
        this.direction = direction;
        if (this.direction < 0f)
        {
            Debug.Log($"Player {playerIndex + 1} decreased volume!");
        }
        else if (this.direction > 0f)
        {
            Debug.Log($"Player {playerIndex + 1} increased volume!");
        } 
    }
}
