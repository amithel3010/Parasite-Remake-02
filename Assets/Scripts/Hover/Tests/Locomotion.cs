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
    private bool _shouldMaintainHeight = true; //TODO: conflicts with seperation
    private bool isGrounded = true;
    private float _timeSinceJumpPressed = 0.5f; // if it's zero character jumps on start
    private float _timeSinceUngrounded;
    private bool _jumpReady = true;
    private int _availableJumps = 3;

    private int _maxJumps = 1;
    private float _jumpHeight = 50f;
    private readonly float gravity = Physics.gravity.y;


    private float _jumpBuffer = 0.2f;
    private float _coyoteTime = 0.2f;
    private float _jumpForce = 55f;

    public void Tick(Vector2 moveInput, bool jumpPressed)
    {
        CharacterMove(moveInput);
        CharacterJump(jumpPressed);
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

    private void CharacterJump(bool jumpPressed)
    {
        //inconsistent jump. needs to work like move where you calculate needed force for jump
        //or maybe define max and min upwards acceleration values

        _timeSinceJumpPressed += Time.fixedDeltaTime;

        if (_rb.linearVelocity.y < 0)
        {
            //_shouldMaintainHeight = true;
            _jumpReady = true;
        }

        if (jumpPressed)
        {
            _timeSinceJumpPressed = 0f;
        }

        if (CanJump())
        {
            //flags
            _jumpReady = false;
            _shouldMaintainHeight = false;
            //_isJumping = true;
            _availableJumps--;

            float initialJumpVel = math.sqrt(2 * -gravity*_rb.mass * _jumpHeight); //this is goal
            //Debug.Log(initialJumpVel);
            Vector3 goalInitialVel = new Vector3(_rb.linearVelocity.x, initialJumpVel, _rb.linearVelocity.z);
            Vector3 currentVel = _rb.linearVelocity;

            float neededAccel = (goalInitialVel.y - currentVel.y)/ Time.fixedDeltaTime;
            Debug.Log(neededAccel);
            _rb.AddForce(Vector3.up * neededAccel, ForceMode.Force);
             //needs to be only in Y
            //set jump height by 1.distance from ground? 2. distance from ride height?
            //gravity force is Physics.gravity * rb.mass
            //set time to jump height

            //calc initial Yvel required to reach jump height
            //calc current Yvel
            //calc ACCELERATION required to reach that goalYvel in 1 frame
            //apply that acceleration in 1 frame

            //_rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _timeSinceJumpPressed = _jumpBuffer; //to make sure jump only happens once per input
        }
    }

    private bool CanJump()
    {
        return _timeSinceJumpPressed < _jumpBuffer && _jumpReady && _availableJumps > 0;
    }

}

