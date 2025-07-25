using UnityEngine;
using System;
using System.Collections;

public class Health : MonoBehaviour
{
    //class responsible for managing a creature health
    //creature with health can: take damage, heal and die
    //raises events for other scripts to listen to

    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _iFramesDuration = 0.3f;

    public event Action<float, float> OnHealthChanged;
    public event Action<float> OnDamaged;
    public event Action OnFinshedIFrames;
    public event Action OnDeath;

    private float _currentHealth;
    private bool _isHittable = true;
    private bool _isInvincible = false;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void ChangeHealth(float amount)
    {
        if (!_isHittable || _isInvincible) return;

        float oldHealth = _currentHealth;
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
        else if (_currentHealth < oldHealth)
        {
            OnDamaged?.Invoke(_iFramesDuration);
            _isHittable = false;
            StartCoroutine(IFrameCooldown());
        }
    }

    public void ResetHealth()
    {
        ChangeHealth(_maxHealth);
    }

    private IEnumerator IFrameCooldown()
    {
        if (_isHittable == false)
        {
            yield return new WaitForSeconds(_iFramesDuration);
            OnFinshedIFrames?.Invoke();
            _isHittable = true;
        }
    }

    public void ToggleInvincible() // for debugging
    {
        _isInvincible = !_isInvincible;
        if (_isInvincible)
        {
            Debug.Log("Parasite is now INVINCIBLE");
        }
        else if (!_isInvincible)
        {
            Debug.Log("Parasite is now HITTABLE");
        }
    }



}
