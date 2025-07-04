using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BruteShockwave : MonoBehaviour
{
    [SerializeField] private float _damage = 25f;
    [SerializeField] private float _hitboxRadius = 1f;
    [SerializeField] private float _duration = 0.2f;

    private bool _isActive;
    private float _timer;


    private HashSet<GameObject> _alreadyHit = new HashSet<GameObject>();

    //Refs
    private HoveringCreatureController _controller;


    void Awake()
    {
        _controller = GetComponent<HoveringCreatureController>();

        _controller.OnLanding += EmitShockwave;
    }

//TODO: debug doesn't work because timer doesn't work
    private void EmitShockwave()
    {
        if (!_isActive)
        {
            _isActive = true;
            _timer = _duration;
            _alreadyHit.Clear();
        }

        if (!_isActive) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _isActive = false;
            return;
        }
        //Debug.Log("Emit Shockwave");

        Collider[] hits = Physics.OverlapSphere(transform.position, _hitboxRadius); //sphere?

        foreach (var hit in hits)
        {
            GameObject target = hit.transform.parent.gameObject;
            print(target);

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
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_isActive)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position, _hitboxRadius);
    }
}


