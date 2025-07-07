using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BruteShockwave : MonoBehaviour
{
    [SerializeField] private BreakableType _canBreak;
    [SerializeField] private float _damage = 25f;
    [SerializeField] private float _hitboxRadius = 1f;
    [SerializeField] private float _duration = 0.2f;

    [Header("Debug")]
    [SerializeField] private Material _circleDebugMaterial;
    [SerializeField] private bool _showDebugCircleInGame = true;

    private bool _isActive;
    private float _timer;
    private HashSet<GameObject> _alreadyHit = new HashSet<GameObject>();

    //Refs
    private IHasLandedEvent _controller;

    void Awake()
    {
        _controller = GetComponent<IHasLandedEvent>();
    }

    void OnEnable()
    {
        _controller.OnLanding += TriggerShockwave;
    }
    void OnDisable()
    {
        _controller.OnLanding -= TriggerShockwave;
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

            if (target.TryGetComponent<Health>(out Health health))
            {
                //Debug.Log("Damaged" + hit.gameObject.name);
                health.ChangeHealth(-_damage);
            }
            if (target.TryGetComponent<KnockbackTest>(out KnockbackTest knockback))
            {
                //Debug.Log("trying to knockback" + knockback.gameObject.name);
                Vector3 hitDir = (hit.transform.position - transform.position).normalized;
                knockback.Knockback(hitDir, Vector3.up, Vector3.zero);
            }
            if (hit.transform.parent.gameObject.TryGetComponent<Breakable>(out Breakable breakable))
            {
                if (breakable._type == this._canBreak)
                {
                    breakable.Break();
                }
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
        if (_isActive)
        {
            DebugUtils.DrawCircle(transform.position, _hitboxRadius, Color.red);
        }
        else
        {
            DebugUtils.DrawCircle(transform.position, _hitboxRadius, Color.white);
        }
    }
}


