using UnityEngine;

public class FlyPhysicsControllerInheritenceTest : PhysicsBasedController
{
    protected override bool CanJump()
    {
        return _timeSinceJumpPressed < _jumpBuffer && _jumpReady && _availableJumps > 0;
    }

    protected override void OnActionPressed(bool actionPressed)
    {
        if (actionPressed)
        {
            _parasitePossessing?.StopPossessing();
            Destroy(this.gameObject);
        }
    }
}
