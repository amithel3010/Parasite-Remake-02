using UnityEngine;

public class FlyAbility : BaseJumpAbility
{
    //TODO: custom inspector
    [Header("Fly Specific")]
    [SerializeField] private int _flyMaxJumps = 5;

    [Header("Health Cost")]
    [SerializeField] private float _jumpCost = 10f;
    private Health _health;

    private bool _shouldConsumeHealth;

    protected override int MaxJumps => _flyMaxJumps;

    protected override void Awake()
    {
        _health = GetComponent<Health>();
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
               _availableJumps > 0;
    }

    protected override void PerformJump()
    {
        base.PerformJump();
        if (_shouldConsumeHealth)
        {
            _health.ChangeHealth(-_jumpCost);
            //TODO: make it subscribe to jump apex event
        }

    }

    protected override void OnReachedJumpApex()
    {


    }

    protected override float CalcAdjustedJumpHeight()
    {
        return _jumpHeight;
    }

    public override void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _shouldConsumeHealth = true;
        base.OnPossessed(playerParasite, inputSource);
    }

    public override void OnUnPossessed(Parasite playerParasite)
    {
        _shouldConsumeHealth = false;
        base.OnUnPossessed(playerParasite);
    }
}
