using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] float _damage = 25;

    private Possessable _optionalPossessable;
    public bool ShouldDamageOnCollision = true;

    private void Awake()
    {
        _optionalPossessable = GetComponent<Possessable>();
        if (_optionalPossessable != null)
        {
            _optionalPossessable.Possessed += OnPossessed;
            _optionalPossessable.UnPossessed += OnUnPossessed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!ShouldDamageOnCollision) return;

        GameObject DamageTarget = other.gameObject;
        Vector3 hitDir = (other.transform.position - transform.position).normalized;

        if (DamageTarget.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.ChangeHealth(-_damage);
            Debug.Log(this.name + " damaged " + damagable + " for" + _damage);
        }

        if (DamageTarget.TryGetComponent<KnockbackTest>(out KnockbackTest knockback))
        {
            knockback.Knockback(hitDir, Vector3.up, Vector3.zero);
        }
    }

    private void OnPossessed(IInputSource inputSource)
    {
        ShouldDamageOnCollision = false;
    }
    private void OnUnPossessed()
    {
        ShouldDamageOnCollision = true;
    }
}
