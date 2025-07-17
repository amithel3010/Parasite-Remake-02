using UnityEngine;

public class Cannon : MonoBehaviour
{
    //cannons are used to send the player up vertically.
    // should have an end point, maybe sends it there using animations?

    //TODO: for now it is just a teleporter

    [SerializeField] private Transform _endPoint;

    [Header("Debug")]
    [SerializeField] private bool _showEndPoint = true;

    private TriggerChanneler _trigger;

    void Awake()
    {
        _trigger = GetComponentInChildren<TriggerChanneler>();
    }

    void OnEnable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent += OnChildTriggerEnter;
        }
    }

    void OnDisable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent -= OnChildTriggerEnter;
        }
    }

    private void OnChildTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Parasite>(out var parasite))
        {
            parasite.TeleportTo(_endPoint.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        //TODO: make it so that it shows only when is selecteed, or children selected
        if (_showEndPoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_endPoint.position, 0.3f);
        }
    }
}
