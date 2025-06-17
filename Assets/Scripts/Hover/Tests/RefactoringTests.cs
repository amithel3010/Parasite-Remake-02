using UnityEngine;

public class RefactoringTests : MonoBehaviour
{
    private IInputSource _inputSource;

    private MaintainHeightWithSpring _hover;
    private MaintainUprightWithSpring _upright;
    private Rigidbody _rb;


    private void Awake()
    {
        _inputSource = GetComponent<IInputSource>();
        _rb = GetComponent<Rigidbody>();
        _hover = new MaintainHeightWithSpring(_rb);
        _upright = new MaintainUprightWithSpring(_rb);
    }

    private void FixedUpdate()
    {
        _hover.Tick();
        _upright.Tick(GetLookDir());
    }

    private Vector3 GetLookDir()
    {
        return new Vector3(_inputSource.MovementInput.x, 0, _inputSource.MovementInput.y).normalized;
    }

    

}

public class MaintainHeightWithSpring
{
    private readonly Rigidbody _rb;
    private readonly float _rideHeight = 1.5f;
    private readonly float _springDampingRatio = 0.5f;
    private readonly float _rideSpringStrength = 1000f;
    private readonly float _raycastToGroundLength = 2f;

    private readonly Vector3 DownDir = Vector3.down;

    public MaintainHeightWithSpring(Rigidbody rb)
    {
        _rb = rb;
    }

    public void Tick()
    {
        MaintainHeight();
    }

    private void MaintainHeight()
    {
        bool rayDidHit = Physics.Raycast(_rb.position, Vector3.down, out RaycastHit rayHit, _raycastToGroundLength);

        //Debug.DrawLine(_rb.position, rayHit.point, Color.green); // actual ray hit
        //Debug.DrawRay(rayHit.point, Vector3.up * _rideHeight, Color.yellow); // target ride height
        if (rayDidHit)
        {
            Vector3 vel = _rb.linearVelocity;
            Vector3 rayDir = _rb.transform.TransformDirection(DownDir); // same as transform.down?
            Debug.DrawRay(_rb.position, rayDir);

            Vector3 othervel = Vector3.zero;
            Rigidbody hitBody = rayHit.rigidbody;
            if (hitBody != null)
            {
                othervel = hitBody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, othervel); //what is dot and how is it used here?

            float relVel = rayDirVel - otherDirVel;

            float mass = _rb.mass;
            float rideSpringDamper = 2f * Mathf.Sqrt(_rideSpringStrength * mass) * _springDampingRatio; //from zeta formula 

            float x = rayHit.distance - _rideHeight;

            float springForce = (x * _rideSpringStrength) - (relVel * rideSpringDamper);

            _rb.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, rayHit.point);
            }
        }
    }
}

public class MaintainUprightWithSpring
{
    private readonly Rigidbody _rb;
    private readonly float _uprightSpringStrength = 800f;
    private readonly float _uprightSpringDamper = 25f;

    public MaintainUprightWithSpring(Rigidbody rb)
    {
        _rb = rb;
    }

    public void Tick(Vector3 lookDir)
    {
        MaintainUpright(lookDir);
    }

    private void MaintainUpright(Vector3 lookDir)
    {
        Quaternion uprightTargetRot = Quaternion.identity; //might be problematic

        if (lookDir != Vector3.zero)
        {
            uprightTargetRot = Quaternion.LookRotation(lookDir, Vector3.up);
        }

        Quaternion currentRot = _rb.rotation;
        Quaternion toGoal = MathUtils.ShortestRotation(uprightTargetRot, currentRot);

        Vector3 rotAxis;
        float rotDegrees;

        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        _rb.AddTorque(rotAxis * (rotRadians * _uprightSpringStrength) - (_rb.angularVelocity * _uprightSpringDamper));
    }

}
