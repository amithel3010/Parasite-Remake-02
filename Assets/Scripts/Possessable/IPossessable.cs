using UnityEngine;

public interface IPossessable
{
    public void OnPossess(IInputSource inputSource);

    public void OnUnPossess();
}
