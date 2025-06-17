using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private PlayerHealth _playerHealth;

    void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    void OnEnable()
    {
        _playerHealth.OnHealthChanged += UpdateHealthBar;
        _playerHealth.OnDeath += ShowGameOverScreen;
    }

    void OnDisable() //not sure why i would ever disable this but i guess this is good practice
    {
        _playerHealth.OnHealthChanged -= UpdateHealthBar;
        _playerHealth.OnDeath -= ShowGameOverScreen;
    }

    private void UpdateHealthBar(float current, float max)
    {
        UIManager.Instance.UpdateHealthBar(current, max);
    }

    private void ShowGameOverScreen()
    {
        UIManager.Instance.ShowGameOverScreen();
    }
}
