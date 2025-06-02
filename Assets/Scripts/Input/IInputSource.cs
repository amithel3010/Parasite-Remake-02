using UnityEngine;

public interface IInputSource
{
    public bool JumpPressed{ get; }
    public bool JumpHeld{ get; }
    public bool ActionPressed{ get; }
    public bool ActionHeld{ get; }
    public Vector2 MovementInput{ get; }
}
