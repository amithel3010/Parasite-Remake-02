using UnityEngine;

public class FlyPhysicsControllerInheritenceTest : PhysicsBasedController
{
    protected override bool CanJump()
    {
        return _timeSinceJumpPressed < _jumpBuffer && _jumpReady && _availableJumps > 0;
    }
}
