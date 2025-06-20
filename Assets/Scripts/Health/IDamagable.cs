using System;
using UnityEngine;

public interface IDamagable
{
    public void ChangeHealth(float amount);
    
    public void ResetHealth();

    public event Action OnDamaged;
   
    public event Action OnDeath;

    public event Action<float, float> OnHealthChanged;

    public float CurrentHealth { get; }

    public float MaxHealth { get; }
}
