using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hover : MonoBehaviour, IPossessionSource
{
    //Maintain Height and Upright with springs

    [TextAreaAttribute]
    public string Warning = "Please note that for now _uprightSpringDamper, _uprightSpringStrength and _rideSpringStrength require exiting play mode to change properly if changing them or the rigidbody's mass!";

    public bool _isActive { get; private set; } = true;

    [Header("Ground Check")]
    [SerializeField] private Vector3 _downDir = Vector3.down;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField][Min(0.1f)] private float _raycastToGroundLength = 1.5f;

    private bool _rayHitGround;
    private RaycastHit _rayHit; // should be a local var?
    private bool _isGrounded;
    private float _currentDistanceFromGround;
    private float _timeSinceUngrounded;

    public bool IsGrounded => _isGrounded;
    public float CurrentDistanceFromGround => _currentDistanceFromGround;
    public float TimeSinceUngrounded => _timeSinceUngrounded;

    [Header("Height Spring Settings")]
    [SerializeField][Min(0.1f)] private float _rideHeight = 0.93f;
    [SerializeField][Min(0.1f)] private float _rideSpringStrength = 1000f;
    [Range(0, 1)] public float _rideSpringDampingRatio = 0.5f;

    private float _defaultRideHeight;

    private bool _shouldMaintainHeight = true;
    public bool ShouldMaintainHeight => _shouldMaintainHeight; //useless?

    [Header("Upright Spring Settings")]
    [SerializeField][Min(0.1f)] private float _uprightSpringDamper = 25f;
    [SerializeField][Min(0.1f)] private float _uprightSpringStrength = 800f;

    private Quaternion _uprightTargetRot = Quaternion.identity;

    [Header("Debug")]
    [SerializeField] private bool _showGroundRay;
    [SerializeField] private float _debugRayThickness = 3f;
    [SerializeField] private bool _knockBackDisablesHover = false;

    // --- refs ---
    private Rigidbody _rb;
    private Rigidbody _hitBody;
    private MonoBehaviour _knockbackProvider; // for seeing in inspector
    private IKnockbackStatus _knockbackStatus;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        AdjustSpringValuesToMass();

        if (_knockbackProvider == null)
            _knockbackProvider = GetComponent<IKnockbackStatus>() as MonoBehaviour;
        _knockbackStatus = _knockbackProvider as IKnockbackStatus;

        _defaultRideHeight = _rideHeight;

    }

    private void FixedUpdate()
    {

        if (!_isActive) return;

        RaycastToGround();

        if (_shouldMaintainHeight)
        {
            if (!(_knockBackDisablesHover && _knockbackStatus.IsKnockedBack))
            {
                MaintainHeight();
            }
        }

        Vector3 lookDir = GetStableLookDirection();
        MaintainUpright(lookDir);

        if (_showGroundRay)
        {
            DrawGroundRay();
        }
    }

    #region Ground Check
    private void RaycastToGround()
    {
        Vector3 _rayDir = _rb.transform.TransformDirection(_downDir);

        Ray rayToGround = new Ray(_rb.position, _rayDir);
        _rayHitGround = Physics.Raycast(rayToGround, out _rayHit, _raycastToGroundLength, _groundLayer);

        if (_rayHitGround)
        {
            if (_rayHit.distance <= _rideHeight * 1.3f) // 1.3f? multiplied because object will oscilate but 1.3 is random
            {
                _isGrounded = true;
            }
            _timeSinceUngrounded = 0;
            _currentDistanceFromGround = _rayHit.distance;
        }
        else
        {
            _isGrounded = false;
            _timeSinceUngrounded += Time.fixedDeltaTime;
            //_currentDistanceFromGround = 0;
        }
    }
    #endregion

    #region Maintain Height
    private void MaintainHeight()
    {
        if (_rayHitGround)
        {
            Vector3 vel = _rb.linearVelocity;
            Vector3 rayDir = -_rb.transform.up;

            Vector3 othervel = Vector3.zero;
            Rigidbody hitBody = _hitBody; //can be a local var for optimization?
            if (hitBody != null)
            {
                othervel = hitBody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, othervel);

            float relVel = rayDirVel - otherDirVel;

            float mass = _rb.mass;
            float rideSpringDamper = 2f * Mathf.Sqrt(_rideSpringStrength * mass) * _rideSpringDampingRatio; //from zeta formula 

            float _distanceFromRideHeight = _currentDistanceFromGround - _rideHeight;

            float springForce = (_distanceFromRideHeight * _rideSpringStrength) - (relVel * rideSpringDamper);

            _rb.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
            }
        }
    }
    #endregion

    #region MaintainUpright
    private void MaintainUpright(Vector3 lookDir)
    {

        if (lookDir != Vector3.zero)
        {
            _uprightTargetRot = Quaternion.LookRotation(lookDir, Vector3.up);
        }

        Quaternion currentRot = _rb.rotation;
        Quaternion toGoal = MathUtils.ShortestRotation(_uprightTargetRot, currentRot);

        Vector3 rotAxis;
        float rotDegrees;

        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);

        // Avoid identity rotation issues
        //if (rotDegrees < 0.001f || float.IsNaN(rotDegrees) || rotAxis == Vector3.zero) return;

        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;
        //Debug.LogError($"toGoal: {toGoal}, rotDegrees: {rotDegrees}, rotAxis: {rotAxis}, rotRadians: {rotRadians}");

        Vector3 torque = rotAxis * (rotRadians * _uprightSpringStrength) - (_rb.angularVelocity * _uprightSpringDamper);

        // Optional final NaN check (for full bulletproofing)
        //if (float.IsNaN(torque.x) || float.IsNaN(torque.y) || float.IsNaN(torque.z)) return;

        _rb.AddTorque(torque);
    }

    private Vector3 GetStableLookDirection()
    {
        Vector3 flatVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVelocity.sqrMagnitude > 0.1f)
            return flatVelocity.normalized;

        return Vector3.zero;
    }

    private void AdjustSpringValuesToMass()
    {
        _rideSpringStrength = _rideSpringStrength * _rb.mass;
        _uprightSpringStrength = _uprightSpringStrength * _rb.mass;
        _uprightSpringDamper = _uprightSpringDamper * _rb.mass;
    }
    #endregion

    public void SetMaintainHeight(bool value)
    {
        _shouldMaintainHeight = value;
    }

    public void ChangeRideHeight(float RideHeightChange)
    {
        //TODO: check if after change ride height makes sense
        _rideHeight += RideHeightChange;
    }

    public void ResetRideHeight()
    {
        _rideHeight = _defaultRideHeight;
    }

    public void OnParasitePossession()
    {
        _isActive = false;
    }

    public void OnParasiteUnPossession()
    {
        _isActive = true;
    }

    private void DrawGroundRay()
    {
        DebugUtils.DrawLine(transform.position, transform.position + Vector3.down * _raycastToGroundLength, _debugRayThickness, Color.red);
    }
}
