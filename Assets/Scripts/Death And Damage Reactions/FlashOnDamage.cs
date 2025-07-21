using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChangeHandler))]
public class FlashOnDamage : MonoBehaviour, IDamageResponse
{
    [SerializeField] private Color _hitColor = Color.red;

    private ColorChangeHandler _colorChangeHandler;

    private void Awake()
    {
        _colorChangeHandler = GetComponent<ColorChangeHandler>();
    }

    public void OnDamage(float IFramesDuration)
    {
        _colorChangeHandler.ChangeColor(_hitColor, IFramesDuration);
    }

}
