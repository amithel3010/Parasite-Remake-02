using UnityEngine;

public interface IPossessionSensitive
{
    public void OnPossessed(Parasite playerParasite, IInputSource inputSource);

    public void OnUnPossessed(Parasite playerParasite);
}
