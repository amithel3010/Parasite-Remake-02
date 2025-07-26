using UnityEngine;

public class HeartGas : MonoBehaviour
{
    private TriggerChanneler _trigger;

    private void Awake()
    {
        _trigger = GetComponentInChildren<TriggerChanneler>();

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