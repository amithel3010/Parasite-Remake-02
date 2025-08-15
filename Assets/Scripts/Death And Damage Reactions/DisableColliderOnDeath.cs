using UnityEngine;

public class DisableColliderOnDeath : MonoBehaviour, IDeathResponse
{
    [SerializeField] private Collider _colliderToDisable;
    public void OnDeath()
    {
        _colliderToDisable.isTrigger = true;
    }
}
