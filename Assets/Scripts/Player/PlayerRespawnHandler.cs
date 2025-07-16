using UnityEngine;

public class PlayerRespawnHandler : MonoBehaviour
{
    private IPlayerRespawnListener[] _listeners;

    void Awake()
    {
        _listeners = GetComponents<IPlayerRespawnListener>(); //does this initialize the array?
    }

    public void OnRespawn()
    {
        foreach(var listener in _listeners )
        {
            listener.OnPlayerRespawn();
        }
    }    
}
