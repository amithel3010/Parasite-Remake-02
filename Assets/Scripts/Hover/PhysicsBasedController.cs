using UnityEngine;

public class PhysicsBasedController : MonoBehaviour
{
    //Keeps Gameobject hovering at a certain ride height using a dampened spring
    //Based on logic by toyful games explained in their video on very very valet

    //in very very valet, the same script is also responsible for moving the player, but i might seperate the two


    [SerializeField] private Vector3 DownDir = Vector3.down; //I have no idea what this is used for

    private Rigidbody _RB;
    private IInputSource _inputSource;
    private Vector3 _previousVelocity = Vector3.zero; //I Would have never thought of this
    public Parasite _parasitePossessing;

    [Header("Height Spring")]
    [SerializeField][Tooltip("Needs to be lower than raycastToGroundLength")] float rideHeight = 1.75f;
    [SerializeField, Range(1, 0)] float springDampingRatio = 0.5f;
    [SerializeField] float rideSpringStrength;
    [SerializeField] float raycastToGroundLength = 3f;

    //movement private vars
    private Vector3 m_GoalDirFromInput; //just input, why is it a member?
    private float m_speedFactor = 1f;
    private float m_maxAccelForceFactor = 1f;
    private Vector3 m_GoalVel = Vector3.zero;

    [Header("Movement")]
    [SerializeField] private float _maxSpeed = 8f;
    [SerializeField] private float _acceleration = 200f;
    [SerializeField] private float _maxAccelForce = 150f;
    [SerializeField] private float _leanFactor = 0.25f;
    [SerializeField] private AnimationCurve _accelerationFactorFromDot;
    [SerializeField] private AnimationCurve _maxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 _moveForceScale = new Vector3(1f, 0f, 1f);

    //rotation private vars
    private enum lookDirectionOptions { velocity, acceleration, moveInput }
    private Quaternion _uprightTargetRot = Quaternion.identity;

    [Header("Upright Spring")]
    [SerializeField] private lookDirectionOptions _charcterLookDirection = lookDirectionOptions.velocity;
    [SerializeField] private float _uprightSpringDamper = 5f;
    [SerializeField] private float _uprightSpringStrength = 40f;

    //jumping private vars
    private bool _shouldMaintainHeight = true;
    private bool isGrounded = true;
    protected float _timeSinceJumpPressed = 0.5f; // if it's zero character jumps on start
    private float _timeSinceUngrounded;
    protected bool _jumpReady = true;
    protected int _availableJumps;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 20f; //TODO: means nothing
    [SerializeField] protected float _jumpBuffer;
    [SerializeField] private float _coyoteTime;
    [SerializeField] private int _maxJumps = 1;

