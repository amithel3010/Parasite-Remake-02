using UnityEngine;

public class FlyAbility : BaseJumpAbility
{
    [Header("Fly Specific")] [SerializeField]
    private int _flyMaxJumps = 5;

    [Header("Resource Cost")] [SerializeField]
    private float _jumpCost = 10f;

    private IResource _resource;

    private bool _shouldConsumeResource;

    protected override int MaxJumps => _flyMaxJumps;

    protected override void Awake()
    {
        _resource = GetComponent<Stamina>() as IResource; //Can be changed if needed.
        if (_resource == null)
        {
            Debug.LogWarning($"{name} has no IResource component. FlyAbility won't consume any resource.");
        }

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
        if (_shouldConsumeResource && _resource != null)
        {
            if (_resource.CanAfford(_jumpCost))
            {
                _resource.Change(-_jumpCost);
            }
            else
            {
                //TODO: wait for jump to finish and then:
                _resource.Change(-_jumpCost);
                
            }
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
        _shouldConsumeResource = true;
        base.OnPossessed(playerParasite, inputSource);
    }

    public override void OnUnPossessed(Parasite playerParasite)
    {
        _shouldConsumeResource = false;
        base.OnUnPossessed(playerParasite);
    }
}