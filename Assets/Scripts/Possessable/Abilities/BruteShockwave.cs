using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class BruteShockwave : MonoBehaviour, IPossessionSensitive
{

    //TODO: this should maybe inherent from jump.

    [Header("General Settings")]
    [SerializeField] private BreakableType _canBreak;
    [SerializeField] private float _damage = 25f;
    [FormerlySerializedAs("_ShockwaveCost")] [SerializeField] private float _shockwaveCost = 5f;

    [Header("Hitbox Settings")]
    [SerializeField] private float _hitboxRadius = 1f;
    [SerializeField] private float _duration = 0.2f;

    [Header("Debug")]
    [SerializeField] private Material _circleDebugMaterial;
    [SerializeField] private bool _showDebugCircleInGame = true;
    
    private bool _isActive;
    private bool _shouldConsumeHealth;
    private float _timer;
    private readonly HashSet<GameObject> _alreadyHit = new();

    //Refs
    private IHasLandedEvent _landingEventRaiser;
    private IResource _resource;

    void Awake()
    {
        _resource = GetComponent<Stamina>(); //Can be changed if needed
        _landingEventRaiser = GetComponent<IHasLandedEvent>();
    }

    void OnEnable()
    {
        _landingEventRaiser.OnLanding += TriggerShockwave; // I don't love this... it triggers even if jumping on something regardless of fall distance
    }
    void OnDisable()
    {
        _landingEventRaiser.OnLanding -= TriggerShockwave;
    }

    void FixedUpdate()
    {
        if (!_isActive) return;

        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0f)
        {
            _isActive = false;
            return;
        }

        EmitShockwave();
    }

    void LateUpdate()
    {
        if (_isActive && _showDebugCircleInGame)
        {
            DebugUtils.DrawFilledCircle(transform.position, _hitboxRadius, _circleDebugMaterial);
        }
    }

    private void EmitShockwave()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _hitboxRadius); //sphere?

        foreach (var hit in hits)
        {
            GameObject target = hit.transform.parent.gameObject;

            if (target == gameObject || _alreadyHit.Contains(target)) continue;

            //check for circle around the player
            Vector3 flatDir = hit.transform.position - transform.position;
            flatDir.y = 0;

            if (flatDir.sqrMagnitude > _hitboxRadius * _hitboxRadius)

                _alreadyHit.Add(target);

            if (target.TryGetComponent(out Health health))
            {
                //Debug.Log("Damaged" + hit.gameObject.name);
                health.ChangeHealth(-_damage);
            }
            if (target.TryGetComponent(out Knockback knockback))
            {
                //Debug.Log("trying to knockback" + knockback.gameObject.name);
                Vector3 hitDir = (hit.transform.position - transform.position).normalized;
                knockback.ApplyKnockback(hitDir, Vector3.up, Vector3.zero);
            }
            if (hit.transform.parent.gameObject.TryGetComponent(out Breakable breakable))
            {
                if (breakable._type == this._canBreak)
                {
                    breakable.Break();
                }
            }

            if (_shouldConsumeHealth)
            {
                _resource.Change(-_shockwaveCost);
            }
        }
    }

    private void TriggerShockwave()
    {
        _isActive = true;
        _timer = _duration;
        _alreadyHit.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        DebugUtils.DrawCircle(transform.position, _hitboxRadius, _isActive ? Color.red : Color.white);
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _shouldConsumeHealth = true;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _shouldConsumeHealth = false;
    }
}


