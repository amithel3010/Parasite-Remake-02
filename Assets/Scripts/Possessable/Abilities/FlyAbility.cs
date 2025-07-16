using UnityEngine;

public class FlyAbility : BaseJumpAbility
{

    [Header("Fly Specific")]
    [SerializeField] private int _flyMaxJumps = 5;

    [Header("Stamina?")]
    [SerializeField] private float _jumpCost = 10f;
    [SerializeField] private float _jumpResource = 100f;

    protected override int MaxJumps => _flyMaxJumps;

    protected override void Awake()
    {
        if (settings != null)
        {
            _flyMaxJumps = settings.MaxJumps;
        }
        base.Awake();
    }
    protected override bool CanJump()
    {
        return _timeSinceJumpPressed < _jumpBuffer &&
               _jumpReady &&
               _availableJumps > 0 &&
               _jumpResource > 0;
    }

    protected override void PerformJump()
    {
        base.PerformJump();
        _jumpResource -= _jumpCost;
    }

    protected override float CalcAdjustedJumpHeight()
    {
        return _jumpHeight;
    }
}
