using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] float _damage = 25;

    private void OnCollisionEnter(Collision other)
    {
        //print("Collided with" + other.gameObject.name);

        if (other.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.ChangeHealth(-_damage);
            Debug.Log(this.name + " damaged " + damagable + " for" + _damage);

            //TODO: Knockback should happen here
        }
    }
}
