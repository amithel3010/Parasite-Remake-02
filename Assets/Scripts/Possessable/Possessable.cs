using UnityEngine;

public class Possessable : MonoBehaviour, IPossessable
{
    private PhysicsBasedController _controller;

    void Awake()
    {
        _controller = GetComponent<PhysicsBasedController>();
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
