using Unity.Cinemachine;
using UnityEngine;

public class DebugHUD : MonoBehaviour
{
    public Transform player;
    public CinemachineBrain brain;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 500, 300), GUI.skin.box);

        GUILayout.Label($"Active Camera: {brain?.ActiveVirtualCamera?.Name ?? "None"}");

        if (player != null)
            GUILayout.Label($"Player Speed: {player.GetComponent<Rigidbody>().linearVelocity.magnitude:F2}");

        GUILayout.EndArea();
    }
}
