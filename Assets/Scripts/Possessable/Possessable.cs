using System;
using UnityEngine;

public class Possessable : MonoBehaviour, ICollector
{
    public bool IsPossessedByPlayer = false;

    [SerializeField] private bool _dieOnUnPossess = true;

    private HoveringCreatureController _controller; //might need to make this an interface
    private Health _healthSystem;

    public event Action<IInputSource> Possessed;
    public event Action UnPossessed;

    void Awake()
    {
        _controller = GetComponent<HoveringCreatureController>();
        _healthSystem = GetComponent<Health>();
    }

    public void OnPossess(IInputSource inputSource)
    {
        //for each possession sesitive component on this game object, OnPossess
        _controller.OnPossess(inputSource);
        IsPossessedByPlayer = true;

        Possessed?.Invoke(inputSource);

        // if (TryGetComponent<DamageOnCollision>(out var damageOnCollision)) //could make a PossessoionSensitive interface to make it easier to enable and disable all scripts that are sensitive to player possession
        // {
        //     if (damageOnCollision.ShouldDamageOnCollision)
        //     {
        //         damageOnCollision.ShouldDamageOnCollision = false;
        //     }
        // }
    }

    public void OnUnPossess()
    {
        _controller.OnUnPossess();
        IsPossessedByPlayer = false;

        if (_dieOnUnPossess)
        {
            _healthSystem.ChangeHealth(-_healthSystem.CurrentHealth); //die
        }
        else
        {
            UnPossessed?.Invoke();
            // if (TryGetComponent<DamageOnCollision>(out var damageOnCollision))
            // {
            //     if (!damageOnCollision.ShouldDamageOnCollision)
            //     {
            //         damageOnCollision.ShouldDamageOnCollision = true;
            //     }
            // }
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
