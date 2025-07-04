using System.Collections;
using UnityEngine;

public class PossessableHealthHandler : MonoBehaviour
{
    private Health _health;

    private Renderer _renderer;
    private Color _defaultColor;
    private Color _hitColor = Color.red;

    void Awake()
    {
        _health = GetComponent<Health>();
        _renderer = GetComponentInChildren<Renderer>();
        _defaultColor = _renderer.material.GetColor("_BaseColor");

        _health.OnDamaged += HandleDamage;
        _health.OnDeath += HandleDeath;
    }

    private void HandleDamage(float IFramesDuration)
    {
        Debug.Log(this + "possessable got hit, it's current health is now" + _health.CurrentHealth);
        StartCoroutine(DamageFlash(IFramesDuration));
        //play animation
    }

    private void HandleDeath()
    {
        Debug.Log(this + "has died");
        //animation
        StopAllCoroutines();
        _renderer.material.SetColor("_BaseColor", Color.black);
        Destroy(gameObject, 1f);
    }

    private IEnumerator DamageFlash(float IFramesDuration)
    {
        _renderer.material.SetColor("_BaseColor", _hitColor);
        yield return new WaitForSeconds(IFramesDuration);
        _renderer.material.SetColor("_BaseColor", _defaultColor);
        //cool unitended bug makes color black on death
    }
}
