using UnityEngine;

public class PlayerRespawnHandler : MonoBehaviour
{
    private Knockback _knockback;
    private Health _health;

    void Awake()
    {
        _knockback = GetComponent<Knockback>();
        _health = GetComponent<Health>();
    }

    public void OnRespawn()
    {
        //TODO: maybe an on respawn event? listeners?
        _knockback.KnockbackEnabled = true;
        _health.ResetHealth();
    }    
}
