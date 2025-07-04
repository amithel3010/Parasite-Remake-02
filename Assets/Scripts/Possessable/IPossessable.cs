using UnityEngine;

public interface IPossessable
{
    //TODO: currently used only in Possessable. defeats purpose
    //if i had calssess like flypossessable and brutepossessable it would be useful.
    public void OnPossess(IInputSource inputSource);

    public void OnUnPossess();
}
