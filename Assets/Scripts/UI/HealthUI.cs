using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private Health _health;

    void Awake()
    {
        _health = GetComponent<Health>();
    }

    void OnEnable()
    {
        _health.OnHealthChanged += UpdateHealthBar;
    }

    void OnDisable() //not sure why i would ever disable this but i guess this is good practice
    {
        _health.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        //this is only for player currently
        UIManager.Instance.UpdateHealthBar(current, max);
    }
}
