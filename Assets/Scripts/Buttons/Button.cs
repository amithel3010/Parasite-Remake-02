using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Button : MonoBehaviour
{
    //for now this is a purely functional script
    //just for grey box testing and level design

    //Should be able to be pressed by brute standing on it, or a wooden box on it
    //on press, raise a unity event

    //private CanPushButtons _pusher;
    private bool _isPushed = false;

    private TriggerChanneler _trigger;

    [FormerlySerializedAs("OnButtonPress")] [SerializeField] private UnityEvent _onButtonPress;
    [FormerlySerializedAs("OnButtonUp")] [SerializeField] private UnityEvent _onButtonUp;

    private readonly List<CanPushButtons> _pushers = new List<CanPushButtons>(); //what does it mean that this is readonly?

    private void Awake()
    {
        _trigger = GetComponentInChildren<TriggerChanneler>();
    }

    private void OnEnable()
    {
        if (_trigger == null) return;
        
        _trigger.OnTriggerEnterEvent += HandleTriggerEnter;
        _trigger.OnTriggerExitEvent += HandleTriggerExit;
    }

    private void OnDisable()
    {
        if (_trigger == null) return;
        
        _trigger.OnTriggerEnterEvent -= HandleTriggerEnter;
        _trigger.OnTriggerExitEvent -= HandleTriggerExit;
    }

    private void HandleTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out CanPushButtons pusher))
        {
            if (!_pushers.Contains(pusher))
            {
                _pushers.Add(pusher);
                if (_pushers.Count == 1)
                {
                    _onButtonPress?.Invoke();
                    Debug.Log("Button Pushed");
                }
            }
        }
    }

    private void HandleTriggerExit(Collider other)
    {
        if (!other.transform.parent.TryGetComponent(out CanPushButtons pusher)) return;
        if (!_pushers.Remove(pusher) || _pushers.Count != 0) return;
        
        _onButtonUp?.Invoke();
    }

    private void Update()
    {
        //TODO: probably not great that this is in update

        // Remove any pushers that were destroyed or deactivated
        _pushers.RemoveAll(p => p == null || !p.gameObject.activeInHierarchy);

        if (_isPushed && _pushers.Count == 0)
        {
            _isPushed = false;
            _onButtonUp?.Invoke();
            Debug.Log("Button Released (via update cleanup)");
        }

        if (!_isPushed && _pushers.Count > 0)
        {
            _isPushed = true;
            _onButtonPress?.Invoke();
            Debug.Log("Button Pushed (via update cleanup)");
        }
    }
}
