using UnityEngine;

public class MaintainHeightAndUpright
{
    //Height
    private readonly Rigidbody _rb;
    private readonly float _rideHeight = 1.5f;
    private readonly float _springDampingRatio = 0.5f;
    private readonly float _rideSpringStrength = 1000f;

    private readonly Vector3 DownDir = Vector3.down;

    //Upright
    private readonly float _uprightSpringStrength = 800f;
    private readonly float _uprightSpringDamper = 25f;

    private Quaternion _uprightTargetRot = Quaternion.identity;

    public MaintainHeightAndUpright(Rigidbody rb, HoverSettings settings)
    {
        _rb = rb;
        _rideHeight = settings.RideHeight; //TODO: ride height should be part of Ground Checker
        _springDampingRatio = settings.RideSpringDampingRatio;
        _rideSpringStrength = settings.RideSpringStrength;

        _uprightSpringStrength = settings.UprightSpringStrength;
        _uprightSpringDamper = settings.UprightSpringDamper;
    }

    public void Tick(Vector3 lookDir, bool ShouldMaintainHeight, GroundChecker groundChecker)
    {
        if (ShouldMaintainHeight)
        {
            MaintainHeight(groundChecker);
        }
        MaintainUpright(lookDir);
    }

    private void MaintainHeight(GroundChecker groundChecker)
    {
       // bool rayDidHit = Physics.Raycast(_rb.position, Vector3.down, out RaycastHit rayHit, _raycastToGroundLength);

        //Debug.DrawLine(_rb.position, rayHit.point, Color.green); // actual ray hit
        //Debug.DrawRay(rayHit.point, Vector3.up * _rideHeight, Color.yellow); // target ride height
        if (groundChecker.RayHitGround)
        {
            Vector3 vel = _rb.linearVelocity;
            Vector3 rayDir = -_rb.transform.up;

            Vector3 othervel = Vector3.zero;
            Rigidbody hitBody = groundChecker.hitBody;
            if (hitBody != null)
            {
                othervel = hitBody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, othervel); //what is dot and how is it used here?

            float relVel = rayDirVel - otherDirVel;

            float mass = _rb.mass;
            float rideSpringDamper = 2f * Mathf.Sqrt(_rideSpringStrength * mass) * _springDampingRatio; //from zeta formula 

            float  _distanceFromRideHeight = groundChecker.CurrentDistanceFromGround - _rideHeight;
            
            float springForce = (_distanceFromRideHeight * _rideSpringStrength) - (relVel * rideSpringDamper);

            _rb.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, groundChecker._rayHit.point);
            }
        }
    }

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
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        _rb.AddTorque(rotAxis * (rotRadians * _uprightSpringStrength) - (_rb.angularVelocity * _uprightSpringDamper));
    }
}
