using UnityEngine;

public interface IControllable
{
    public void HandleAllMovement(Vector2 MoveInput, bool jumpPressed);

    public void OnPossess();

    public void OnDePossess();

} 

