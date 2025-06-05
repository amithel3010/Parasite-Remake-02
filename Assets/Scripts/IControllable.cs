using UnityEngine;

public interface IControllable
{
    public void OnFixedUpdate();

    public void OnMovementInput(Vector2 moveInput);

    public void OnJumpInput(bool jumpInput);

    public void OnActionInput(bool actionInput);
}
