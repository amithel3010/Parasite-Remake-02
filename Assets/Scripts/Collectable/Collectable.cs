using UnityEngine;

public class Collectable : MonoBehaviour
{
    //should be able to be collected by parasite,
    //or parasite controlled possessable.
    //also, total amount in scene should be tracked somewhere
    //and collected amount

    private TriggerChanneler _trigger;

    void Awake()
    {
        _trigger = GetComponentInChildren<TriggerChanneler>();
    }

    void OnEnable()
    {
        if (CollectableManager.Instance != null)
        {
            CollectableManager.Instance.InitCollectable(this);

            if (_trigger != null)
            {
                _trigger.OnTriggerEnterEvent += HandleTriggerEnter;
            }
        }
        else
        {
            Debug.LogError("Collectable manager instance is null when trying to init collectable");
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
        if (other.gameObject.layer != LayerUtils.PlayerControlledLayer) return; //Only checks for player
        
        other.transform.parent.TryGetComponent<ICollector>(out var collector);
        collector?.Collect(this);
    }
}
