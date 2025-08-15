using UnityEngine;
using UnityEngine.Serialization;

public class DisableGFXOnDeath : MonoBehaviour, IDeathResponse
{
    [SerializeField] private GameObject _gfxToDisable;

    public void OnDeath()
    {
        _gfxToDisable.SetActive(false);
    }
}