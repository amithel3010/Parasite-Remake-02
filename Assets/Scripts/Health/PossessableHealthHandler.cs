using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PossessableHealthHandler : MonoBehaviour, IPossessionSensitive
{
    private Health _health;
    private IDamageResponse[] _damageResponsers;
    private IDeathResponse[] _deathResponsers;
    
    [Header("Vfx")]
    [SerializeField] private ParticleSystem _deathParticles;

    void Awake()
    {
        _deathResponsers = GetComponents<IDeathResponse>();
        _damageResponsers = GetComponents<IDamageResponse>();
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

    private void HandleDamage(float iFramesDuration)
    {
        Debug.Log(this + "possessable got hit, it's current health is now" + _health.CurrentHealth);
        foreach (var damageResponse in _damageResponsers)
        {
            damageResponse.OnDamage(iFramesDuration);
        }
        //play animation
    }

    private void HandleDeath()
    {
        Debug.Log(this + "has died");
        //animation
        foreach (var deathResponse in _deathResponsers)
        {
            deathResponse.OnDeath();
        }
        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject, 5f);
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _health.ResetHealth();
        //TODO: unclear code!!! baiscally means on death, exit possessable. sounds like a death response to me. but how do i get a reference to parasite?
        _health.OnDeath += playerParasite.ExitPossessable;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _health.OnDeath -= playerParasite.ExitPossessable;
    }
    
}
