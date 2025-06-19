using UnityEngine;

[CreateAssetMenu(fileName = "GroundCheckerSettings", menuName = "Scriptable Objects/GroundCheckerSettings")]
public class GroundCheckerSettings : ScriptableObject
{
    public  LayerMask GroundLayer;

    public float RaycastToGroundLength;

    public Vector3 DownDir = Vector3.down;
}
