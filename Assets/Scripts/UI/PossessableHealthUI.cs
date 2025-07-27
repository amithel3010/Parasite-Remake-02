using UnityEngine;
using UnityEngine.UI;

public class PossessableHealthUI : MonoBehaviour, IPossessionSensitive
{
    //TODO: Unused Script
    
    
    //On possess, set new sprite for health bar
    //this possessable health values to update it

    [SerializeField] private Image _healthbarSprite;

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        throw new System.NotImplementedException();
    }
}
