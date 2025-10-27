using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class ActivatePossessionTheme : MonoBehaviour, IPossessionSource, IPlayerRespawnListener
{
   [SerializeField] AudioMixerSnapshot _chillSnapshot;
   [SerializeField] private AudioMixerSnapshot _possessionSnapshot;
   
   [Header("Settings")]
   [SerializeField] private float _duration;


    public void OnParasitePossession()
    {
        _possessionSnapshot.TransitionTo(_duration);
    }

    public void OnParasiteUnPossession()
    {
        _chillSnapshot.TransitionTo(_duration);
    }

    public void OnPlayerRespawn()
    {
        _chillSnapshot.TransitionTo(_duration);
    }
}
