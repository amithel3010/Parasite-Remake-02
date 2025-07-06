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
}
