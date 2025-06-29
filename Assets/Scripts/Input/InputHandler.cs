using UnityEngine;

public class InputHandler : MonoBehaviour, IInputSource
{
    //TODO: needs to always have ONLY 1 target. is there a way to make sure of that?
    //maybe could be a singleton. also might not need to be on player.

    public float _horizontalInput;
    public float _verticalInput;
    public bool _jumpPressed;
    public bool _jumpHeld;
    public bool _actionPressed;
    public bool _actionHeld;
    public bool _action2Pressed;
    public Vector2 _movementInputs;

    public bool _debugPressed;

    bool readyToClear;
    bool IInputSource.JumpPressed => _jumpPressed;
    bool IInputSource.JumpHeld => _jumpHeld;
    bool IInputSource.ActionPressed => _actionPressed;
    bool IInputSource.ActionHeld => _actionHeld;
    bool IInputSource.Action2Pressed => _action2Pressed;
    Vector2 IInputSource.MovementInput => _movementInputs;

    void Update()
    {
        ClearInputs();

        ProcessInputs();
    }

    private void FixedUpdate()
    {
        readyToClear = true;
    }

    private void ClearInputs()
    {
        if (!readyToClear)
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

        _debugPressed = false;

        readyToClear = false;
    }

    private void ProcessInputs()
    {
        _horizontalInput += Input.GetAxisRaw("Horizontal");
        _verticalInput += Input.GetAxisRaw("Vertical");

        _jumpPressed = _jumpPressed || Input.GetButtonDown("Jump");
        _jumpHeld = _jumpHeld || Input.GetButton("Jump");

        _actionPressed = _actionPressed || Input.GetKeyDown(KeyCode.E);
        _actionHeld = _actionHeld || Input.GetKey(KeyCode.E);

        _action2Pressed = _action2Pressed || Input.GetKeyDown(KeyCode.F);

        _debugPressed = _debugPressed || Input.GetKeyDown(KeyCode.Alpha0);

        _horizontalInput = Mathf.Clamp(_horizontalInput, -1f, 1f); //used to be in update is it ok here?
        _verticalInput = Mathf.Clamp(_verticalInput, -1f, 1f);

        _movementInputs = new Vector2(_horizontalInput, _verticalInput);
    }
}
