using UnityEngine;

public class Possessable : MonoBehaviour, IPossessable, ICollector
{
    [HideInInspector] public bool IsPossessedByPlayer = false;

    [SerializeField] private bool _dieOnUnPossess = true;

    private HoveringCreatureController _controller; //might need to make this an interface
    private IDamagable _healthSystem;

    void Awake()
    {
        _controller = GetComponent<HoveringCreatureController>();
        _healthSystem = GetComponent<IDamagable>();
    }

    public void OnPossess(IInputSource inputSource)
    {
        //for each possession sesitive component, OnPossess
        _controller.OnPossess(inputSource);
        IsPossessedByPlayer = true;

        if (TryGetComponent<DamageOnCollision>(out var damageOnCollision)) //could make a PossessoionSensitive interface to make it easier to enable and disable all scripts that are sensitive to player possession
        {
            if (damageOnCollision.ShouldDamageOnCollision)
            {
                damageOnCollision.ShouldDamageOnCollision = false;
            }
        }
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
            if (TryGetComponent<DamageOnCollision>(out var damageOnCollision))
            {
                if (!damageOnCollision.ShouldDamageOnCollision)
                {
                    damageOnCollision.ShouldDamageOnCollision = true;
                }
            }
        }
    }

    public void Collect(Collectable collectable)
    {
        if (IsPossessedByPlayer)
        {
            //TODO: Vfx and Sfx
            Debug.Log("Collected" + collectable);
        }
    }
}
