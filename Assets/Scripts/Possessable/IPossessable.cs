using UnityEngine;

public interface IPossessable
{
    public void OnPossess(IInputSource inputSource, Parasite parasite);

    public void OnUnPossess();
}
