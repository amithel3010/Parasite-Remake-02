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

    public void OnPossess(IInputSource inputSource, Parasite parasite)
    {
        _controller.OnPossess(inputSource, parasite);
        IsPossessedByPlayer = true;
    }

    public void OnUnPossess()
    {
        _controller.OnUnPossess();
        IsPossessedByPlayer = false;

        if (_dieOnUnPossess)
        {
            _healthSystem.ChangeHealth(-_healthSystem.CurrentHealth); //die
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
