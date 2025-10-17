using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PossessableHealthHandler : MonoBehaviour, IPossessionSensitive
{
    private Health _health;
    private AudioSource _audioSource;
    private IDamageResponse[] _damageResponsers;
    private IDeathResponse[] _deathResponsers;
    
    [Header("Vfx")]
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private float _particleOffset;

    [Header("Sfx")]
    [SerializeField] private AudioClip _sfxDamaged;
    [SerializeField] private AudioClip _sfxDeath;
    
    void Awake()
    {
        _deathResponsers = GetComponents<IDeathResponse>();
        _damageResponsers = GetComponents<IDamageResponse>();
        _health = GetComponent<Health>();
        if (!TryGetComponent(out _audioSource))
        {
            Debug.Log($"no  audio source found on {name}");
        }
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
        
        //play sfx
        if (_audioSource != null && _sfxDamaged != null)
        {
            _audioSource.pitch = Random.Range(0.6f, 1.4f);
            _audioSource.PlayOneShot(_sfxDamaged);
        }
    }

    private void HandleDeath()
    {
        Debug.Log(this + "has died");
        //animation
        foreach (var deathResponse in _deathResponsers)
        {
            deathResponse.OnDeath();
        }
        if (_deathParticles != null)
        {
            Instantiate(_deathParticles, transform.position + Vector3.up *_particleOffset, Quaternion.identity);
        }
        if (_audioSource != null && _sfxDeath != null)
        {
            _audioSource.pitch = Random.Range(0.6f, 1.4f);
            _audioSource.PlayOneShot(_sfxDeath);
        }
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