    [Header("Other")]
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        _inputSource = GetComponent<IInputSource>();
    }

    void FixedUpdate()
    {
        OnActionPressed(_inputSource.ActionPressed);

        (bool isRayHittingGround, RaycastHit groundRayHitInfo) = RaycastToGround();

        isGrounded = CheckIfGrounded(isRayHittingGround, groundRayHitInfo);

        if (isGrounded)
        {
            _timeSinceUngrounded = 0f;
            ResetNumberOfJumps();
        }
        else
        {
            _timeSinceUngrounded += Time.fixedDeltaTime;
        }

        CharacterMove(_inputSource.MovementInput);
        CharacterJump(_inputSource.JumpPressed, groundRayHitInfo);

        if (isRayHittingGround && _shouldMaintainHeight)
        {
            MaintainHeight(groundRayHitInfo);
        }

        Vector3 lookDirection = GetLookDirection();
        MaintainUpright(lookDirection);
        //Maintain Upright
    }


    private void MaintainHeight(RaycastHit rayHit)
    {
        // bool _rayDidHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit _rayhit, raycastToGroundLength);

        Debug.DrawLine(transform.position, rayHit.point, Color.green); // actual ray hit
        Debug.DrawRay(rayHit.point, Vector3.up * rideHeight, Color.yellow); // target ride height

        Vector3 vel = _RB.linearVelocity;
        Vector3 rayDir = transform.TransformDirection(DownDir); // same as transform.down?

        Vector3 othervel = Vector3.zero;
        Rigidbody hitBody = rayHit.rigidbody;
        if (hitBody != null)
        {
            othervel = hitBody.linearVelocity;
        }

        float rayDirVel = Vector3.Dot(rayDir, vel);
        float otherDirVel = Vector3.Dot(rayDir, othervel); //what is dot and how is it used here?

        float relVel = rayDirVel - otherDirVel;

        float mass = _RB.mass;
        float rideSpringDamper = 2f * Mathf.Sqrt(rideSpringStrength * mass) * springDampingRatio; //from zeta formula 

        float x = rayHit.distance - rideHeight;

        float springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);

        _RB.AddForce(rayDir * springForce);

        if (hitBody != null)
        {
            hitBody.AddForceAtPosition(rayDir * -springForce, rayHit.point);
        }
    }

    private Vector3 GetLookDirection()
    {
        Vector3 lookDirection = Vector3.zero;

        if (_charcterLookDirection == lookDirectionOptions.velocity || _charcterLookDirection == lookDirectionOptions.acceleration)
        {
            Vector3 velocity = _RB.linearVelocity;
            velocity.y = 0;

            if (_charcterLookDirection == lookDirectionOptions.velocity)
            {
                lookDirection = velocity;
            }
            else if (_charcterLookDirection == lookDirectionOptions.acceleration)
            {
                Vector3 deltaVelocity = velocity - _previousVelocity;
                _previousVelocity = velocity;
                Vector3 acceleration = deltaVelocity / Time.fixedDeltaTime;
                lookDirection = acceleration;
            }
        }
        else if (_charcterLookDirection == lookDirectionOptions.moveInput)
        {
            lookDirection = new Vector3(_inputSource.MovementInput.x, 0, _inputSource.MovementInput.y); //TODO: make this vector 3 a part of inputhandler? its reused a lot
        }

        return lookDirection;
    }

    private void MaintainUpright(Vector3 lookDir)
    {
        if (lookDir != Vector3.zero)
        {
            _uprightTargetRot = Quaternion.LookRotation(lookDir, Vector3.up);
        }

        Quaternion currentRot = transform.rotation;
        Quaternion toGoal = MathUtils.ShortestRotation(_uprightTargetRot, currentRot);

        Vector3 rotAxis;
        float rotDegrees;

        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        _RB.AddTorque(rotAxis * (rotRadians * _uprightSpringStrength) - (_RB.angularVelocity * _uprightSpringDamper));

    }
    private (bool, RaycastHit) RaycastToGround()
    {
        Vector3 _rayDir = transform.TransformDirection(DownDir);

        RaycastHit rayHit;
        Ray rayToGround = new Ray(transform.position, _rayDir);
        bool rayHitGround = Physics.Raycast(rayToGround, out rayHit, raycastToGroundLength, groundLayer.value);

        //Debug.DrawRay(transform.position, _rayDir * _rayToGroundLength, Color.blue);

        return (rayHitGround, rayHit);
    }

    private bool CheckIfGrounded(bool rayHitGround, RaycastHit rayHit)
    {
        bool grounded;
        if (rayHitGround)
        {
            grounded = rayHit.distance <= rideHeight * 1.3f; // 1.3f? multiplied because object will oscilate but 1.3 is random
        }
        else
        {
            grounded = false;
        }

        return grounded;
    }

    private void CharacterMove(Vector2 moveInput)
    {
        m_GoalDirFromInput = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        //calculate new goal vel...
        Vector3 unitDir = m_GoalVel.normalized; //current vel direction

        float velDot = Vector3.Dot(m_GoalDirFromInput, unitDir); // checking difference in direction in current input and current velocity direction?
        float accel = _acceleration * _accelerationFactorFromDot.Evaluate(velDot); //should be between 0 and 1?
                                                                                   // Debug.Log("VelDot:" + velDot + ", accel:" + accel);

        Vector3 goalVel = _maxSpeed * m_speedFactor * m_GoalDirFromInput; //velocity at its max

        //THIS IS THE ACTUAL CALCULATION OF THE NEW m_goalVel
        m_GoalVel = Vector3.MoveTowards(m_GoalVel, goalVel, accel * Time.fixedDeltaTime);

        //actual force
        Vector3 neededAccel = (m_GoalVel - _RB.linearVelocity) / Time.fixedDeltaTime; // acceleration needed to reach max accel in 1 fixed update?

        float maxAccel = _maxAccelForce * _maxAccelerationForceFactorFromDot.Evaluate(velDot) * m_maxAccelForceFactor;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        _RB.AddForceAtPosition(Vector3.Scale(neededAccel * _RB.mass, _moveForceScale), transform.position + new Vector3(0f, transform.localScale.y * _leanFactor, 0f)); // Using AddForceAtPosition in order to both move the player and cause the play to lean in the direction of input.
        //_RB.AddForceAtPosition(Vector3.Scale(neededAccel * _RB.mass, _moveForceScale), transform.position);
    }

    private void CharacterJump(bool jumpPressed, RaycastHit rayHit)
    {
        //inconsistent jump. needs to work like move where you calculate needed force for jump
        //or maybe define max and min upwards acceleration values

        _timeSinceJumpPressed += Time.fixedDeltaTime;

        if (_RB.linearVelocity.y < 0)
        {
            _shouldMaintainHeight = true;
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

            _RB.linearVelocity = new Vector3(_RB.linearVelocity.x, 0f, _RB.linearVelocity.z); //TODO: cheat fix by Joe Binns. I would like to have calculations for needed accel like in CharacterMove(). 

            //jump
            if (rayHit.distance != 0)
            {
                _RB.position = new Vector3(_RB.position.x, _RB.position.y - (rayHit.distance - rideHeight), _RB.position.z);
            }

            _RB.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _timeSinceJumpPressed = _jumpBuffer; //to make sure jump only happens once per input
        }
    }

    protected virtual bool CanJump()
    {
        return _timeSinceJumpPressed < _jumpBuffer && _timeSinceUngrounded < _coyoteTime && _jumpReady && _availableJumps > 0;
    }

    protected virtual void OnActionPressed(bool actionPressed)
    {
        if (actionPressed)
        {
            if (_parasitePossessing == null)
            {
                Debug.Log("Action pressed, but is parasite");
                return;
            }
            else
            {
                _parasitePossessing.StopPossessing();
            }
        }
    }

    private void ResetNumberOfJumps()
    {
        _availableJumps = _maxJumps;
    }

    // public void ChangeInputSource(IInputSource newInputSource)
    // {
    //     _inputSource = newInputSource;
    // }

    public void OnPossess(IInputSource newInputSource, Parasite parasite)
    {
        _inputSource = newInputSource;
        _parasitePossessing = parasite;
    }

    public void OnUnPossess()
    {
        _inputSource = GetComponent<IInputSource>();
        _parasitePossessing = null;
    }
}

