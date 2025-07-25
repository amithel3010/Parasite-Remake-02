using System.Collections;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour, IPossessionSensitive
{
    [SerializeField] float _damage = 25;

    private bool ShouldDamageOnCollision = true;

    private GameObject _Possessor;
    private float _safeDuration = 1f;
    private float _timeOfUnpossess = -999f;

    private void OnCollisionEnter(Collision other)
    {
        if (!ShouldDamageOnCollision) return;

        GameObject DamageTarget = other.gameObject;

        if (DamageTarget == _Possessor && Time.time - _timeOfUnpossess < _safeDuration)
        {
            return;
        }
        else
        {
            _Possessor = null;
        }

        //TODO: check if hitdirXZ is better?
        Vector3 hitDir = (other.transform.position - transform.position).normalized;
        Vector3 hitDirXZ = new Vector3(hitDir.x, 0f, hitDir.z).normalized;

        if (DamageTarget.TryGetComponent<Health>(out Health health))
        {
            health.ChangeHealth(-_damage);
            Debug.Log(this.name + " damaged " + health + " for" + _damage);
        }

        if (DamageTarget.TryGetComponent<Knockback>(out Knockback knockback))
        {
            knockback.ApplyKnockback(hitDirXZ, Vector3.up, Vector3.zero);
        }
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        ShouldDamageOnCollision = false;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _timeOfUnpossess = Time.time;
        _Possessor = playerParasite.gameObject;
    }


}
