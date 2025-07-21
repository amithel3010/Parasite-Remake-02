using UnityEngine;


[RequireComponent(typeof(Collider))]
public class CompoundTriggerChild : MonoBehaviour
{
    //TODO: make this more generic

    private CompoundTrigger _compoundTrigger;
    private Collider _thisTrigger;

    private void Awake()
    {
        _compoundTrigger = GetComponentInParent<CompoundTrigger>();
        _thisTrigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerUtils.PlayerControlledLayer)
            _compoundTrigger.ObjectEntered(_thisTrigger, other);
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.layer == LayerUtils.PlayerControlledLayer)
            _compoundTrigger.ObjectExited(_thisTrigger, other);
    }
}
