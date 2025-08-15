using System;
using UnityEngine;

public class HeartGas : MonoBehaviour
{
    [SerializeField] private Vector3 _size = Vector3.one;
    
    [Header("References")]
    [SerializeField] private ParticleSystem _heartGasParticles;
    [SerializeField] private Transform _triggerObject;
    [SerializeField] private GameObject _colliderVisualizer;
    
    [Header("Debug")]
    [SerializeField] private bool _showCollider = false;
    
    //refs
    private TriggerChanneler _trigger;
    private BoxCollider _collider;
    

    private void Awake()
    {
        if (_triggerObject != null)
        {
            _trigger = _triggerObject.GetComponent<TriggerChanneler>();
            _collider = _triggerObject.GetComponent<BoxCollider>();
        }
        
        if (_collider != null)
        {
            _collider.size = _size;
            _collider.center = Vector3.zero;
            _collider.isTrigger = true;
        }

        if (_heartGasParticles != null)
        {
            var shape = _heartGasParticles.shape;
            shape.scale = _size;
        }
        
        if (_colliderVisualizer != null)
        {
            _colliderVisualizer.transform.localScale = _size;
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

    private void OnValidate()
    {
        if (_collider == null && _triggerObject != null)
        {
            _collider = _triggerObject.GetComponent<BoxCollider>();
        }
        
        if (_collider != null)
            _collider.size = _size;
        
        if (_heartGasParticles != null)
        {
            var shape = _heartGasParticles.shape;
            shape.scale = _size;
        }
        
        if (_colliderVisualizer != null)
        {
            _colliderVisualizer.transform.localScale = _size;
            _colliderVisualizer.SetActive(_showCollider);
        }
    }
}