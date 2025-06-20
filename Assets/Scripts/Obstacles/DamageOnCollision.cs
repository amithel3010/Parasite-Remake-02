using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] float _damage = 25;

    private void OnCollisionEnter(Collision other)
    {
        //print("Collided with" + other.gameObject.name);
        GameObject DamageTarget = other.gameObject;
        Vector3 hitDir = (other.transform.position - transform.position).normalized; //backwards?

        if (DamageTarget.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.ChangeHealth(-_damage);
            Debug.Log(this.name + " damaged " + damagable + " for" + _damage);

            //TODO: Knockback should happen here
        }
        if (DamageTarget.TryGetComponent<KnockbackTest>(out KnockbackTest knockback))
        {
            Debug.Log("Attempting to knockback");
            knockback.Knockback(hitDir, Vector3.up, Vector3.zero);
        }
    }
}
