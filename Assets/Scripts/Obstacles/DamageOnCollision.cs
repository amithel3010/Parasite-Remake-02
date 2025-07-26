using System.Collections;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour, IPossessionSensitive
{
    [SerializeField] float _damage = 25;

    private bool _shouldDamageOnCollision = true;

    private GameObject _possessor;
    private readonly float _safeDurationAfterUnPossession = 1f; //what does it mean that this is readonly
    private float _timeOfUnpossess = -999f;

    private void OnCollisionEnter(Collision other)
    {
        if (!_shouldDamageOnCollision) return;

        GameObject damageTarget = other.gameObject;


//TODO: this is kinda hard to read
        if (damageTarget == _possessor && Time.time - _timeOfUnpossess < _safeDurationAfterUnPossession)
        {
            return;
        }
        else
        {
            _possessor = null;
        }

        //TODO: check if hitDirXZ is better?
        Vector3 hitDir = (other.transform.position - transform.position).normalized;
        Vector3 hitDirXZ = new Vector3(hitDir.x, 0f, hitDir.z).normalized;

        if (damageTarget.TryGetComponent(out Health health))
        {
            health.ChangeHealth(-_damage);
            Debug.Log(this.name + " damaged " + health + " for" + _damage);
        }

        if (damageTarget.TryGetComponent(out Knockback knockback))
        {
            knockback.ApplyKnockback(hitDirXZ, Vector3.up, Vector3.zero);
        }
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _shouldDamageOnCollision = false;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _timeOfUnpossess = Time.time;
        _possessor = playerParasite.gameObject;
    }
}