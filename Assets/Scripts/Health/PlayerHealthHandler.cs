using System.Collections;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IPossessionSource, IPlayerRespawnListener
{

    [SerializeField] private float _timeBeforeGameOver;

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
        StartCoroutine(WaitBeforeGameOver(_timeBeforeGameOver));

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

    public void OnPlayerRespawn()
    {
        _health.ResetHealth();
    }

    private IEnumerator WaitBeforeGameOver(float _timeBeforeGameOver)
    {
        yield return new WaitForSeconds(_timeBeforeGameOver);
        GameManager.Instance.GameOver();
    }
}
