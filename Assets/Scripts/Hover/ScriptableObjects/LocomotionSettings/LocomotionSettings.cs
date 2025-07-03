using UnityEngine;

[CreateAssetMenu(fileName = "LocomotionSettings", menuName = "Scriptable Objects/LocomotionSettings")]
public class LocomotionSettings : ScriptableObject
{
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

    public bool IsFlying = false;
}
