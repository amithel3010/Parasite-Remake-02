using UnityEngine;

public class InputHandler : MonoBehaviour, IInputSource
{
    //TODO: needs to always have ONLY 1 target. is there a way to make sure of that?
    //maybe could be a singleton. also might not need to be on player.
    
    //TODO: camera.main is expensive

    private float _horizontalInput;
    private float _verticalInput;
    private bool _jumpPressed;
    private bool _jumpHeld;
    private bool _actionPressed;
    private bool _actionHeld;
    private bool _action2Pressed;
    
    private Vector2 _movementInputs;
    private Vector3 _horizontalMovementVector;

    public bool DebugPressed { get; private set; }

    
    private Camera _mainCamera;
    private Vector3 _adjustToCameraTest;

    private bool _readyToClear;
    
    bool IInputSource.JumpPressed => _jumpPressed;
    bool IInputSource.JumpHeld => _jumpHeld;
    bool IInputSource.ActionPressed => _actionPressed;
    bool IInputSource.ActionHeld => _actionHeld;
    bool IInputSource.Action2Pressed => _action2Pressed;
    Vector2 IInputSource.MovementInput => _movementInputs;
    Vector3 IInputSource.HorizontalMovementVector => _horizontalMovementVector;

    void Awake()
    {
        //_mainCamera = Camera.main;
    }
    void Update()
    {
        ClearInputs();

        ProcessInputs();
    }

    private void FixedUpdate()
    {
        _readyToClear = true;
    }

    private void ClearInputs()
    {
        if (!_readyToClear)
        {
            return;
        }

        _horizontalInput = 0f;
        _verticalInput = 0f;
        _jumpPressed = false;
        _jumpHeld = false;
        _actionPressed = false;
        _actionHeld = false;
        _action2Pressed = false;

        DebugPressed = false;

        _readyToClear = false;
    }

    private void ProcessInputs()
    {
        _horizontalInput += Input.GetAxisRaw("Horizontal");
        _verticalInput += Input.GetAxisRaw("Vertical");

        _jumpPressed = _jumpPressed || Input.GetButtonDown("Jump");
        _jumpHeld = _jumpHeld || Input.GetButton("Jump");

        _actionPressed = _actionPressed || Input.GetKeyDown(KeyCode.X);
        _actionHeld = _actionHeld || Input.GetKey(KeyCode.X);

        _action2Pressed = _action2Pressed || Input.GetKeyDown(KeyCode.C);

        DebugPressed = DebugPressed || Input.GetKeyDown(KeyCode.Alpha0);

        _horizontalInput = Mathf.Clamp(_horizontalInput, -1f, 1f); //used to be in update is it ok here?
        _verticalInput = Mathf.Clamp(_verticalInput, -1f, 1f);

        _movementInputs = new Vector2(_horizontalInput, _verticalInput);
        //_HorizontalMovement = new Vector3(_movementInputs.x, 0f, _movementInputs.y);
        //_horizontalMovementVector = AdjustToCamera(_movementInputs);
        _horizontalMovementVector = AdjustToCamera(_movementInputs);
    }
    
    private Vector3 AdjustToCamera(Vector2 input)
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
    
        if (_mainCamera == null)
            return Vector3.zero;

        // Get camera forward and right directions
        Vector3 camForward = _mainCamera.transform.forward;
        Vector3 camRight = _mainCamera.transform.right;

        // Project onto horizontal plane
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Combine directions with input
        Vector3 adjusted = camForward * input.y + camRight * input.x;
        return adjusted;
    }


}
