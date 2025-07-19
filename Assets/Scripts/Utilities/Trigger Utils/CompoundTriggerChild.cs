using UnityEngine;


[RequireComponent(typeof(Collider))]
public class CompoundTriggerChild : MonoBehaviour
{
    private CompoundTrigger _compoundTrigger;
    private Collider _thisTrigger;

    private void Awake()
    {
        _compoundTrigger = GetComponentInParent<CompoundTrigger>();
        _thisTrigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerUtils.PlayerControlledLayer)
            _compoundTrigger.PlayerEntered(_thisTrigger);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerUtils.PlayerControlledLayer)
            _compoundTrigger.PlayerExit(_thisTrigger);
    }
}
