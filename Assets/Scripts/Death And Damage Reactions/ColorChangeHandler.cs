using System.Collections;
using UnityEngine;

public class ColorChangeHandler : MonoBehaviour
{

    /// <summary>
    /// Class responsible for coordinating multiple color changes on the same object.
    /// </summary>

    private Renderer _renderer;
    private Material _material;
    private Color _defaultColor;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    
    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer == null)
        {
            Debug.Log(this + "Has No Renderer in children");
        }
        else
        {
            _defaultColor = _renderer.material.GetColor(BaseColor);
            _material = _renderer.material;
        }
    }

    /// <summary>
    /// Change color for a set duration in seconds.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="duration"></param>
    public void ChangeColor(Color color, float duration)
    {
        StopAllCoroutines();
        _material.SetColor(BaseColor, _defaultColor);
        StartCoroutine(ChangeColorCoroutine(color, duration));
    }

    /// <summary>
    /// Change color without time limit.
    /// </summary>
    /// <param name="color"></param>
    public void ChangeColor(Color color)
    {
        StopAllCoroutines();
        _material.SetColor(BaseColor, color);
    }

    public void ResetColor()
    {
        StopAllCoroutines();
        _material.SetColor(BaseColor, _defaultColor);
    }

    private IEnumerator ChangeColorCoroutine(Color colorToChangeTo, float duration)
    {
        _renderer.material.SetColor(BaseColor, colorToChangeTo);
        yield return new WaitForSeconds(duration);
        _renderer.material.SetColor(BaseColor, _defaultColor);
    }

}
