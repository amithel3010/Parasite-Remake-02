using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompoundTrigger : MonoBehaviour
{
    //Credit to: Ruchir on unity forums


    [SerializeField] private LayerMask _layerMask;

    /// <summary>
    /// Tells if player is inside the Trigger Area
    /// </summary>
    [SerializeField] private bool _isInside; //for seeing in inspector

    private Dictionary<Collider, bool> triggers;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private void Awake()
    {
        triggers = new Dictionary<Collider, bool>();
    }

    /// <summary>
    /// Call this function when player enters the area
    /// </summary>
    /// <param name="sender"></param>
    public void ObjectEntered(Collider sender, Collider other)
    {
        if (Filter(other.gameObject))
        {
            triggers[sender] = true;
            CheckInside();
        }
    }

    /// <summary>
    /// Call this function when player leaves the area
    /// </summary>
    /// <param name="sender"></param>
    public void ObjectExited(Collider sender, Collider other)
    {
        if (Filter(other.gameObject))
        {
            triggers[sender] = false;
            CheckInside();
        }
    }

    /// <summary>
    /// Checks whether the player is inside and updates isInside
    /// </summary>
    private void CheckInside()
    {
        bool _lastState = _isInside;

        _isInside = false;
        foreach (var trigger in triggers)
        {
            if (trigger.Value)
            {
                _isInside = true;
                break;
            }
        }
        if (_lastState != _isInside)
        {
            if (_isInside)
            {
                Debug.Log("entered");
                OnEnter?.Invoke();
            }
            else
            {
                Debug.Log("exited");

                OnExit?.Invoke();
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