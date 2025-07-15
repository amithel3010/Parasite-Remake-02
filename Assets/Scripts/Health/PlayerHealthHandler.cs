using System.Collections;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IPossessionSource
{
    private Health _health;

    void Awake()
    {
        _health = GetComponent<Health>();
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
        //for each IDeathReactor ReactToDeath().
        foreach (var deathResponse in GetComponents<IDeathResponse>())
        {
            deathResponse.OnDeath();
        }
        //play animation,
        //stop control,
        //show game over screen
        GameManager.Instance.GameOver();
    }

    private void HandleDamage(float IFramesDuration)
    {
        Debug.Log("player got hit");
        foreach (var damageResponse in GetComponents<IDamageResponse>())
        {
            damageResponse.OnDamage(IFramesDuration);
        }

        //play animation
        //mostly visual stuff
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
