using UnityEngine;

[CreateAssetMenu(fileName = "HoverSettings", menuName = "Scriptable Objects/HoverSettings")]
public class HoverSettings : ScriptableObject
{
    [Header("Height Spring Settings")]
    public float RideHeight = 1.5f;
    public float RaycastToGroundLength = 2f;
    public float RideSpringStrength = 1000f;
    [Range(0, 1)] public float RideSpringDampingRatio = 0.5f;

    [Header("Upright Spring Settings")]
    public float UprightSpringStrength = 800f;
    public float UprightSpringDamper = 25f;

    void OnValidate()
    {
        if (RaycastToGroundLength < RideHeight)
        {
            Debug.Log("RaycastToGroundLength should not be shorter than RideHeight");
            RaycastToGroundLength = RideHeight;
        }
    }
}
