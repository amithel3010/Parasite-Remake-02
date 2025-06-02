using UnityEngine;

public class ControllableManager : MonoBehaviour
{
    //this script is responsible for translating player input into commands for the current controllable.

    private static ControllableManager instance;

    private IControllable ParasiteControllable;  //Defualt Controller
    private IControllable currentControllable; 

    [SerializeField] private InputHandler input; // should be singleton?

    void Awake()
    {
        if (instance = null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        ParasiteControllable = FindAnyObjectByType<ParasiteControllable>();
        currentControllable = ParasiteControllable;
    }

    void FixedUpdate()
    {
        if (input.actionPressed )
        {
            if (currentControllable == ParasiteControllable)
            {
                currentControllable.OnDePossess();
                currentControllable = FindAnyObjectByType<FlyControllable>();
                currentControllable.OnPossess();
            }

            else if (currentControllable != ParasiteControllable)
            {
                currentControllable.OnDePossess();
                currentControllable = ParasiteControllable;
                currentControllable.OnPossess();
            }
        }

        currentControllable.HandleAllMovement(input.MovementInput, input.jumpPressed);
    }


}
