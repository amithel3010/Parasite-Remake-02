using UnityEngine;

public interface IDamagable
{
    public void ChangeHealth(float amount);

    public void OnDeath();

    public float CurrentHealth { get; }

    public float MaxHealth{ get; }
}
