using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //class responsible for managing a creature health
    //creature with health can: take damage, heal and die
    //should also update UI if relevant

    public event Action OnHealthChanged; //TODO: watch pavel's lesson on events
    public event Action OnDeath; 

    [SerializeField] private float _maxHealth = 100f;

    [SerializeField] private GameObject _optionalUI;

    private float _currentHealth;


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
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth > 0)
        {
            OnHealthChanged?.Invoke();
        }
        else if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }

    }

    private void HealthChangedEventTest()
    {
        UIManager.Instance.UpdateHealthBar(_currentHealth, _maxHealth);
    }

    private void DeathEventTest()
    {
        Debug.Log("Death Event Invoked");
        UIManager.Instance.ShowGameOverScreen();
        //TODO: disable movement
    }


}
