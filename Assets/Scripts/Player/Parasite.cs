using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Parasite : MonoBehaviour, ICollector
{
    //assumes haveing Health, rigidbody, hover controller, input handler.
    
    //TODO: maybe should be a singleton?

    //TODO: on death, should probably disable movement script.

    //will check downwards for possessables
    // if found, will possess
    //when possess, stop checking disable hover script, and give reference to player input

    [Header("Raycast")]
    [SerializeField] private LayerMask _possessableLayer;
    [SerializeField] private float _possessRayLength;

    [Header("Possession")]
    [SerializeField] private float _ejectForce = 50f;
    [SerializeField] private float _possessionCooldown = 3f;

    [Header("Other")]
    [SerializeField] private GameObject _gfx;

    private bool _canPossess = true;

    private Possessable _currentlyPossessed;
    private Transform _currentlyPossessedTransform;
    private IDamagable _currentlyPossessedHealthSystem;

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
        if (_currentlyPossessed == null && _canPossess && _rb.linearVelocity.y < -0.2f)
        {
            TryPossess();
        }

        if (_playerInput._actionPressed)
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

            Possessable target = hitInfo.transform.GetComponent<Possessable>();
            if (target != null)
            {
                _currentlyPossessed = target;
                _currentlyPossessedTransform = target.transform;

                _currentlyPossessedHealthSystem = _currentlyPossessedTransform.GetComponent<IDamagable>();
                if (_currentlyPossessedHealthSystem != null)
                {
                    _currentlyPossessedHealthSystem.OnDeath += StopPossessing;
                }

                _parasiteHealth.ResetHealth(); // on possess reset health

                _rb.isKinematic = true;
                _rb.detectCollisions = false;
                _movementScript.enabled = false;
                _gfx.SetActive(false);

                _currentlyPossessed.OnPossess(_playerInput); // on possess update input
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
        //TODO: vfx and sfx
        Debug.Log("Collected" + collectable);
    }

    public bool IsControlling(GameObject obj)
    {
        //is the parasite controlling this object?
        if (obj == this.gameObject) return true; //the object is the parrasite itself

        if (_currentlyPossessedTransform != null && _currentlyPossessedTransform.gameObject == obj)
            return true;

        return false;
    }

    public void RespawnAt(Vector3 respawnPos)
    {
        //reset position and velocity. I wonder if this could cause problems...
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.position = respawnPos;

        _movementScript.enabled = true;
        _gfx.SetActive(true);
        _rb.isKinematic = false;
        _rb.detectCollisions = true;

        _parasiteHealth.ResetHealth(); //maybe event? ONRESPAWN?
    }
}
