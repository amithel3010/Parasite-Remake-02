using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Parasite : MonoBehaviour, ICollector
{
    //will check downwards for possessables
    // if found, will possess
    //when possess, stop checking disable hover script, and give reference to player input

    //TODO: will be easier to handle if it became the parent of possessed creature and not the other way around...

    [Header("Raycast")]
    [SerializeField] private LayerMask _possessableLayer;
    [SerializeField] private float _possessRayLength;

    [Header("Possession")]
    [SerializeField] private float _ejectForce = 50f;
    [SerializeField] private float _possessionCooldown = 3f;

    [Header("Other")]
    [SerializeField] private GameObject _gfx;

    private bool _canPossess = true;

    private IPossessable _currentlyPossessed;
    private Transform _currentlyPossessedTransform;
    private IDamagable _currentlyPossessedHealthSystem;

    //private PhysicsBasedController _movementScript;
    private HoveringCreatureController _movementScript; //coupled... hard to change

    private InputHandler _playerInput;
    private Rigidbody _rb;
    private IDamagable _parasiteHealth;

    void Awake()
    {
        _movementScript = GetComponent<HoveringCreatureController>();
        _playerInput = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
        _parasiteHealth = GetComponent<IDamagable>();

        _parasiteHealth.OnDamaged += TriggerPossessionCooldown; //TODO: this feels wrong
    }

    void FixedUpdate()
    {
        if (_currentlyPossessed == null && _canPossess && _rb.linearVelocity.y < -0.5f)
        {
            TryPossess();
        }

        if (_playerInput.actionPressed)
        {
            ExitPossessableOnActionPress();
        }
    }

    private void TryPossess()
    {
        Ray PossessCheckRay = new(transform.position, -transform.up);
        if (Physics.Raycast(PossessCheckRay, out RaycastHit hitInfo, _possessRayLength, _possessableLayer))
        {
            Debug.Log("Trying to possess" + hitInfo.transform.name);

            IPossessable target = hitInfo.transform.GetComponent<IPossessable>();
            if (target != null)
            {
                _currentlyPossessed = target;
                _currentlyPossessedTransform = hitInfo.transform;

                _currentlyPossessedHealthSystem = _currentlyPossessedTransform.GetComponent<IDamagable>();
                if (_currentlyPossessedHealthSystem != null)
                {
                    _currentlyPossessedHealthSystem.OnDeath += StopPossessing;
                }

                _parasiteHealth.ResetHealth();

                _rb.isKinematic = true;
                _rb.detectCollisions = false;
                _movementScript.enabled = false;
                _gfx.SetActive(false);
                //transform.SetParent(_currentlyPossessedTransform); //TODO:isn't it weird that the child is controlling the parent?

                _currentlyPossessed.OnPossess(_playerInput, this);
                _canPossess = false;

            }
        }
    }

    public void StopPossessing()
    {
        if (_currentlyPossessed == null) return;

        Vector3 targetExitPosition = _currentlyPossessedTransform != null
               ? _currentlyPossessedTransform.position + Vector3.up * 1.5f
               : transform.position;
        _currentlyPossessed.OnUnPossess();

        if (_currentlyPossessedHealthSystem != null)
        {
            _currentlyPossessedHealthSystem.OnDeath -= StopPossessing;
        }

        _rb.position = targetExitPosition;

        _currentlyPossessed = null;
        _currentlyPossessedTransform = null;
        _currentlyPossessedHealthSystem = null;


        //this.transform.SetParent(null);

        _gfx.SetActive(true);
        _rb.isKinematic = false;
        _rb.detectCollisions = true;
        _movementScript.enabled = true;

        _rb.AddForce(Vector3.up * _ejectForce, ForceMode.Impulse); //that's for exiting Possessable with height
        StartCoroutine(PossessionCooldown());
    }

    private IEnumerator PossessionCooldown()
    {
        if (_canPossess == true)
            _canPossess = false;

        yield return new WaitForSeconds(_possessionCooldown);
        _canPossess = true;

    }

    private void ExitPossessableOnActionPress()
    {
        if (_currentlyPossessed == null)
        {
            Debug.Log("Action pressed, but player is controlling parasite");
        }
        else
        {
            StopPossessing();
        }
    }

    private void TriggerPossessionCooldown()
    {
        StartCoroutine(PossessionCooldown());
    }

    public void Collect(Collectable collectable)
    {
        Debug.Log("Collected" + collectable);
    }
}
