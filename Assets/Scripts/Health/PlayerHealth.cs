using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    //class responsible for managing a creature health
    //creature with health can: take damage, heal and die
    //should also update UI if relevant

    public event Action OnHealthChanged; //TODO: watch pavel's lesson on events
    public event Action OnDeath;

    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _IFramesDuration = 0.3f;

    private float _currentHealth;
    private bool _isHittable = true;


    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    void Awake()
    {
        OnHealthChanged += HealthChangedEventTest;
        OnDeath += DeathEventTest;

        _currentHealth = _maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeHealth(-5f);
        }
    }

    public void ChangeHealth(float amount)
    {
        if (_isHittable)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            if (_currentHealth > 0)
            {
                OnHealthChanged?.Invoke();
            }
            else if (_currentHealth <= 0)
            {
                OnHealthChanged?.Invoke();
                OnDeath?.Invoke();
            }
        }

    }

    private void HealthChangedEventTest()
    {
        UIManager.Instance.UpdateHealthBar(_currentHealth, _maxHealth);
        _isHittable = false;
        StartCoroutine(IsHittableCooldown());
    }

    private void DeathEventTest()
    {
        Debug.Log("Death Event Invoked");
        UIManager.Instance.ShowGameOverScreen();
        //TODO: disable movement
    }

    void IDamagable.OnDeath()
    {
        //
    }

    private IEnumerator IsHittableCooldown()
    {
        if (_isHittable == false)
        {
            yield return new WaitForSeconds(_IFramesDuration);
            _isHittable = true;
        }
    }
}
