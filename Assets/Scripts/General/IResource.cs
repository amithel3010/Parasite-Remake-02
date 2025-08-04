
using System;

public interface IResource
{
        float CurrentValue { get; }
        float MaxValue { get; }

        void Change(float amount); // Negative for cost, positive for regen
        bool CanAfford(float amount); // Useful for checking before consumption
        
        event Action<float,float> OnValueChanged; 
    
}
