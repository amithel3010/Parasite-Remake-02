using Unity.Cinemachine;
using UnityEngine;

public class DebugHUD : MonoBehaviour
{
    private Transform _player;
    private CinemachineBrain _brain;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _player = GameObject.FindAnyObjectByType<Parasite>().transform;
        _rigidbody = _player.GetComponent<Rigidbody>();
        if (Camera.main != null) _brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 500, 300), GUI.skin.box);

        GUILayout.Label($"Active Camera: {_brain?.ActiveVirtualCamera?.Name ?? "None"}");

        if (_player != null)
            GUILayout.Label($"Player Speed: {_rigidbody.linearVelocity.magnitude:F2}");

        GUILayout.EndArea();
    }
}
