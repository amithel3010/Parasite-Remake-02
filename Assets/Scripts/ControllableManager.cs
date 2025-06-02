using UnityEngine;

public class ControllableManager : MonoBehaviour
{
    //this script is responsible for translating player input into commands for the current controllable.

    public static ControllableManager instance;

    private ParasiteAbstractTest ParasiteControllable;  //Defualt Controller
    [SerializeField] private Possessable currentControllable;

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
        ParasiteControllable = FindAnyObjectByType<ParasiteAbstractTest>();
        currentControllable = ParasiteControllable;
        currentControllable.SetInputSource(input);
    }

    private void FixedUpdate() {
    }

    public void Possess(Possessable CreatureToPossess)
    {
        currentControllable = CreatureToPossess;
    }

}
