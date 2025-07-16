using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PossessableHealthHandler : MonoBehaviour, IPossessionSensitive
{
    private Health _health;

    void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.OnDamaged += HandleDamage;
        _health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnDamaged -= HandleDamage;
        _health.OnDeath -= HandleDeath;
    }

    private void HandleDamage(float IFramesDuration)
    {
        Debug.Log(this + "possessable got hit, it's current health is now" + _health.CurrentHealth);
        foreach (var damageResponse in GetComponents<IDamageResponse>())
        {
            damageResponse.OnDamage(IFramesDuration);
        }
        //play animation
    }

    private void HandleDeath()
    {
        Debug.Log(this + "has died");
        //animation
        foreach (var deathResponse in GetComponents<IDeathResponse>())
        {
            deathResponse.OnDeath();
        }
        Destroy(gameObject, 3f);
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        //TODO: unclear code!!! baiscally means on death, exit possessable. sounds like a death response to me
        _health.OnDeath += playerParasite.ExitPossessable;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _health.OnDeath -= playerParasite.ExitPossessable;
    }
}
