using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
            print("Good to go");
        }

        currentPossessable.SetInputSource(inputHandler); //Feels weird calling this from here... feels like it should be in OnPossessed but i dont know how to do that
    }

    public void Possess(Possessable newPossessable)
    {
        currentPossessable.OnDepossessed(); //here we set the input source of the old possessable back to AI if it should live
        currentPossessable = newPossessable;
        currentPossessable.OnPossessed(); 
        currentPossessable.SetInputSource(inputHandler);
    }

}
