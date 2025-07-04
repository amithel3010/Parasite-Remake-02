using Unity.Mathematics;
using UnityEngine;

public class Locomotion
{
    private readonly Rigidbody _rb;

    public Locomotion(Rigidbody rb, LocomotionSettings settings)
    {
        _rb = rb;

        _maxSpeed = settings.MaxSpeed;
        _acceleration = settings.Acceleration;
        _maxAccelForce = settings.MaxAccelForce;
        _leanFactor = settings.LeanFactor;
        _accelerationFactorFromDot = settings.AccelerationFactorFromDot;
        _maxAccelerationForceFactorFromDot = settings.MaxAccelerationForceFactorFromDot;
        _moveForceScale = settings.MoveForceScale;

        _maxJumps = settings.MaxJumps;
        _jumpHeight = settings.JumpHeight;
        _jumpBuffer = settings.JumpBuffer;
        _coyoteTime = settings.CoyoteTime;

        _isflying = settings.IsFlying;

        _availableJumps = _maxJumps;
    }

    //movement
    private float _maxSpeed = 4f;
    private float _acceleration = 25f;
    private float _maxAccelForce = 150f;
    private float _leanFactor = 0.25f;
    private AnimationCurve _accelerationFactorFromDot;
    private AnimationCurve _maxAccelerationForceFactorFromDot;
    private Vector3 _moveForceScale = new Vector3(1f, 0f, 1f);

    private Vector3 _GoalDirFromInput;
    private Vector3 _GoalVel;
    private float _speedFactor = 1f; //Does nothing?
    private float _maxAccelForceFactor = 1f; //also does nothin?

    //jumping
    public bool IsJumping;
    private float _timeSinceJumpPressed = 0.5f; // if it's zero character jumps on start
    private bool _jumpReady = true;
    private int _availableJumps = 1;
    private bool _isflying = false;

    //debug
    public Vector3 _debugJumpheight;

    private readonly int _maxJumps = 1;
    private float _jumpHeight = 5f;
    private float _jumpBuffer = 0.2f;
    private float _coyoteTime = 0.2f;

    public void Tick(Vector2 moveInput, bool jumpPressed, GroundChecker groundCheckerInfo)
    {
        CharacterMove(moveInput);
        //StaticCharacterJump(jumpPressed);
        HoveringCharacterJump(jumpPressed, groundCheckerInfo);
    }

    private void CharacterMove(Vector2 moveInput) //might be better if moveInput was a field
    {
        _GoalDirFromInput = new Vector3(moveInput.x, 0, moveInput.y).normalized;

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

    private void HoveringCharacterJump(bool jumpPressed, GroundChecker groundChecker)
    {

        _timeSinceJumpPressed += Time.fixedDeltaTime;

        if (groundChecker.IsGrounded)
        {
            _availableJumps = _maxJumps;
        }

        if (_rb.linearVelocity.y < 0)
        {
            _jumpReady = true;
            IsJumping = false;
        }

        if (jumpPressed)
        {
            _timeSinceJumpPressed = 0f;
        }

        if (CanJump(groundChecker))
        {
            //flags
            _jumpReady = false;
            IsJumping = true;
            _availableJumps--;

            //Ypos changing due to spring:
            //FIRST we have to stop maintaining height!!!! doable with IsJumping Public Bool
            //jump height should be fixed somewhere above player, no need for distance from ground// cheat right now we do get ride height
            //calc jump height from current pos
            float adjustedJumpHeight = _jumpHeight - groundChecker.CurrentDistanceFromGround; //still has small inconsistencies but I cant figure out why, and it's for sure good enough.
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
    }

    private bool CanJump(GroundChecker groundChecker)
    {
        if (!_isflying)
        {
            return _timeSinceJumpPressed < _jumpBuffer && groundChecker.TimeSinceUngrounded < _coyoteTime && _jumpReady && _availableJumps > 0;
        }
        else
        {
            return _timeSinceJumpPressed < _jumpBuffer && _jumpReady && _availableJumps > 0;
        }
    }
}

