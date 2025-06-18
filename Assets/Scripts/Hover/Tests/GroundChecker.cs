using UnityEngine;

public class GroundChecker
{
    private readonly Rigidbody _rb;

    private bool _rayHitGround;
    private bool _isGrounded;
    private float _currentDistanceFromGround;
    private float _timeSinceUngrounded;

    private Rigidbody _hitBody;

    private float _rideHeight;

    private readonly LayerMask _groundLayer;
    private float _raycastToGroundLength;

    private Vector3 _downDir = Vector3.down;

    public bool RayHitGround => _rayHitGround;
    public bool IsGrounded => _isGrounded;
    public float CurrentDistanceFromGround => _currentDistanceFromGround;
    public float TimeSinceUngrounded => _timeSinceUngrounded;
    public Rigidbody hitBody => _hitBody;

    public RaycastHit _rayHit;

    public GroundChecker(Rigidbody rb, GroundCheckerSettings settings)
    {
        _rb = rb;

        _groundLayer = settings.GroundLayer;
        _raycastToGroundLength = settings.RaycastToGroundLength;
        _downDir = settings.DownDir;
    }

    public GroundChecker(Rigidbody rb, GroundCheckerSettings settings, HoverSettings optionalHoverSettings)
    {
        _rb = rb;

        _groundLayer = settings.GroundLayer;
        _downDir = settings.DownDir;

        _raycastToGroundLength = optionalHoverSettings.RaycastToGroundLength;
        _rideHeight = optionalHoverSettings.RideHeight;
    }

    public void Tick()
    {
        RaycastToGround();
    }

    private void RaycastToGround()
    {
        Vector3 _rayDir = -_rb.transform.up;

        Ray rayToGround = new Ray(_rb.position, _rayDir);
        _rayHitGround = Physics.Raycast(rayToGround, out RaycastHit _rayHit, _raycastToGroundLength, _groundLayer.value);

        if (_rayHitGround)
        {
            _isGrounded = _rayHit.distance <= _rideHeight * 1.3f; // 1.3f? multiplied because object will oscilate but 1.3 is random
            _isGrounded = true;
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
}
