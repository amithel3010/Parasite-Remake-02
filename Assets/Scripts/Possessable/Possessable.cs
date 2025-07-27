using UnityEngine;

public class Possessable : MonoBehaviour, ICollector
{
    [SerializeField] private bool _dieOnUnPossess = true;

    private Health _healthSystem;

    void Awake()
    {
        _healthSystem = GetComponent<Health>();
    }

    public void OnPossess(Parasite playerParasite, IInputSource inputSource)
    {
        MiscUtils.SetLayerAllChildren(this.transform, LayerUtils.PlayerControlledLayer);

        IPossessionSensitive[] possessionSensitive = GetComponents<IPossessionSensitive>();
        foreach (var sensitive in possessionSensitive)
        {
            sensitive.OnPossessed(playerParasite, inputSource);
        }
    }

    public void OnUnPossess(Parasite playerParasite)
    {
        MiscUtils.SetLayerAllChildren(this.transform, LayerUtils.PossessableLayer);

        IPossessionSensitive[] possessionSensitive = GetComponents<IPossessionSensitive>();
        foreach (var sensitive in possessionSensitive)
        {
            sensitive.OnUnPossessed(playerParasite);
        }

        if (_dieOnUnPossess)
        {
            _healthSystem.ChangeHealth(-_healthSystem.CurrentHealth); //die
        }
    }

    public void Collect(Collectable collectable) // called only if controlled by player
    {     
            CollectableManager.Instance.CollectCollectable(collectable);      
    }
}
