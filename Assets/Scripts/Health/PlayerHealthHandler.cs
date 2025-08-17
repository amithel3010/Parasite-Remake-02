using System.Collections;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IPossessionSource, IPlayerRespawnListener
{

    [SerializeField] private float _timeBeforeGameOver;

    private Health _health;
    private IDamageResponse[] _damageResponsers;
    private IDeathResponse[] _deathResponses;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _damageResponsers = GetComponents<IDamageResponse>(); //does this properly catch all damage responses?
        _deathResponses = GetComponents<IDeathResponse>(); 
    }

    private void OnEnable()
    {
        _health.OnDamaged += HandleDamage;
        _health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnDamaged -= HandleDamage;
        _health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        Debug.Log("Player Died!");
        //for each IDeathReactor ReactToDeath().
        foreach (var deathResponse in _deathResponses)
        {
            deathResponse.OnDeath();
        }
        //play animation,
        //stop control,
        //show game over screen
        StartCoroutine(WaitBeforeGameOver(_timeBeforeGameOver));

    }

    private void HandleDamage(float iFramesDuration)
    {
        Debug.Log("player got hit");
        
        foreach (var damageResponse in _damageResponsers)
        {
            damageResponse.OnDamage(iFramesDuration);
        }

        //play animation
        //mostly visual stuff

    }

    public void OnParasitePossession()
    {
        _health.ResetHealth();
        _health.SetDead(false);
    }

    public void OnParasiteUnPossession()
    {
        //nothing required
    }

    public void OnPlayerRespawn()
    {
        _health.ResetHealth();
        
    }

    private IEnumerator WaitBeforeGameOver(float timeBeforeGameOver)
    {
        yield return new WaitForSeconds(timeBeforeGameOver);
        GameManager.Instance.GameOver();
    }
}
