using UnityEditor;
using UnityEngine;

public static class DebugUtils
{
    private static Mesh _circleMesh;

    public static void DrawCircle(Vector3 center, float radius, Color color, int segments = 32)
    {
        Gizmos.color = color;

        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    public static void DrawFilledCircle(Vector3 center, float radius, Material material, int segments = 64)
    {
        if (material == null) return;

        if (_circleMesh == null || _circleMesh.vertexCount != segments + 1)
        {
            _circleMesh = GenerateFilledCircleMesh(segments);
        }

        Matrix4x4 matrix = Matrix4x4.TRS(center, Quaternion.identity, Vector3.one * radius);
        material.SetPass(0);
        Graphics.DrawMesh(_circleMesh, matrix, material, 0);
    }

    private static Mesh GenerateFilledCircleMesh(int segments)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        }

        for (int i = 0; i < segments; i++)
        {
            int start = i + 1;
            int end = (i + 1) % segments + 1;

            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = end;
            triangles[i * 3 + 2] = start;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    public static void DrawSphere(Vector3 position, Color color, float radius = 1f, float duration = 0f)
    {
        //TODO: Duration doesn't really work
        float angle = 10f;
        Vector3 lastPoint = position + new Vector3(radius, 0, 0);
        Vector3 nextPoint;

        for (float i = angle; i <= 360; i += angle)
        {
            float rad = Mathf.Deg2Rad * i;

            // XY circle
            nextPoint = position + new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0);
            Debug.DrawLine(lastPoint, nextPoint, color, duration);
            lastPoint = nextPoint;
        }

        lastPoint = position + new Vector3(radius, 0, 0);
        for (float i = angle; i <= 360; i += angle)
        {
            float rad = Mathf.Deg2Rad * i;

            // XZ circle
            nextPoint = position + new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);
            Debug.DrawLine(lastPoint, nextPoint, color, duration);
            lastPoint = nextPoint;
        }

        lastPoint = position + new Vector3(0, radius, 0);
        for (float i = angle; i <= 360; i += angle)
        {
            float rad = Mathf.Deg2Rad * i;

            // YZ circle
            nextPoint = position + new Vector3(0, Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius);
            Debug.DrawLine(lastPoint, nextPoint, color, duration);
            lastPoint = nextPoint;
        }
    }

    // Last known pixel-ratio of the scene view camera. This represents the world-space distance between each pixel.
    // This cache gets around an issue where the scene view camera is null on certain editor frames.
    private static float _lastKnownSceneViewPixelRatio = 0.001f;

    // Draws a line in the scene
    public static void DrawLine(Vector3 start, Vector3 end, float thickness, Color color, float duration = 0f)
    {
#if UNITY_EDITOR
        Vector3 lineDir = (end - start).normalized;
        Vector3 toCamera = Vector3.back;

        Camera camera = null;
        // Get the current scene view camera.
        if (SceneView.currentDrawingSceneView != null)
        {
            camera = SceneView.currentDrawingSceneView.camera;
        }

        if (camera == null)
        {
            // Default to the
            camera = Camera.current;
        }
        if (camera != null)
        {
            toCamera = (camera.transform.position - start).normalized;
            _lastKnownSceneViewPixelRatio = (camera.orthographicSize * 2f) / camera.pixelHeight;
        }

        Vector3 orthogonal = Vector3.Cross(lineDir, toCamera).normalized;
        int pixelThickness = Mathf.CeilToInt(thickness);
        float totalThick = _lastKnownSceneViewPixelRatio * pixelThickness;
        for (int i = 0; i < pixelThickness; i++)
        {
            Vector3 offset = orthogonal * ((i * _lastKnownSceneViewPixelRatio) - (totalThick / 2f));
            Debug.DrawLine(start + offset, end + offset, color, duration);
        }
#endif //UNITY_EDITOR
    }
}

