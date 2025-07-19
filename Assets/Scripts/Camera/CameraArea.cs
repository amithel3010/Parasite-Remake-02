using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraArea : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private CompoundTrigger _compoundTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
