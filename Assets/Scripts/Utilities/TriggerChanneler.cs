using System;
using UnityEngine;
    
public class TriggerChanneler : MonoBehaviour
{
    /// <summary>
    /// Used to channel a trigger from a child to it's parent.
    /// </summary>

    //TODO: requires subscribing on awake and kinda clutters the code
    
    public event Action<Collider> OnTriggerEnterEvent;
    public event Action<Collider> OnTriggerStayEvent;
    public event Action<Collider> OnTriggerExitEvent;

    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerStayEvent?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }
}
