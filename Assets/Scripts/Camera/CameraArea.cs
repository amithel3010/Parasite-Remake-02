using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CompoundTrigger))]
public class CameraArea : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private CompoundTrigger _compoundTrigger;

    private void Awake()
    {
        _compoundTrigger = GetComponent<CompoundTrigger>();
    }

    private bool Filter(GameObject other)
    {
        if (!enabled)
            return false;
        if (((1 << other.layer) & _layerMask) == 0)
            return false;

        return true;
    }


}
