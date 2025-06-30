using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoveringCreatureController : MonoBehaviour
{
    [SerializeField] private HoverSettings _hoverSettings;
    [SerializeField] private LocomotionSettings _locomotionSettings;
    [SerializeField] private GroundCheckerSettings _groundCheckerSettings;

    [SerializeField] private bool _enableHover;
    [SerializeField] private bool _enableMovement;

    private MaintainHeightAndUpright _hover;
    private Locomotion _locomotion;
    private GroundChecker _groundChecker;

    private KnockbackTest _knockback;

    private Rigidbody _rb;
    private IInputSource _inputSource;
    private IInputSource _defaultInputSource;

    //TODO: currently coupled: 
    //1.ride height needs to be shared in hover and ground check
    //2.hover needs to know about locomotion's IsJumping

    //maybe I can accept that ground check is REQUIRED for both locomotion and hover, ill move all of hovers parameters to ground check

    void Awake()
    {

        _defaultInputSource = GetComponent<IInputSource>();
        _inputSource = _defaultInputSource;
        _rb = GetComponent<Rigidbody>();
        _knockback = GetComponent<KnockbackTest>();
        _hover = new MaintainHeightAndUpright(_rb, _hoverSettings);
        _locomotion = new Locomotion(_rb, _locomotionSettings);
        _groundChecker = new GroundChecker(_rb, _groundCheckerSettings, _hoverSettings);
    }

    void Start()
    {
        if (_inputSource == null)
        {
            Debug.Log("Missing Input Source");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _groundChecker?.Tick();

        if (_enableMovement)
        {
            //TODO: must be a cleaner way to check if knockback is null
            if (_knockback != null)
            {
                if (!_knockback.IsKnockedBack)
                {
                    _locomotion?.Tick(_inputSource.MovementInput, _inputSource.JumpPressed, _groundChecker);
                }
            }
            else if (_knockback == null)
            {
                _locomotion?.Tick(_inputSource.MovementInput, _inputSource.JumpPressed, _groundChecker);
            }
        }

        Vector3 lookDir = GetLookDir();
        //TODO: find a replacement for ShouldMaintainHeight

        if (_enableHover)
        {
            _hover?.Tick(lookDir, !_locomotion.IsJumping, _groundChecker);
        }
    }

    private Vector3 GetLookDir()
    {
        Vector3 lookDir = Vector3.zero;
        lookDir = new Vector3(_inputSource.MovementInput.x, 0, _inputSource.MovementInput.y);
        return lookDir;
    }

    public void OnPossess(IInputSource newInputSource)
    {
        _inputSource = newInputSource;
    }

    public void OnUnPossess()
    {
        _inputSource = _defaultInputSource;
    }

    void OnValidate()
    {
        if (_groundCheckerSettings.RaycastToGroundLength < _hoverSettings.RideHeight)
        {
            Debug.Log(this + "make sure raycast length is longer than ride height!");
            _groundCheckerSettings.RaycastToGroundLength = _hoverSettings.RideHeight;
        }
    }

}
