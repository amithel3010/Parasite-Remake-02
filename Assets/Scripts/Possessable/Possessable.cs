using UnityEngine;

public class Possessable : MonoBehaviour, ICollector
{
    [SerializeField] private bool _dieOnUnPossess = true;
    private bool IsPossessedByPlayer = false;

    private Health _healthSystem;

    private LayerMask _playerControlledLayer;
    private LayerMask _possessableLayer;

    void Awake()
    {
        _healthSystem = GetComponent<Health>();

        _playerControlledLayer = LayerMask.NameToLayer("Player Controlled");//TODO: kinda breakable... also, i can get the layer throught the parasite object but i think this is a bit more readable. idk about performance
        _possessableLayer = LayerMask.NameToLayer("Possessable");//TODO: kinda breakable
    }

    public void OnPossess(Parasite playerParasite, IInputSource inputSource)
    {
        IsPossessedByPlayer = true;
        MiscUtils.SetLayerAllChildren(this.transform, _playerControlledLayer);

        IPossessionSensitive[] possessionSensitive = GetComponents<IPossessionSensitive>();
        foreach (var sensitive in possessionSensitive)
        {
            sensitive.OnPossessed(playerParasite, inputSource);
        }
    }

    public void OnUnPossess(Parasite playerParasite)
    {
        IsPossessedByPlayer = false;
        MiscUtils.SetLayerAllChildren(this.transform, _possessableLayer);

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

    public void Collect(Collectable collectable)
    {
        if (IsPossessedByPlayer)
        {
            //TODO: basically the same implementation as in parasite. do i really need an interface? might be a global way of checking what the player is controlling
            CollectableManager.Instance.CollectCollectable(collectable);
        }
    }
}
