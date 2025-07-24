using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool _showCollider; //TODO: not implemented


    //TODO: add debug mode to see colliders
    //private Renderer _renderer;
    private TriggerChanneler _trigger;

    private void Awake()
    {
        //_renderer = GetComponentInChildren<Renderer>();
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
        Debug.Log(other.name);
        if (other.transform.parent.gameObject.TryGetComponent(out Health health))
        {
            health.KillImmediately();
        }
    }

}
