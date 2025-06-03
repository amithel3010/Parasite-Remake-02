using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //responsible for keeping track of currently possessed possessable
    //and giving them input from the player

    //TODO: maybe should be a singleton? no reason for 2 of these

    [SerializeField] InputHandler inputHandler;
    [SerializeField] Possessable currentPossessable;

    void Start()
    {
        if (currentPossessable == null)
        {
            Debug.LogError("No Possessable On Start");
        }
        else
        {
            print("Possessable on Start:" + currentPossessable); //Should always be Parasite.
        }

        currentPossessable.SetInputSource(inputHandler); //Feels weird calling this from here... feels like it should be in OnPossessed but i dont know how to do that
    }

    public void Possess(Possessable newPossessable)
    {
        currentPossessable.OnDepossessed(); //here we would set the input source of the old possessable back to AI if it should live
        currentPossessable = newPossessable;
        currentPossessable.OnPossessed(); 
        currentPossessable.SetInputSource(inputHandler);
    }

}
