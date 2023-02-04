using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vec2InputEvent
{
    public Vector2 direction;
    public int playerIndex;
    public int stickIndex;

    public Vec2InputEvent(Vector2 _direction, int _plindex, int _stindex)
    {
        direction = _direction;
        playerIndex = _plindex;
        stickIndex = _stindex;
    }

    public override string ToString()
    {
        return "direction: " + direction + "\n" +
            "playerIndex: " + playerIndex;
    }
}
