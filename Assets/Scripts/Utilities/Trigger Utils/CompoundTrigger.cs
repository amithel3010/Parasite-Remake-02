using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CompoundTrigger : MonoBehaviour
{
    //Credit to: Ruchir on unity forums


    [SerializeField] private LayerMask _layerMask;

    /// <summary>
    /// Tells if player is inside the Trigger Area
    /// </summary>
    [SerializeField] private bool _isInside; //for seeing in inspector

    private Dictionary<Collider, bool> _triggers;

    [FormerlySerializedAs("OnEnter")] public UnityEvent _onEnter;
    [FormerlySerializedAs("OnExit")] public UnityEvent _onExit;
    
    //TODO: confusing naming
    public event Action onEnter;
    public event Action onExit;

    private void Awake()
    {
        _triggers = new Dictionary<Collider, bool>();
    }

    /// <summary>
    /// Call this function when player enters the area
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="other"></param>
    public void ObjectEntered(Collider sender, Collider other)
    {
        if (Filter(other.gameObject))
        {
            _triggers[sender] = true;
            CheckInside();
        }
    }

    /// <summary>
    /// Call this function when player leaves the area
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="other"></param>
    public void ObjectExited(Collider sender, Collider other)
    {
        if (Filter(other.gameObject))
        {
            _triggers[sender] = false;
            CheckInside();
        }
    }

    /// <summary>
    /// Checks whether the player is inside and updates isInside
    /// </summary>
    private void CheckInside()
    {
        bool lastState = _isInside;

        _isInside = false;
        foreach (var trigger in _triggers)
        {
            if (trigger.Value)
            {
                _isInside = true;
                break;
            }
        }
        if (lastState != _isInside)
        {
            if (_isInside)
            {
                _onEnter?.Invoke();
                onEnter?.Invoke();
            }
            else
            {
                _onExit?.Invoke();
                onExit?.Invoke();
            }
        }
    }

    private bool Filter(GameObject other)
    {
        //Debug.Log($"Checking layer: {other.layer} ({LayerMask.LayerToName(other.layer)})");
        //Debug.Log($"LayerMask value: {_layerMask.value} | Bit check result: {(1 << other.layer) & _layerMask.value}");

        if (!enabled)
            return false;
        if (((1 << other.layer) & _layerMask) == 0)
            return false;

        return true;
    }
}