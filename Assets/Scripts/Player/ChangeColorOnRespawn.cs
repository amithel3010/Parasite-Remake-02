using UnityEngine;

public class ChangeColorOnRespawn : MonoBehaviour, IPlayerRespawnListener
{
    private Renderer _renderer;
    private Color _defaultColor;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _defaultColor = _renderer.material.GetColor("_BaseColor");
    }

    public void OnPlayerRespawn()
    {
        _renderer.material.SetColor("_BaseColor", _defaultColor);
    }
}
