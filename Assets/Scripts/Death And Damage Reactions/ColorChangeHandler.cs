using System.Collections;
using UnityEngine;

public class ColorChangeHandler : MonoBehaviour
{
    /// <summary>
    /// Class responsible for coordinating multiple color changes on the same object.
    /// </summary>
    private MeshRenderer[] _renderers;

    private Material[] _materials;
    private Color[] _defaultColors;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
        if (_renderers.Length == 0)
        {
            Debug.Log(this + "Has No Renderer in children");
        }
        else
        {
            _defaultColors = new Color[_renderers.Length];
            _materials = new Material[_renderers.Length];

            for (var i = 0; i < _renderers.Length; i++)
            {
                if (!_renderers[i].material.HasColor(BaseColor)) continue;
                
                _defaultColors[i] = _renderers[i].material.GetColor(BaseColor);
                _materials[i] = _renderers[i].material;
            }
        }
    }

    /// <summary>
    /// Change color for a set duration in seconds.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="duration"></param>
    public void ChangeColor(Color color, float duration)
    {
        ResetColor();
        StartCoroutine(ChangeColorCoroutine(color, duration));
    }

    /// <summary>
    /// Change color without time limit.
    /// </summary>
    /// <param name="color"></param>
    public void ChangeColor(Color color)
    {
        StopAllCoroutines();
        foreach (var material in _materials)
        {
            material.SetColor(BaseColor, color);
        }
    }

    public void ResetColor()
    {
        StopAllCoroutines();
        for (var i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor(BaseColor, _defaultColors[i]);
        }
    }

    private IEnumerator ChangeColorCoroutine(Color colorToChangeTo, float duration)
    {
        foreach (var material in _materials)
        {
            material.SetColor(BaseColor, colorToChangeTo);
        }

        yield return new WaitForSeconds(duration);
        for (var i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor(BaseColor, _defaultColors[i]);
        }
    }
}