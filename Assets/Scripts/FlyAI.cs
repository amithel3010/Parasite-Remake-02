using UnityEngine;

public class FlyAI : MonoBehaviour, IInputSource
{
    Possessable possessable;

    public bool JumpPressed => false;
    public bool JumpHeld => false;
    public bool ActionPressed => false;
    public bool ActionHeld => false;
    public Vector2 MovementInput => Vector2.zero;


    void Start()
    {
        possessable = GetComponent<Possessable>();
        possessable.SetInputSource(this);
    }

    void FixedUpdate()
    {
        possessable.OnFixedUpdate();
    }

    //where is the logic for the inputs????
}
