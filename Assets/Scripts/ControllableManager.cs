using UnityEngine;

public class ControllableManager : MonoBehaviour
{
    //this script is responsible for translating player input into commands for the current controllable.
    
    [SerializeField] private IControllable currentControllable;
    [SerializeField] private InputHandler input;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentControllable.HandleAllMovement();
    }
}
