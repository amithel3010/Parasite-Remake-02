using UnityEngine;

public class Cannon : MonoBehaviour
{
    //cannons are used to send the player up vertically.
    // should have an end point, maybe sends it there using animations?

    //TODO: for now it is just a teleporter

    [SerializeField] private Transform _endPoint;

    private TriggerEventHandler _trigger;

    void Awake()
    {
        _trigger = GetComponentInChildren<TriggerEventHandler>();
    }

    void OnEnable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent += OnChildTriggerEnter;
            _trigger.OnTriggerExitEvent += OnChildTriggerExit;
        }
    }

    void OnDisable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent -= OnChildTriggerEnter;
            _trigger.OnTriggerExitEvent -= OnChildTriggerExit;
        }
    }

    private void OnChildTriggerEnter(Collider other)
    {
        if(other.transform.parent.TryGetComponent<Parasite>(out var parasite))
        {
            parasite.RespawnAt(_endPoint.position);
        }
    }

    private void OnChildTriggerExit(Collider other)
    {

    }
}
