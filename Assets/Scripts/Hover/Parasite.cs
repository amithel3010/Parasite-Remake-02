using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Parasite : MonoBehaviour
{
    //will check downwards for possessables
    // if found, will possess
    //when possess, stop checking disable hover script, and give reference to player input

    [Header("Raycast")]
    [SerializeField] private LayerMask _possessableLayer;
    [SerializeField] private float _possessRayLength;

    [Header("Possession")]
    [SerializeField] private float _explosionForce = 50f;
    [SerializeField] private float _possessionCooldown = 3f;

    [Header("Other")]
    [SerializeField] private GameObject _gfx;

    private bool _ableToPossess = true;
    private IPossessable _currentlyPossessed;
    private Transform _currentlyPossessedTransform;

    //private PhysicsBasedController _movementScript;
    private HoveringCreatureController _movementScript;

    private InputHandler _playerInput;
    private Rigidbody _rb;
    private Health _healthSystem;

    void Awake()
    {
        _movementScript = GetComponent<HoveringCreatureController>();
        _playerInput = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
        _healthSystem = GetComponent<Health>();

        _healthSystem.OnDamaged += StartPossessionCooldown; //TODO: this feels wrong
    }

    void FixedUpdate()
    {
        if (_currentlyPossessed == null && _ableToPossess)
        {
            TryPossess();
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

                _healthSystem.ResetHealth();

                _rb.isKinematic = true;
                _rb.detectCollisions = false;
                _movementScript.enabled = false;
                _gfx.SetActive(false);
                transform.SetParent(_currentlyPossessedTransform); //TODO:isn't it weird that the child is controlling the parent?

                _currentlyPossessed.OnPossess(_playerInput, this);
                _ableToPossess = false;

            }
        }
    }

    public void StopPossessing() //this is called from WITHIN possessable, which feels very very wrong
    {
        if (_currentlyPossessed == null) return;

        _currentlyPossessed.OnUnPossess();
        _currentlyPossessed = null;

        this.transform.SetParent(null);
        _gfx.SetActive(true);

        _rb.isKinematic = false;
        _rb.detectCollisions = true;

        _movementScript.enabled = true;

        _rb.AddForce(Vector3.up * _explosionForce, ForceMode.Impulse);
        StartCoroutine(PossessionCooldown());
    }

    private IEnumerator PossessionCooldown()
    {
        if (_ableToPossess == true)
            _ableToPossess = false;

        yield return new WaitForSeconds(_possessionCooldown);
        _ableToPossess = true;

    }

    private void StartPossessionCooldown()
    {
        StartCoroutine(PossessionCooldown());
    }
}
