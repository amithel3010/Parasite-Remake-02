using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] float _damage = 25;

    public bool ShouldDamageOnCollision = true; //TODO: seems not ideal

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
}
