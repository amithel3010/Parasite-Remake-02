using UnityEngine;

public abstract class Hover : MonoBehaviour, IControllable
{
    //Keeps Gameobject hovering at a certain ride height using a dampened spring
    //Based on logic by toyful games explained in their video on very very valet

    //in very very valet, the same script is also responsible for moving the player, but i might seperate the two


    private Rigidbody _RB;
    protected IInputSource InputSource;

    [SerializeField] private Vector3 DownDir = Vector3.down;

    [Header("Ride Properties")]
    [SerializeField][Tooltip("Needs to be lower than raycastToGroundLength")] float rideHeight = 1.75f;
    [SerializeField, Range(1, 0)] float springDampingRatio = 0.5f;
    [SerializeField] float rideSpringStrength;
    [SerializeField] float raycastToGroundLength = 3f;

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

    [Header("Other")]
    [SerializeField] private InputHandler _input;
    [SerializeField] private LayerMask groundLayer;


    public virtual void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        InputSource = FindAnyObjectByType<InputHandler>(); //feels wrong
    }

    public virtual void OnFixedUpdate()
    {
        (bool isRayHittingGround, RaycastHit groundRayHitInfo) = RaycastToGround();

        bool isGrounded = CheckIfGrounded(isRayHittingGround, groundRayHitInfo);

        if (isGrounded)
        {
            //grounded logic
        }

        if (InputSource != null)
        {
            OnMovementInput(InputSource.MovementInput);
            OnJumpInput(InputSource.JumpPressed);           
        }

        if (isRayHittingGround)
        {
            MaintainHeight(groundRayHitInfo);
        }

        //Maintain Upright
    }


    public void OnMovementInput(Vector2 moveInput)
    {
        CharacterMove(moveInput);
    }

    public virtual void OnJumpInput(bool jumpInput)
    {
        if(jumpInput)
            Debug.Log("Jumped");
    }

    public virtual void OnActionInput(bool actionInput)
    {
        if(actionInput)
            Debug.Log("Used Action");
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
        float rideSpringDamper = 2f * Mathf.Sqrt(rideSpringStrength * mass) * springDampingRatio; //from zeta formula, 

        float x = rayHit.distance - rideHeight;

        float springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);

        _RB.AddForce(rayDir * springForce);

        if (hitBody != null)
        {
            hitBody.AddForceAtPosition(rayDir * -springForce, rayHit.point);
        }
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
        //Debug.Log("VelDot:" + velDot + ", accel:" + accel);

        Vector3 goalVel = _maxSpeed * m_speedFactor * m_GoalDirFromInput; //velocity at its max

        //THIS IS THE ACTUAL CALCULATION OF THE NEW m_goalVel
        m_GoalVel = Vector3.MoveTowards(m_GoalVel, goalVel, accel * Time.fixedDeltaTime);

        //actual force
        Vector3 neededAccel = (m_GoalVel - _RB.linearVelocity) / Time.fixedDeltaTime; // acceleration needed to reach max accel in 1 fixed update?

        float maxAccel = _maxAccelForce * _maxAccelerationForceFactorFromDot.Evaluate(velDot) * m_maxAccelForceFactor;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        // _RB.AddForceAtPosition(Vector3.Scale(neededAccel * _RB.mass, _moveForceScale), transform.position + new Vector3(0f, transform.localScale.y * _leanFactor, 0f)); // Using AddForceAtPosition in order to both move the player and cause the play to lean in the direction of input.
        _RB.AddForceAtPosition(Vector3.Scale(neededAccel * _RB.mass, _moveForceScale), transform.position);
    }

}

