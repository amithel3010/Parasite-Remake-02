using System;
using UnityEngine;

[RequireComponent(typeof(Hover))]
public class InputBasedHoverMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private MonoBehaviour _inputSourceProvider; // for seeing in inspector
    private Hover _hover;
    private IInputSource _inputSource;
    private Rigidbody _rb;

    [Header("Movement")]
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _acceleration = 25f;
    [SerializeField] private float _maxAccelForce = 150f;
    [SerializeField] private float _leanFactor = 0.25f;
    [SerializeField] private AnimationCurve _accelerationFactorFromDot;
    [SerializeField] private AnimationCurve _maxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 _moveForceScale = new Vector3(1f, 0f, 1f);

    private Vector3 _GoalDirFromInput;
    private Vector3 _GoalVel;
    private float _speedFactor = 1f; //Does nothing?
    private float _maxAccelForceFactor = 1f; //also does nothin?

    [Header("Jumping")]
    [SerializeField] private int _maxJumps = 1;
    [SerializeField] private float _jumpHeight = 5f;
    [SerializeField] private float _jumpBuffer = 0.2f;
    [SerializeField] private float _coyoteTime = 0.2f;
    [SerializeField] private bool _isFlying = false;

    private bool _isJumping;
    private float _timeSinceJumpPressed = 0.5f; // if it's zero character jumps on start
    private bool _jumpReady = true;
    private int _availableJumps = 1;
    private bool _wasGrounded;

    public event Action OnLanding;

    [Header("Debug")]
    [SerializeField] private bool _debugJumpheight; //TODO: unimplemented


    void Awake()
    {
        _hover = GetComponent<Hover>();
        _rb = GetComponent<Rigidbody>(); // TODO: should get ref from hover?

        if (_inputSourceProvider == null)
            _inputSourceProvider = GetComponent<IInputSource>() as MonoBehaviour;

        _inputSource = _inputSourceProvider as IInputSource;

        if (_inputSource == null)
            Debug.LogError($"{name}: No IInputSource assigned or found.");
    }

    void FixedUpdate()
    {
        if (_inputSource == null) return;

        CharacterMove(_inputSource.HorizontalMovement);
        CharacterJump(_inputSource.JumpPressed);
    }

    private void CharacterMove(Vector3 horizontalMoveInput) //might be better if moveInput was a field
    {
        _GoalDirFromInput = horizontalMoveInput.normalized;

        //calculate new goal vel...
        Vector3 unitDir = _GoalVel.normalized; //current vel direction

        float velDot = Vector3.Dot(_GoalDirFromInput, unitDir); // checking difference in direction in current input and current velocity direction?
        float accel = _acceleration * _accelerationFactorFromDot.Evaluate(velDot); //should be between 0 and 1?
                                                                                   // Debug.Log("VelDot:" + velDot + ", accel:" + accel);

        Vector3 goalVel = _maxSpeed * _speedFactor * _GoalDirFromInput; //velocity at its max

        //THIS IS THE ACTUAL CALCULATION OF THE NEW m_goalVel
        _GoalVel = Vector3.MoveTowards(_GoalVel, goalVel, accel * Time.fixedDeltaTime);

        //actual force
        Vector3 neededAccel = (_GoalVel - _rb.linearVelocity) / Time.fixedDeltaTime; // acceleration needed to reach max accel in 1 fixed update?

        float maxAccel = _maxAccelForce * _maxAccelerationForceFactorFromDot.Evaluate(velDot) * _maxAccelForceFactor;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        _rb.AddForceAtPosition(Vector3.Scale(neededAccel * _rb.mass, _moveForceScale), _rb.position + new Vector3(0f, _rb.transform.localScale.y * _leanFactor, 0f)); // Using AddForceAtPosition in order to both move the player and cause the play to lean in the direction of input.
    }

    private void CharacterJump(bool jumpPressed)
    {

        _timeSinceJumpPressed += Time.fixedDeltaTime;

        if (_hover.IsGrounded)
        {
            if (!_wasGrounded)
            {
                OnLanding?.Invoke();
                Debug.Log("Landed");
            }
            _availableJumps = _maxJumps;
        }

        if (_rb.linearVelocity.y < 0)
        {
            _jumpReady = true;
            _isJumping = false;
            _hover.SetMaintainHeight(true);
        }

        if (jumpPressed)
        {
            _timeSinceJumpPressed = 0f;
        }

        if (CanJump())
        {
            //flags
            _jumpReady = false;
            _isJumping = true;
            _hover.SetMaintainHeight(false);
            _availableJumps--;

            //Ypos changing due to spring:
            //FIRST we have to stop maintaining height!!!! doable with IsJumping Public Bool
            //jump height should be fixed somewhere above player, no need for distance from ground// cheat right now we do get ride height
            //calc jump height from current pos
            float adjustedJumpHeight = _jumpHeight - _hover.CurrentDistanceFromGround; //still has small inconsistencies but I cant figure out why, and it's for sure good enough.
                                                                                              //Debug.Log($"current distance from ground: {groundChecker.CurrentDistanceFromGround}, jump height: {_jumpHeight}, adjusted jump height: {adjustedJumpHeight}");


            //could V0 be regarded as 0? probably not. we need to get the  difference in velocity needed to be applied this frame to reach that height
            float goalVel = Mathf.Sqrt(adjustedJumpHeight * -2 * Physics.gravity.y);
            float currentVel = _rb.linearVelocity.y;
            //meaning RequiredVel = GoalVel - currentVel 
            float requiredVel = goalVel - currentVel;
            //then , we need the force needd for that velocity change (acceleration)
            // jumpForce = RequiredVel * rb.mass
            float jumpForce = requiredVel * _rb.mass;
            //add impulse force;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            _timeSinceJumpPressed = _jumpBuffer; //to make sure jump only happens once per input
        }
        _wasGrounded = _hover.IsGrounded;
    }

    private bool CanJump()
    {
        if (!_isFlying)
        {
            return _timeSinceJumpPressed < _jumpBuffer && _hover.TimeSinceUngrounded < _coyoteTime && _jumpReady && _availableJumps > 0;
        }
        else
        {
            return _timeSinceJumpPressed < _jumpBuffer && _jumpReady && _availableJumps > 0;
        }
    }

}
