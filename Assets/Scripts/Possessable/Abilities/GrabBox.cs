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
    [SerializeField] private float _sphereRadius = 0.3f;
    [SerializeField] private LayerMask _boxLayer;

    [Header("Movement Changes On Box Pickup")]
    [SerializeField][Min(0)] private float _rideHeightChange = 1f;
    [SerializeField][Max(0)] private float _maxSpeedChange = -6f;
    [SerializeField][Max(0)] private float _jumpHeightChange = -3f;

    [Header("Debug")]
    //[SerializeField] private bool _showRaycast;
    [SerializeField] private bool _showOverlapSphere;

    private IInputSource _inputSource;
    private IInputSource _defaultInputSource;
    private Hover _hover;
    private InputBasedHoverMovement _movement;
    private BaseJumpAbility _jumpAbility;

    private GameObject _currentBox;
    private Rigidbody _currentBoxRB;

    private bool _isHoldingBox;


    private void Awake()
    {
        _hover = GetComponent<Hover>();
        _movement = GetComponent<InputBasedHoverMovement>();
        _jumpAbility = GetComponent<BaseJumpAbility>();

        _defaultInputSource = GetComponent<IInputSource>();
        _inputSource = _defaultInputSource;
    }

    private void FixedUpdate()
    {
        if (_currentBox == null && _currentBoxRB == null && !_isHoldingBox)
        {
            CheckForBoxesWithOverlapSphere();
        }
        else if (_currentBox != null && _currentBoxRB != null && _isHoldingBox)
        {
            _currentBoxRB.position = _holder.position;

            if (_inputSource.Action2Pressed)
            {
                ReleaseBox();
            }
        }

    }

    private void CheckForBoxesWithOverlapSphere()
    {
        if (_inputSource.Action2Pressed)
        {
            Collider[] hitColliders = Physics.OverlapSphere(_holder.position, _sphereRadius, _boxLayer);
            foreach (var collider in hitColliders)
            {
                if (collider.transform.parent.gameObject.TryGetComponent<WoodenBox>(out var hitBox))
                {
                    Debug.Log("Found Box");
                    //Grab
                    _currentBox = hitBox.gameObject;
                    _currentBoxRB = _currentBox.GetComponent<Rigidbody>();

                    //TODO: for now this works but i want to make sure it cant go through walls
                    _currentBoxRB.isKinematic = true;
                    _currentBoxRB.detectCollisions = false;
                    _currentBoxRB.position = _holder.position;

                    _hover.ChangeRideHeight(_rideHeightChange);
                    _movement.ChangeMovementParams(_maxSpeedChange);
                    _jumpAbility.ChangeJumpHeight(_jumpHeightChange);

                    _isHoldingBox = true;

                    return;

                }
            }
        }
    }

    private void ReleaseBox()
    {
        Debug.Log("Releasing Box");
        _currentBoxRB.isKinematic = false;
        _currentBoxRB.detectCollisions = true;

        _hover.ResetRideHeight();
        _movement.ResetMovementParams();
        _jumpAbility.ResetJumpHeight();

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

    private void OnDrawGizmos()
    {
        if (_showOverlapSphere)
        {
            Gizmos.color = Color.cadetBlue;
            Gizmos.DrawWireSphere(_holder.position, _sphereRadius);
        }
    }
}
