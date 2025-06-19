using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    private IDamagable _health;

    void Awake()
    {
        _health = GetComponent<IDamagable>();
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
}
