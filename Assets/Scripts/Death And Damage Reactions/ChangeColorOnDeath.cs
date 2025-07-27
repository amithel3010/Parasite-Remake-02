using UnityEngine;

public class ChangeColorOnDeath : MonoBehaviour, IDeathResponse
{
    [SerializeField] private Color _deathColor = Color.black;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    public void OnDeath()
    {
        _renderer.material.SetColor("_BaseColor", _deathColor);
    }
}
