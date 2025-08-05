using System;
using UnityEngine;

public class Stamina : MonoBehaviour, IResource
{
    [SerializeField] private float _currentStamina; //serialized for debugging
    [SerializeField] private float _maxStamina = 100f;
    
    void Awake()
    {
        _currentStamina = _maxStamina;
    }

    public float CurrentValue => _currentStamina;
    public float MaxValue => _maxStamina;
    public event Action<float, float> OnValueChanged;   
    public void Change(float amount)
    {
        
        float prevStamina = _currentStamina;
        _currentStamina += amount;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
        
        OnValueChanged?.Invoke(_currentStamina, _maxStamina);
        
    }

    public bool CanAfford(float amount)
    {
        return amount <= _currentStamina;
    }

}
