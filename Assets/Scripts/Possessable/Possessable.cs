using Unity.Mathematics;
using UnityEngine;


public abstract class Possessable : Hover
{

    public void SetInputSource(IInputSource inputSource)
    {
        base.InputSource = inputSource;
    }
}
