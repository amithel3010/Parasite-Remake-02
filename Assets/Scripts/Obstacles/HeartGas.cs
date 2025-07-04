using UnityEngine;

public class HeartGas : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggerd by" + other.name);
        if (other.transform.parent.TryGetComponent<Possessable>(out Possessable possessable))
        {
            if (possessable.TryGetComponent<Health>(out var health))
            {
                health.ChangeHealth(-health.MaxHealth);
            }
        }

    }
}
