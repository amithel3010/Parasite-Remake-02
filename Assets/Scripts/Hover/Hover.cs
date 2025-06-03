using UnityEngine;

public class Hover : MonoBehaviour
{
    public Vector3 DownDir = Vector3.down;

    private Rigidbody _RB;
    private InputHandler input;

    private Vector3 moveInput;
    private float _speedFactor = 1f;
    private float _maxAccelForceFactor = 1f;
    private Vector3 _m_GoalVel = Vector3.zero;

    [Header("Ride Properties")]
    [SerializeField] [Tooltip("Needs to be lower than raycastToGroundLength")] float RideHeight = 1.75f;
    [SerializeField, Range(1, 0)] float springDampingRatio = 0.5f;
    [SerializeField] float rideSpringStrength; //?
    [SerializeField] float raycastToGroundLength = 3f;
    float rideSpringDamper;

    [Header("Movement:")]
    [SerializeField] private float _maxSpeed = 8f;
    [SerializeField] private float _acceleration = 200f;
    [SerializeField] private float _maxAccelForce = 150f;
    [SerializeField] private float _leanFactor = 0.25f;
    [SerializeField] private AnimationCurve _accelerationFactorFromDot;
    [SerializeField] private AnimationCurve _maxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 _moveForceScale = new Vector3(1f, 0f, 1f);


    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MaintainHeight();
    }


    private void MaintainHeight()
    {
        bool _rayDidHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit _rayhit, raycastToGroundLength);

        if (_rayDidHit)
        {

            Debug.DrawLine(transform.position, _rayhit.point, Color.green); // actual ray hit
            Debug.DrawRay(_rayhit.point, Vector3.up * RideHeight, Color.yellow); // target ride height

            Vector3 vel = _RB.linearVelocity;
            Vector3 rayDir = transform.TransformDirection(DownDir); // same as transform.down?

            Vector3 othervel = Vector3.zero;
            Rigidbody hitBody = _rayhit.rigidbody;
            if (hitBody != null)
            {
                othervel = hitBody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, othervel); //what is dot and how is it used here?

            float relVel = rayDirVel - otherDirVel;

            float mass = _RB.mass;
            rideSpringDamper = 2f * Mathf.Sqrt(rideSpringStrength * mass) * springDampingRatio;

            float x = _rayhit.distance - RideHeight;

            float springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);

            _RB.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, _rayhit.point);
            }

        }
    }

    private void CharacterMove(Vector3 moveInput, RaycastHit rayHit)
    {
        Vector3 m_UnitGoal = moveInput;

        Vector3 unitVel = _m_GoalVel.normalized; //pat goal?

        float velDot = Vector3.Dot(m_UnitGoal, unitVel);
        float accel = _acceleration * _accelerationFactorFromDot.Evaluate(velDot); // animation curve evaluate. need to learn syntax

        Vector3 goalVel = m_UnitGoal * _maxSpeed * _speedFactor; //????

        Vector3 otherVel = Vector3.zero;// ?????

        Rigidbody hitBody = rayHit.rigidbody;

        _m_GoalVel = Vector3.MoveTowards(_m_GoalVel, goalVel, accel * Time.fixedDeltaTime);

        Vector3 neededAccel = (_m_GoalVel - _RB.linearVelocity) / Time.fixedDeltaTime; // vel needed to reach max accel in 1 fixed update?

        float maxAccel = _maxAccelForce * _maxAccelerationForceFactorFromDot.Evaluate(velDot) * _maxAccelForceFactor;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        _RB.AddForceAtPosition(Vector3.Scale(neededAccel * _RB.mass, _moveForceScale), transform.position + new Vector3(0f, transform.localScale.y * _leanFactor, 0f)); // Using AddForceAtPosition in order to both move the player and cause the play to lean in the direction of input.
    }

}

