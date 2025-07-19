using System;
using System.Collections.Generic;
using UnityEngine;

public class CompoundTrigger : MonoBehaviour
{
    //Credit to: Ruchir on unity forums


    /// <summary>
    /// Tells if player is inside the Trigger Area
    /// </summary>
    [SerializeField] private bool _isInside; //for seeing in inspector

    private Dictionary<Collider, bool> triggers;

    public event Action OnEnter;
    public event Action OnExit;

    private void Awake()
    {
        triggers = new Dictionary<Collider, bool>();
    }

    /// <summary>
    /// Call this function when player enters the area
    /// </summary>
    /// <param name="sender"></param>
    public void PlayerEntered(Collider sender)
    {
        triggers[sender] = true;
        CheckInside();
    }

    /// <summary>
    /// Call this function when player leaves the area
    /// </summary>
    /// <param name="sender"></param>
    public void PlayerExit(Collider sender)
    {
        triggers[sender] = false;
        CheckInside();
    }

    /// <summary>
    /// Checks whether the player is inside and updates isInside
    /// </summary>
    private void CheckInside()
    {
        //TODO: need to setup events properly
        _isInside = false;
        foreach (var trigger in triggers)
        {
            if (trigger.Value)
            {
                _isInside = true;
                Debug.Log("OnEnter");
                break;
            }
        }
        if (!_isInside)
            Debug.Log("OnExit");
    }
}