using UnityEngine;

public class HeartGas : MonoBehaviour
{
    private TriggerChanneler _trigger;
    
    private ParticleSystem _heartGasParticles;
    private BoxCollider _collider;

    private void Awake()
    {
        _trigger = GetComponentInChildren<TriggerChanneler>();
        
        
        //Must be a better way
        _collider = GetComponentInChildren<BoxCollider>();
        _heartGasParticles = GetComponentInChildren<ParticleSystem>();

        if (_collider != null && _heartGasParticles != null)
        {
            var shape = _heartGasParticles.shape;
            shape.scale = _collider.size;
        }
        
        if (_trigger == null)
        {
            Debug.LogError("No Channeler In Child");
        }
    }

    private void OnEnable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent += HandleTriggerEnter;
        }
    }

    private void OnDisable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent -= HandleTriggerEnter;
        }
    }

    private void HandleTriggerEnter(Collider other)
    {
        Transform target = other.transform.parent;

        if (!target.TryGetComponent(out Possessable _)) return;

        if (target.TryGetComponent(out Health health))
        {
            health.ChangeHealth(-health.MaxHealth);
            Debug.Log("Heart Gas Killed" + target.name);
        }
    }
}