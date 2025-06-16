using UnityEngine;

public class InputHandler : MonoBehaviour, IInputSource
{
    //TODO: needs to always have ONLY 1 target. is there a way to make sure of that?

    public float horizontalInput;
    public float verticalInput;
    public bool jumpPressed;
    public bool jumpHeld;
    public bool actionPressed;
    public bool actionHeld;
    public bool action2Pressed;
    public Vector2 movementInput;

    bool readyToClear;
    bool IInputSource.JumpPressed => jumpPressed;
    bool IInputSource.JumpHeld => jumpHeld;
    bool IInputSource.ActionPressed => actionPressed;
    bool IInputSource.ActionHeld => actionHeld;
    bool IInputSource.Action2Pressed => action2Pressed;
    Vector2 IInputSource.MovementInput => movementInput;

    void Update()
    {
        ClearInputs();

        if (GameManager.Instance.IsPaused) return;
        
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

        horizontalInput = 0f;
        verticalInput = 0f;
        jumpPressed = false;
        jumpHeld = false;
        actionPressed = false;
        actionHeld = false;
        action2Pressed = false;

        readyToClear = false;
    }

    private void ProcessInputs()
    {
        horizontalInput += Input.GetAxisRaw("Horizontal");
        verticalInput += Input.GetAxisRaw("Vertical");

        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld = jumpHeld || Input.GetButton("Jump");

        actionPressed = actionPressed || Input.GetKeyDown(KeyCode.E);
        actionHeld = actionHeld || Input.GetKey(KeyCode.E);

        action2Pressed = action2Pressed || Input.GetKeyDown(KeyCode.F);

        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f); //used to be in update is it ok here?
        verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);

        movementInput = new Vector2(horizontalInput, verticalInput);
    }
}
