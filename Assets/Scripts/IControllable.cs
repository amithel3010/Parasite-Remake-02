using UnityEngine;

public interface IControllable
{
    public void HandleAllMovement(Vector3 MoveAmount); // this might be too generic. I don't really understand how you do something like that when the types of movement are so different from eachother

    public void OnPossess();

    public void OnDePossess();
    //void HandleAllMovement();
} 

