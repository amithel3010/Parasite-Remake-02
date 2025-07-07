using System.Collections;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour, IPossessionSensitive
{
    [SerializeField] float _damage = 25;

    private bool ShouldDamageOnCollision = true;

    private void OnCollisionEnter(Collision other)
    {
        if (!ShouldDamageOnCollision) return;

        GameObject DamageTarget = other.gameObject;
        Vector3 hitDir = (other.transform.position - transform.position).normalized;

        if (DamageTarget.TryGetComponent<Health>(out Health health))
        {
            health.ChangeHealth(-_damage);
            Debug.Log(this.name + " damaged " + health + " for" + _damage);
        }

        if (DamageTarget.TryGetComponent<KnockbackTest>(out KnockbackTest knockback))
        {
            knockback.Knockback(hitDir, Vector3.up, Vector3.zero);
        }
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        ShouldDamageOnCollision = false;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        StartCoroutine(WaitBeforeReactivating());
    }

    private IEnumerator WaitBeforeReactivating() //TODO: kind of a cheat fix
    {
        if (ShouldDamageOnCollision == false)
        {
            yield return new WaitForSeconds(1);
            ShouldDamageOnCollision = true;
        }
    }
}
