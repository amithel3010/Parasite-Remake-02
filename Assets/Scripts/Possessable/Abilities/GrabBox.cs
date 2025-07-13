using UnityEngine;


[RequireComponent(typeof(Hover))]
[RequireComponent(typeof(InputBasedHoverMovement))]
public class GrabBox : MonoBehaviour, IPossessionSensitive
{
    //On action press, if the cat is near a wooden box,
    //snap the box to a holdingPos,
    //adjust ride height and ground ray distance?
    //box becomes kinematic?
    //cat should feel heavier when holding box. adjust rb.mass?
    // if already holding box, release it.


    [Header("Pickup Transform")]
    [SerializeField] Transform _holder;

    [Header("Raycast Settings")]
    //[SerializeField] private float _overlapSphereRadius = 2f;
    [SerializeField] private float _raycastLength = 3f;
    [SerializeField] private LayerMask _boxLayer;

    [Header("Movement Changes On Box Pickup")]
    [SerializeField][Min(0)] private float _newRideHeight = 2.3f;
    [SerializeField] private float _newSpeed = 2f;
    [SerializeField] private float _newJumpHeight = 2f;

    private IInputSource _inputSource;
    private IInputSource _defaultInputSource;
    private Hover _hover;
    private InputBasedHoverMovement _movement;

    private GameObject _currentBox;
    private Rigidbody _currentBoxRB;

    private float _defaultRideHeight;
    private float _defaultSpeed;
    private float _defaultJumpHeight;

    private bool _isHoldingBox;


    private void Awake()
    {
        _hover = GetComponent<Hover>();
        _defaultRideHeight = _hover._rideHeight;
        _movement = GetComponent<InputBasedHoverMovement>();
        _defaultSpeed = _movement._maxSpeed;
        _defaultJumpHeight = _movement._jumpHeight;

        _defaultInputSource = GetComponent<IInputSource>();
        _inputSource = _defaultInputSource;
    }

    private void FixedUpdate()
    {
        if (_currentBox == null && _currentBoxRB == null && !_isHoldingBox)
        {
            CheckForBoxes();
        }
        else if (_currentBox != null && _currentBoxRB != null && _isHoldingBox)
        {
            _currentBoxRB.position = _holder.position;

            if(_inputSource.Action2Pressed)
            {
                ReleaseBox();
            }
        }

    }

    private void CheckForBoxes()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastLength, _boxLayer))
        {
            if (hitInfo.transform.TryGetComponent<WoodenBox>(out var hitBox))
            {
                Debug.Log("Found Box");
                if (_inputSource.Action2Pressed)
                {
                    //Grab
                    _currentBox = hitBox.gameObject;
                    _currentBoxRB = _currentBox.GetComponent<Rigidbody>();

                    //TODO: for now this works but i want to make sure it cant go through walls
                    _currentBoxRB.isKinematic = true;
                    _currentBoxRB.position = _holder.position;

                    //change hover and movement values //TODO: this is a bit sensitive...
                    _hover._rideHeight = _newRideHeight;
                    _movement._maxSpeed = _newSpeed;
                    _movement._jumpHeight = _newJumpHeight;

                    _isHoldingBox = true;

                }
            }

        }

    }

    private void ReleaseBox()
    {
        Debug.Log("Releasing Box");
        _currentBoxRB.isKinematic = false;

        //change hover and movement values //TODO: this is a bit sensitive...
        _hover._rideHeight = _defaultRideHeight;
        _movement._maxSpeed = _defaultSpeed;
        _movement._jumpHeight = _defaultJumpHeight;

        _currentBox = null;
        _currentBoxRB = null;

        _isHoldingBox = false;
    }

    public void OnPossessed(Parasite playerParasite, IInputSource newInputSource)
    {
        _inputSource = newInputSource;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        if (_isHoldingBox)
        {
            ReleaseBox();
        }
        _inputSource = _defaultInputSource;
    }
}
