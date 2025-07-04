using System;
using UnityEngine;

public interface IDamagable
{
    //TODO: i think this is useless beacuse i'm using only 1 implementation of health
    public void ChangeHealth(float amount);
    
    public void ResetHealth();

    public event Action OnDamaged;
   
    public event Action OnDeath;

    public event Action<float, float> OnHealthChanged;

    public float CurrentHealth { get; }

    public float MaxHealth { get; }
}
