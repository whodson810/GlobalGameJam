using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingInputEvent
{
    public int playerIndex;
    public bool isAiming;

    public AimingInputEvent(int _ind, bool _aim)
    {
        playerIndex = _ind;
        isAiming = _aim;
    }
}
