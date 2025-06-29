using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private IDamagable _healthSystem;

    void Awake()
    {
        _healthSystem = GetComponent<IDamagable>();
    }

    void OnEnable()
    {
        _healthSystem.OnHealthChanged += UpdateHealthBar;
    }

    void OnDisable() //not sure why i would ever disable this but i guess this is good practice
    {
        _healthSystem.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        //this is only for player currently
        UIManager.Instance.UpdateHealthBar(current, max);
    }
}
