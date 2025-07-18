using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    //for now this is a purely functional script
    //just for grey box testing and level design

    //Should be able to be pressed by brute standing on it, or a wooden box on it
    //on press, raise a unity event

    //private CanPushButtons _pusher;
    private bool _isPushed = false;

    private TriggerChanneler _trigger;

    [SerializeField] private UnityEvent OnButtonPress;
    [SerializeField] private UnityEvent OnButtonUp;

    private List<CanPushButtons> _pushers = new List<CanPushButtons>();

    void Awake()
    {
        _trigger = GetComponentInChildren<TriggerChanneler>();
    }

    void OnEnable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent += HandleTriggerEnter;
            _trigger.OnTriggerExitEvent += HandleTriggerExit;
        }
    }

    void OnDisable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent -= HandleTriggerEnter;
            _trigger.OnTriggerExitEvent -= HandleTriggerExit;
        }
    }

    public void HandleTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out CanPushButtons pusher))
        {
            if (!_pushers.Contains(pusher))
            {
                _pushers.Add(pusher);
                if (_pushers.Count == 1)
                {
                    OnButtonPress?.Invoke();
                    Debug.Log("Button Pushed");
                }
            }
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out CanPushButtons pusher))
        {
            if (_pushers.Remove(pusher) && _pushers.Count == 0)
            {
                OnButtonUp?.Invoke();
                Debug.Log("Button Released");
            }
        }
    }

    private void Update()
    {
        //TODO: probably not great that this is in update

        // Remove any pushers that were destroyed or deactivated
        _pushers.RemoveAll(p => p == null || !p.gameObject.activeInHierarchy);

        if (_isPushed && _pushers.Count == 0)
        {
            _isPushed = false;
            OnButtonUp?.Invoke();
            Debug.Log("Button Released (via update cleanup)");
        }

        if (!_isPushed && _pushers.Count > 0)
        {
            _isPushed = true;
            OnButtonPress?.Invoke();
            Debug.Log("Button Pushed (via update cleanup)");
        }
    }
}
