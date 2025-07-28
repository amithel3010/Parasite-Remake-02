using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CompoundTrigger))]
public class CameraArea : MonoBehaviour
{
    //TODO: make this script a simple script for camera areas that automatically calls for activate camera and mark for de activation, so it requires almost no setup in inspector.
    [SerializeField] private CameraHolder _cameraForArea;
    [SerializeField] private LayerMask _layerMask;

    private CompoundTrigger _compoundTrigger;

    private void Awake()
    {
        _compoundTrigger = GetComponent<CompoundTrigger>();
        //_compoundTrigger._onEnter += ActivateCamera;
    }

    private bool Filter(GameObject other)
    {
        if (!enabled)
            return false;
        if (((1 << other.layer) & _layerMask) == 0)
            return false;

        return true;
    }

    private void ActivateCamera()
    {
        
    }

    private void MarkForDeactivation()
    {
        
    }
}
