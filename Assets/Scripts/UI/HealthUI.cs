using UnityEngine;

public class HealthUI : MonoBehaviour, IPossessionSensitive, IPossessionSource
{
    //TODO: manager class changes value, Health UI on possess give ui manager values and sprite

    [SerializeField] private bool _isPlayerOnStart;
    private Health _health;

    private bool _isCurrentUITarget;

    void Awake()
    {
        _health = GetComponent<Health>();
        if (_isPlayerOnStart)
        {
            _isCurrentUITarget = true;
        }
    }

    void OnEnable()
    {
        _health.OnHealthChanged += UpdateHealthBar;
    }

    void OnDisable() 
    {
        _health.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        if (_isCurrentUITarget)
        {
            UIManager.Instance.UpdateHealthBar(current, max);
        }
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _isCurrentUITarget = true;
        UpdateHealthBar(_health.CurrentHealth, _health.MaxHealth);
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _isCurrentUITarget = false;
    }

    public void OnParasitePossession()
    {
        _isCurrentUITarget = false;
    }

    public void OnParasiteUnPossession()
    {
        _isCurrentUITarget = true;
        UpdateHealthBar(_health.CurrentHealth, _health.MaxHealth);
    }
}
