using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //responsible for keeping track of currently possessed possessable
    //and setting their input to be from the player

    //TODO: maybe should be a singleton? no reason ever for 2 of these
    //TODO: maybe setinputsource should be here?

    [SerializeField] InputHandler inputHandler;
    [SerializeField] IControllable currentControllable;

    void Start()
    {
        
       currentControllable = FindAnyObjectByType<ParasitePossessable>(FindObjectsInactive.Exclude);
    }

    private void FixedUpdate()
    {
        currentControllable.OnFixedUpdate();
    }



}
