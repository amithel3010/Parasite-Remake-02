using System.Collections;
using UnityEngine;

public class FlashOnDamage : MonoBehaviour, IDamageResponse
{

    private Renderer _renderer;
    private Color _defaultColor;

       [SerializeField] private Color _hitColor = Color.red;

    public void OnDamage(float IFramesDuration)
    {
        Flash(IFramesDuration);
    }

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _defaultColor = _renderer.material.GetColor("_BaseColor");
    }

    private IEnumerator FlashRoutine(float duration)
    {
        _renderer.material.SetColor("_BaseColor", _hitColor);
        yield return new WaitForSeconds(duration);
        _renderer.material.SetColor("_BaseColor", _defaultColor);
    }
    private void Flash(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine(duration));
    }

}
