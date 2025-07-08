using System.Collections;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IPossessionSource
{
    private Health _health;
    private KnockbackTest _knockback; //TODO: this feels coupled. i use this to disable knockback on death

    private Renderer _renderer;
    private Color _defaultColor;
    private Color _hitColor = Color.red;
    //on hit Iframes, visual flickering, cant possess take the same time

    void Awake()
    {
        _health = GetComponent<Health>();
        _knockback = GetComponent<KnockbackTest>();

        _renderer = GetComponentInChildren<Renderer>();
        _defaultColor = _renderer.material.GetColor("_BaseColor");
    }

    void OnEnable()
    {
        _health.OnDamaged += HandleDamage;
        _health.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        _health.OnDamaged -= HandleDamage;
        _health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        Debug.Log("Player Died!");
        //play animation,
        //stop control,
        _knockback.KnockbackEnabled = false;
        //show game over screen
        GameManager.Instance.GameOver();
    }

    private void HandleDamage(float IFramesDuration)
    {
        Debug.Log("player got hit");
        //play animation
        StartCoroutine(DamageFlash(IFramesDuration));
        //mostly visual stuff
        //Knockback happens in attacker
    }

    public void HandleRespawn() //TODO: called in respawn in game manager, is this fine?
    {
        _knockback.KnockbackEnabled = true;
    }

    private IEnumerator DamageFlash(float IFramesDuration)
    {
        _renderer.material.SetColor("_BaseColor", _hitColor);
        yield return new WaitForSeconds(IFramesDuration);
        _renderer.material.SetColor("_BaseColor", _defaultColor);
    }

    public void OnParasitePossession()
    {
        _health.ResetHealth();
    }

    public void OnParasiteUnPossession()
    {
        //nothing required
    }
}
