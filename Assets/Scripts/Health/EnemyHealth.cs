using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _iFramesDuration = 0.3f;

    private float _currentHealth;
    private bool _isHittable = true;
    
    public float CurrentHealth => _currentHealth;

    public float MaxHealth => _maxHealth;

    public event Action OnDamaged;
    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void ChangeHealth(float amount)
    {
        if (!_isHittable) return;

        float oldHealth = _currentHealth;
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth < oldHealth)
        {
            OnDamaged?.Invoke();
            _isHittable = false;
            StartCoroutine(IFrameCooldown());
        }

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    private IEnumerator IFrameCooldown()
    {
        if (_isHittable == false)
        {
            yield return new WaitForSeconds(_iFramesDuration);
            _isHittable = true;
        }
    }

}
