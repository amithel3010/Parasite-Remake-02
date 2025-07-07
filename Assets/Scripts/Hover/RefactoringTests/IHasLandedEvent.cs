using System;
using UnityEngine;

public interface IHasLandedEvent
{
    public event Action OnLanding;
}
