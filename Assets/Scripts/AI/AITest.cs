using UnityEngine;

public class AITest : MonoBehaviour, IInputSource
{
    [SerializeField] bool _action2Pressed = false; //for debugging

    public bool JumpPressed => false;

    public bool JumpHeld => false;

    public bool ActionPressed => false;

    public bool ActionHeld => false;

    public bool Action2Pressed => _action2Pressed;

    public Vector2 MovementInput => Vector2.zero;

    public Vector3 HorizontalMovement => new Vector3(MovementInput.x, 0f, MovementInput.y);
}
