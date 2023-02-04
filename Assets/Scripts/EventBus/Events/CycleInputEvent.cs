using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleInputEvent
{
    public int direction;
    public int playerIndex;

    public CycleInputEvent(int _D, int _I)
    {
        direction = _D;
        playerIndex = _I;
    }
}
