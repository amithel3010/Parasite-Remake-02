using UnityEngine;

public class Possessable : MonoBehaviour, IPossessable
{
    [SerializeField] private bool _dieOnUnPossess = true;

    private HoveringCreatureController _controller; //might need to make this an interface
    private IDamagable _healthSystem;
    
    [HideInInspector] public bool IsPossessedByPlayer = false;

    void Awake()
    {
        _controller = GetComponent<HoveringCreatureController>();
        _healthSystem = GetComponent<IDamagable>();
    }

    public void OnPossess(IInputSource inputSource, Parasite parasite)
    {
        _controller.OnPossess(inputSource, parasite);
        gameObject.AddComponent<Collector>(); //TODO: feels like a weird way of doing things
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
}
