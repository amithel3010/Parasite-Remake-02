using System;
using UnityEngine;

public class Possessable : MonoBehaviour, ICollector
{
    [SerializeField] private bool _dieOnUnPossess = true;
    private bool IsPossessedByPlayer = false;

    private Health _healthSystem;

    void Awake()
    {
        _healthSystem = GetComponent<Health>();
    }

    public void OnPossess(Parasite playerParasite, IInputSource inputSource)
    {
        IsPossessedByPlayer = true;

        IPossessionSensitive[] possessionSensitive = GetComponents<IPossessionSensitive>();
        foreach (var sensitive in possessionSensitive)
        {
            sensitive.OnPossessed(playerParasite, inputSource);
        }
    }

    public void OnUnPossess(Parasite playerParasite)
    {
        IsPossessedByPlayer = false;

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
