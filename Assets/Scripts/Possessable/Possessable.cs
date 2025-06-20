using UnityEngine;

public class Possessable : MonoBehaviour, IPossessable
{
    //private PhysicsBasedController _controller;
    private HoveringCreatureController _controller; //might need to make this an interface

    void Awake()
    {
        _controller = GetComponent<HoveringCreatureController>();
    }

    public void OnPossess(IInputSource inputSource, Parasite parasite)
    {
        _controller.OnPossess(inputSource, parasite);
    }

    public void OnUnPossess()
    {
        _controller.OnUnPossess();
    }
}
