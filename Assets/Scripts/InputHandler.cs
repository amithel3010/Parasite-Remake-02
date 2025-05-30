using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public float horizontalInput;
    public float verticalInput;
    public bool jumpPressed;
    public bool jumpHeld;
    public bool actionPressed;
    public bool actionHeld;
    public Vector3 MovementInput;

    bool readyToClear;

    void Update()
    {
        ClearInputs();

        ProcessInputs();

        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
        verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);
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

        MovementInput = new Vector2(horizontalInput, verticalInput);
    }
}
