using Unity.Cinemachine;
using UnityEngine;

public class DebugStatsOverlay : MonoBehaviour
{
    private bool _showDebugStats;

    private CinemachineBrain _brain;
    private CinemachineCamera[] _cams;

    //[SerializeField] private Rigidbody _playerRB;

    void Awake()
    {
        if (Camera.main != null) _brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    void OnGUI()
    {
        if (!_showDebugStats) return;
        
        GUIContent content = new GUIContent(BuildDebugText());
        GUIStyle style = GUI.skin.box;

        // Calculate how big the box should be to fit the text
        Vector2 size = style.CalcSize(content);

        // Add some vertical room since there's more than one line
        size.y += 100;

        Rect rect = new Rect(100, 500, size.x + 20, size.y); // +20 for padding

        GUILayout.BeginArea(rect, GUI.skin.box);

        GUILayout.Label(content);

        GUILayout.EndArea();

    }

    private string BuildDebugText()
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine();
        sb.AppendLine($" Brain Live Camera: {_brain?.ActiveVirtualCamera?.Name ?? "None"}");

        sb.AppendLine(" Active Cameras:");

        _cams = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
        foreach (var cam in _cams)
        {
            if (cam.isActiveAndEnabled && cam.transform.parent.gameObject.activeSelf)
                sb.AppendLine($" {cam.name}");
        }

        //if (_playerRB != null)
        //{
        //    sb.AppendLine();
        //    sb.AppendLine($"Player RB Velocity:{_playerRB.linearVelocity.magnitude:F3} in direction: {_playerRB.linearVelocity.normalized}");
        //}
        return sb.ToString();
    }

    public void ToggleDebugStats()
    {
        _showDebugStats = !_showDebugStats;
    }

}
