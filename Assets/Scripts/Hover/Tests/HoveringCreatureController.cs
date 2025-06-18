using UnityEngine;

public class HoveringCreatureController : MonoBehaviour
{
    [SerializeField] private HoverSettings _hoverSettings;
    [SerializeField] private LocomotionSettings _locomotionSettings;

    //there should be a ground checker class, that gets a ride height from hover

    [SerializeField] private bool _enableHover;
    [SerializeField] private bool _enableMovement;
    
    private MaintainHeightAndUpright _hover;
    private Locomotion _locomotion;
    private Rigidbody _rb;
    private IInputSource _inputSource;

    void Awake()
    {
        _inputSource = GetComponent<IInputSource>();
        _rb = GetComponent<Rigidbody>();
        _hover = new MaintainHeightAndUpright(_rb, _hoverSettings);
        _locomotion = new Locomotion(_rb, _locomotionSettings);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_enableMovement) _locomotion.Tick(_inputSource.MovementInput, _inputSource.JumpPressed, _hover._rideHeight);
        // TODO: find a way to pass isGrounded, timeSinceUngrounded, currentDistanceFromGround

        Vector3 lookDir = GetLookDir();
        //TODO: find a replacement for ShouldMaintainHeight
        if(_enableHover)_hover.Tick(lookDir, !_locomotion.IsJumping);
    }

    private Vector3 GetLookDir()
    {
        Vector3 lookDir = Vector3.zero;
        lookDir = new Vector3(_inputSource.MovementInput.x, 0, _inputSource.MovementInput.y);
        return lookDir;
    }
}
