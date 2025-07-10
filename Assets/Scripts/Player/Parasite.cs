using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Parasite : MonoBehaviour, ICollector
{
    //assumes haveing Health, rigidbody, hover controller, input handler.

    //TODO: maybe should be a singleton?

    //will check downwards for possessables
    // if found, will possess
    //when possess, stop checking disable hover script, and give reference to player input

    [Header("Raycast")]
    [SerializeField] private LayerMask _possessableLayer;
    [SerializeField] private float _possessRayLength;

    [Header("On UnPossession")]
    [SerializeField] private float _ejectForce = 50f;
    [SerializeField] private float _possessionCooldown = 3f;

    [Header("Other")]
    [SerializeField] private GameObject _gfx;

    private bool _canPossess = true;

    private Possessable _currentlyPossessed;
    private Transform _currentlyPossessedTransform;

    private InputHandler _playerInput;
    private Rigidbody _rb;
    private Health _parasiteHealth;

    void Awake()
    {
        _playerInput = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
        _parasiteHealth = GetComponent<Health>();

        _parasiteHealth.OnDamaged += TriggerPossessionCooldown; //TODO: this feels wrong
    }

    void FixedUpdate()
    {
        if (_currentlyPossessed == null && _canPossess && _rb.linearVelocity.y < -0.2f)
        {
            TryPossess();
        }
        else if (_currentlyPossessedTransform != null)
        {
            //TODO: better way?
            _rb.position = _currentlyPossessedTransform.position;
            _rb.rotation = _currentlyPossessedTransform.rotation;
        }

        if (_playerInput._actionPressed)
            {
                ExitPossessable();
            }
    }

    #region possesion

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
                //deactivate rb
                _rb.isKinematic = true;
                _rb.detectCollisions = false;

                foreach (var sensitive in GetComponents<IPossessionSource>())
                {
                    sensitive.OnParasitePossession();
                }
                //disable gfx
                _gfx.SetActive(false);

                _currentlyPossessed.OnPossess(this, _playerInput);
                _canPossess = false;
            }
        }
    }

    private void StopPossessing()
    {
        if (_currentlyPossessed == null) return;

        Vector3 targetExitPosition = _currentlyPossessedTransform != null
               ? _currentlyPossessedTransform.position + Vector3.up * 1.5f
               : transform.position;
        _currentlyPossessed.OnUnPossess(this);

        _rb.position = targetExitPosition;

        _currentlyPossessed = null;
        _currentlyPossessedTransform = null;

        //this.transform.SetParent(null);
        foreach (var sensitive in GetComponents<IPossessionSource>())
        {
            sensitive.OnParasiteUnPossession(); //feels weirddd
        }

        _gfx.SetActive(true);
        _rb.isKinematic = false;
        _rb.detectCollisions = true;

        _rb.AddForce(Vector3.up * _ejectForce, ForceMode.Impulse); //that's for exiting Possessable with height
        StartCoroutine(PossessionCooldown(_possessionCooldown));
    }

    private IEnumerator PossessionCooldown(float CooldownDuration)
    {
        if (_canPossess == true)
            _canPossess = false;

        yield return new WaitForSeconds(CooldownDuration);
        _canPossess = true;

    }

    public void ExitPossessable()
    {
        if (_currentlyPossessed == null)
        {
            Debug.Log("Action pressed, but parasite is not controlling any possessable");
        }
        else
        {
            StopPossessing();
        }
    }

    private void TriggerPossessionCooldown(float CooldownDuration)
    {
        StartCoroutine(PossessionCooldown(CooldownDuration));
    }

    #endregion

    public void Collect(Collectable collectable)
    {
        CollectableManager.Instance.CollectCollectable(collectable);
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

        //TODO: _movementScript.enabled = true;
        _gfx.SetActive(true);
        _rb.isKinematic = false;
        _rb.detectCollisions = true;

        _parasiteHealth.ResetHealth(); //maybe event? ONRESPAWN?
    }

}
