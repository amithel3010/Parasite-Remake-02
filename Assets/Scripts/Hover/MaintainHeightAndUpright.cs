using UnityEngine;

public class MaintainHeightAndUpright : MonoBehaviour
{

    //trying to seperate movement and hovering logic


    [SerializeField] private Vector3 DownDir = Vector3.down; //I have no idea what this is used for

    private Rigidbody _RB;
    private Vector3 _previousVelocity = Vector3.zero; //I Would have never thought of this
    private bool isGrounded;

    [Header("Height Spring")]
    [SerializeField][Tooltip("Needs to be lower than raycastToGroundLength")] float rideHeight = 1.75f;
    [SerializeField, Range(1, 0)] float springDampingRatio = 0.5f;
    [SerializeField] float rideSpringStrength;
    [SerializeField] float raycastToGroundLength = 3f;

    private enum lookDirectionOptions { velocity, acceleration, moveInput }
    private Quaternion _uprightTargetRot = Quaternion.identity;

    [Header("Upright Spring")]
    [SerializeField] private lookDirectionOptions _charcterLookDirection = lookDirectionOptions.velocity;
    [SerializeField] private float _uprightSpringDamper = 5f;
    [SerializeField] private float _uprightSpringStrength = 40f;

    [Header("Other")]
    [SerializeField] LayerMask groundLayer;

    void Awake()
    {
        _RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        (bool isRayHittingGround, RaycastHit groundRayHitInfo) = RaycastToGround();
        isGrounded = CheckIfGrounded(isRayHittingGround, groundRayHitInfo);

        if (isRayHittingGround)
        {
            MaintainHeight(groundRayHitInfo);
        }
        MaintainUpright();
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

    private void MaintainUpright()
    {

        _uprightTargetRot = Quaternion.LookRotation(transform.forward, Vector3.up);


        Quaternion currentRot = transform.rotation;
        Quaternion toGoal = MathUtils.ShortestRotation(_uprightTargetRot, currentRot);

        Vector3 rotAxis;
        float rotDegrees;

        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        _RB.AddTorque(rotAxis * (rotRadians * _uprightSpringStrength) - (_RB.angularVelocity * _uprightSpringDamper));

    }
}
