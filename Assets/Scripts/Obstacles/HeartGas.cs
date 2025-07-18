using UnityEngine;
using UnityEngine.UIElements;

public class HeartGas : MonoBehaviour
{
    private TriggerChanneler _trigger;

    void Awake()
    {
        _trigger = GetComponentInChildren<TriggerChanneler>();

        if (_trigger == null)
        {
            Debug.LogError("No Channeler In Child");
        }
    }

    void OnEnable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent += HandleTriggerEnter;
        }
    }

    void OnDisable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent -= HandleTriggerEnter;
        }
    }

    public void HandleTriggerEnter(Collider other)
    {
        Transform target = other.transform.parent;
        if (target.TryGetComponent<Possessable>(out Possessable possessable))
        {
            if(target.TryGetComponent<Health>(out Health health))
            {
                health.ChangeHealth(-health.MaxHealth);
                Debug.Log("Heart Gas Killed" + target.name);
            }
        }


    }
}
