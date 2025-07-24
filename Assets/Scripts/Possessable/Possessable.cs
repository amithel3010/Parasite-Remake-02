using UnityEngine;

public class Possessable : MonoBehaviour, ICollector
{
    [SerializeField] private bool _dieOnUnPossess = true;

    private Health _healthSystem;
    private IPossessionSensitive[] _possessionSensitive;

    void Awake()
    {
        _possessionSensitive = GetComponents<IPossessionSensitive>();
        _healthSystem = GetComponent<Health>();
    }

    public void OnPossess(Parasite playerParasite, IInputSource inputSource)
    {
        LayerUtils.SetLayerAllChildren(this.transform, LayerUtils.PlayerControlledLayer);

        foreach (var sensitive in _possessionSensitive)
        {
            sensitive.OnPossessed(playerParasite, inputSource);
        }
    }

    public void OnUnPossess(Parasite playerParasite)
    {
        LayerUtils.SetLayerAllChildren(this.transform, LayerUtils.PossessableLayer);

        IPossessionSensitive[] possessionSensitive = _possessionSensitive;
        foreach (var sensitive in possessionSensitive)
        {
            sensitive.OnUnPossessed(playerParasite);
        }

        if (_dieOnUnPossess)
        {
            _healthSystem.KillImmediately();
        }
    }

    public void Collect(Collectable collectable) // called only if controlled by player
    {     
            CollectableManager.Instance.CollectCollectable(collectable);      
    }
}
