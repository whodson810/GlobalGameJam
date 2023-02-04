using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowInputEvent
{
    public Vector2 direction;
    public int playerIndex;

    public ThrowInputEvent(int _index)
    {
        playerIndex = _index;
    }
}
