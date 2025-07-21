using UnityEngine;

[RequireComponent(typeof(ColorChangeHandler))]
public class ChangeColorOnRespawn : MonoBehaviour, IPlayerRespawnListener
{
    private ColorChangeHandler _handler;

    private void Awake()
    {
        _handler = GetComponent<ColorChangeHandler>();
    }

    public void OnPlayerRespawn()
    {
        _handler.ResetColor();
    }
}
