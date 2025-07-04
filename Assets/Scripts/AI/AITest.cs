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

}
