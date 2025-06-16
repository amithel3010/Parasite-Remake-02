using UnityEngine;

public class Thorns : MonoBehaviour
{
    [SerializeField] float _damage = 25;

    private void OnCollisionEnter(Collision other)
    {
        //print("Collided with" + other.gameObject.name);

        if (other.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.ChangeHealth(-_damage);
            Debug.Log("thorns damaged" + damagable + "for" + _damage);
        }
    }
}

