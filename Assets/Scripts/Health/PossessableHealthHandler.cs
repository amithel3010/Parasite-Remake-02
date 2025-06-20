using UnityEngine;

public class PossessableHealthHandler : MonoBehaviour
{
    private IDamagable _health;

    void Awake()
    {
        _health = GetComponent<IDamagable>();

        _health.OnDamaged += HandleDamage;
        _health.OnDeath += HandleDeath;
    }

    private void HandleDamage()
    {
        Debug.Log(this + "possessable got hit, it's current health is now" + _health.CurrentHealth);
        //play animation
    }

    private void HandleDeath()
    {
        Debug.Log(this + "has died");
        Destroy(gameObject, 1f);
        //animation
    }
}
