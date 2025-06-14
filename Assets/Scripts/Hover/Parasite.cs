using Unity.VisualScripting;
using UnityEngine;

public class Parasite : MonoBehaviour
{
    //will check downwards for possessables
    // if found, will possess
    //when possess, stop checking disable hover script, and give reference to player input

    private bool _isPossessing;
    private GameObject _currentlyPossessed;

    [SerializeField] private LayerMask _possessableLayer;
    [SerializeField] private float _possessRayLength;
    [SerializeField] private GameObject _gfx;

    [SerializeField] float _explosionForce = 50f;

    private PhysicsBasedController _movementScript;
    private InputHandler _playerInput;
    private Rigidbody _rb;

    void Awake()
    {
        _movementScript = GetComponent<PhysicsBasedController>();
        _playerInput = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!_isPossessing)
        {
            Ray PossessCheckRay = new(transform.position, -transform.up);
            if (Physics.Raycast(PossessCheckRay, out RaycastHit hitInfo, _possessRayLength, _possessableLayer))
            {
                Possess(hitInfo);
            }
        }
    }

    private void Possess(RaycastHit PossessedInfo)
    {
        Debug.Log("Trying to possess" + PossessedInfo.transform.name);

        //make rigid body kinematic...
        _rb.isKinematic = true;
        _rb.detectCollisions = false;
        //stop hovering...
        _movementScript.enabled = false;
        //make this a child of what it hit...
        //hitInfo.transform.SetParent(this.transform);
        this.transform.SetParent(PossessedInfo.transform); //TODO:isn't it weird that the child is controlling the parent?

        //disable gfx...
        _gfx.SetActive(false);

        //give control to possessable...
        _currentlyPossessed = PossessedInfo.transform.gameObject;
        _currentlyPossessed.GetComponent<PhysicsBasedController>().ChangeInputSource(_playerInput);
        _currentlyPossessed.GetComponent<PhysicsBasedController>()._parasitePossessing = this; //TODO: this is so wrong...

        //flag
        _isPossessing = true;
    }

    public void StopPossessing() //this is called from WITHIN possessable, which feels very very wrong
    {
        _rb.isKinematic = false;
        _rb.detectCollisions = true;

        _movementScript.enabled = true;

        this.transform.SetParent(null);

        _gfx.SetActive(true);

        _rb.AddForce(Vector3.up * _explosionForce, ForceMode.Impulse);
        _currentlyPossessed = null;
        _isPossessing = false;
    }
}
