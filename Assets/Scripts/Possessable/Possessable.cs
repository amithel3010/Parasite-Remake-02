using UnityEngine;

public class Possessable : MonoBehaviour, ICollector
{
    [Header("Gfx")]
    [SerializeField] private GameObject _defaultGfx;
    [SerializeField] private GameObject _possessedGfx;

    private Health _healthSystem;
    private IPossessionSensitive[] _possessionSensitive;

    void Awake()
    {
        _possessionSensitive = GetComponents<IPossessionSensitive>();
        _healthSystem = GetComponent<Health>();

        if (_defaultGfx == null)
        {
            Debug.LogWarning("Possessable: _defaultGfx is null");
        }

        if (_possessedGfx == null)
        {
            Debug.LogWarning("Possessable: _possessedGfx is null");
        }
        
        _possessedGfx?.SetActive(false);
        _defaultGfx?.SetActive(true);
    }

    public void OnPossess(Parasite playerParasite, IInputSource inputSource)
    {
        LayerUtils.SetLayerAllChildren(this.transform, LayerUtils.PlayerControlledLayer);
        
        _possessedGfx?.SetActive(true);
        _defaultGfx?.SetActive(false);

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
        
        //Kill possessable after unpossessing
        _healthSystem.KillImmediately();
    }

    public void Collect(Collectable collectable) // called only if controlled by player
    {
        CollectableManager.Instance.CollectCollectable(collectable);
    }
}