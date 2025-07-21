using UnityEngine;

[RequireComponent(typeof(ColorChangeHandler))]
public class ChangeColorOnDeath : MonoBehaviour, IDeathResponse
{
    [SerializeField] private Color _deathColor = Color.black;

    private ColorChangeHandler _handler;

    private void Awake()
    {
        _handler = GetComponent<ColorChangeHandler>();
    }

    public void OnDeath()
    {
        _handler.ChangeColor(_deathColor);
    }
}
