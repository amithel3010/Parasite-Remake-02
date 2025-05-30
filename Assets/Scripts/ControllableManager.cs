using UnityEngine;

public class ControllableManager : MonoBehaviour
{
    //this script is responsible for translating player input into commands for the current controllable.

    private IControllable defaultControllable;
    private IControllable currentControllable; //would really like a way to see this in inspector

    [SerializeField] private InputHandler input;

    Vector3 moveInput;

    void Start()
    {
        defaultControllable = FindAnyObjectByType<ParasiteControllable>();
        currentControllable = defaultControllable;
    }

    void FixedUpdate()
    {
        if (input.actionPressed && currentControllable != defaultControllable)
        {
            currentControllable.OnDePossess();
            currentControllable = defaultControllable;
        }

        currentControllable.HandleAllMovement(input.MovementInput);
    }


}
