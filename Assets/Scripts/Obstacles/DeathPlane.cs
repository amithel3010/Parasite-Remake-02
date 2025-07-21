using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool _showCollider; //TODO: not implemented


    //TODO: add debug mode to see colliders
    private Renderer _renderer;
    private TriggerChanneler _trigger;

    void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
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
        Debug.Log(other.name);
        if (other.transform.parent.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.Killimmediately();
        }
    }

}
