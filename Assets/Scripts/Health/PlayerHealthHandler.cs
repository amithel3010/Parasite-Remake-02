using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    private IDamagable _health;
    private KnockbackTest _knockback; //TODO: this feels coupled. i use this to disable knockback on death

    void Awake()
    {
        _health = GetComponent<IDamagable>();
        _knockback = GetComponent<KnockbackTest>();
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

    private void HandleDamage()
    {
        Debug.Log("player got hit");
        //play animation
        //mostly visual stuff
        //Knockback happens in attacker
    }

    public void HandleRespawn() //TODO: called in respawn in game manager, is this fine?
    {
        _knockback.KnockbackEnabled = true;
    }
}
