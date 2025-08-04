using UnityEngine;

[RequireComponent(typeof(Health))]
public class DieOnResourceDepletion : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _resourceComponent;

    private IResource _resource;
    private Health _health;

    void Awake()
    {
        _resource = _resourceComponent as IResource;
        _health = GetComponent<Health>();

        if (_resource == null || _health == null)
        {
            Debug.LogWarning("KillOnDepletion requires both IResource and Health components.");
            enabled = false;
        }
    }

    void Update()
    {
        if (_resource.CurrentValue <= 0)
        {
            _health.KillImmediately();
        }
    }
}