using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public bool MarkedForDeactivation { get; private set; } = false;

    public void SetMarkedForDeactivation(bool markForDeactivation)
    {
        MarkedForDeactivation = markForDeactivation;
    }
}
