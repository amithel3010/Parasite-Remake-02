using UnityEngine;

[CreateAssetMenu(fileName = "HoveringCreatureSettings", menuName = "Scriptable Objects/HoveringCreatureSettings")]
public class HoveringCreatureSettings : ScriptableObject
{
    [Header("Ground Check")]
    public Vector3 DownDIr = Vector3.down;
    public LayerMask GroundLayer;
    public float RaycastToGroundLength;

    [Header("Height Spring")]
    [Min(0.1f)] public float RideHeight = 0.93f;
    [Min(0.1f)] public float RideSpringStrength = 1000f;
    [Range(0, 1)] public float RideSpringDampingRatio = 0.5f;

    [Header("Upright Spring")]
    [Min(0.1f)] public float UprightSpringDamper = 25f;
    [Min(0.1f)] public float UprightSpringStrength = 800f;

    [Header("Movement")]
    public float MaxSpeed = 4f;
    public float Acceleration = 25f;
    public float MaxAccelForce = 150f;
    public float LeanFactor = 0.25f;
    public AnimationCurve AccelerationFactorFromDot;
    public AnimationCurve MaxAccelerationForceFactorFromDot;
    public Vector3 MoveForceScale = new Vector3(1f, 0f, 1f);

    [Header("Jumping")]
    public int MaxJumps = 1;
    public float JumpHeight = 5f;
    public float JumpBuffer = 0.2f;
    public float CoyoteTime = 0.2f;

}
