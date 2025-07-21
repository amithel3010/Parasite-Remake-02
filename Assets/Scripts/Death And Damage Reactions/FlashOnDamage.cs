using System.Collections;
using UnityEngine;

public class FlashOnDamage : MonoBehaviour, IDamageResponse
{
    //BUG: when dies with die immedietly, change color on damage overwrites changecolor on death
    //potential fix: merge into one component

    [SerializeField] private Color _hitColor = Color.red;

    private Renderer _renderer;
    private Color _defaultColor;

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
