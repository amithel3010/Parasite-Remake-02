using System;
using UnityEngine;

public class JumpAbility : BaseJumpAbility 
{   
    protected override void Awake()
    {
        if (settings != null)
        {
            _coyoteTime = settings.CoyoteTime;
        }
        base.Awake();
    }

    protected override bool CanJump()
    {
        return _timeSinceJumpPressed < _jumpBuffer &&
               _hover.TimeSinceUngrounded < _coyoteTime &&
               _jumpReady &&
               _availableJumps > 0;
    }
}
