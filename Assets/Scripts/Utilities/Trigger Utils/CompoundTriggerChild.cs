using UnityEngine;


[RequireComponent(typeof(Collider))]
public class CompoundTriggerChild : MonoBehaviour
{
    //TODO: Add Debug Visuals

    private CompoundTrigger _compoundTrigger;
    private Collider _thisTrigger;

    private bool _shouldDrawDebugGfx;

    private void Awake()
    {
        _compoundTrigger = GetComponentInParent<CompoundTrigger>();
        _thisTrigger = GetComponent<Collider>();

        if (_thisTrigger.isTrigger == false)
        {
            Debug.LogWarning(this + "Is on a collider that isn't a trigger. setting it as trigger.");
            _thisTrigger.isTrigger = true;
        }

        if (_compoundTrigger == null)
        {
            Debug.LogError(this + "Doesn't have a parent with a compound trigger script!");
        }
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

    public void SetDebugGfxActive(bool active)
    {
        _shouldDrawDebugGfx = active;
    }
}
