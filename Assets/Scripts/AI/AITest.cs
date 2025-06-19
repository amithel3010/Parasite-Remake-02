using UnityEngine;

public class AITest : MonoBehaviour, IInputSource
{
    public bool JumpPressed => false;

    public bool JumpHeld => false;

    public bool ActionPressed => false;

    public bool ActionHeld => false;

    public bool Action2Pressed => false;

    public Vector2 MovementInput => Vector2.zero;

}
