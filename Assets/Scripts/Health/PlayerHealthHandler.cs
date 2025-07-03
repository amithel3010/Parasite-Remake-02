using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    private Health _health;
    private KnockbackTest _knockback; //TODO: this feels coupled. i use this to disable knockback on death

    //on hit Iframes, visual flickering, cant possess take the same time

    void Awake()
    {
        _health = GetComponent<Health>();
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
        //TODO: visual indication for IFRAMES
        //mostly visual stuff
        //Knockback happens in attacker
    }

    public void HandleRespawn() //TODO: called in respawn in game manager, is this fine?
    {
        _knockback.KnockbackEnabled = true;
    }
}
