using UnityEngine;

public interface IControllable
{
    public void HandleAllMovement(Vector2 MoveInput, bool jumpPressed);

    //TODO: maybe parasite should not inherent from this interface, as it doesnt need OnPossess and On Depossess
    public void OnPossess();

    public void OnDePossess();

} 

