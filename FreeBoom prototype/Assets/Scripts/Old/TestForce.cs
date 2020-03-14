using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForce : MonoBehaviour
{
    public float speed = 0f;
    public float magnitude = 0f;
    public float thrust = 10f;
    [Header("3D")]
    public Transform transform3d;
    public Rigidbody rigidbody3d;

    [Header("2D")]
    public Transform transform2d;
    public Rigidbody2D rigidbody2d;

    public LineRenderer line;
    [SerializeField] private float fov = 90f;
    [SerializeField] private int rayCount = 50;
    public Vector3 direction;
    public Vector3 origin;
    public float distance;
    public int lengthOfLineRenderer;//Количество точек,шагов,вершин
    private float startingAngle;
    //private void Update()
    //{
    //    Vector3[] vertices = new Vector3[rayCount + 1 + 1];

    //    line.positionCount = lengthOfLineRenderer; //Укажем количество вершин(vertices)
    //    var points = new Vector3[lengthOfLineRenderer];
    //    var t = Time.time;
    //    for (int i = 0; i < lengthOfLineRenderer; i++)
    //    {
    //        points[i] = new Vector3(i * 0.5f, Mathf.Sin(i + t), 0);
    //    }
    //    line.SetPositions(points);
    //}



    //private void Update()
    //{
    //    rayCount = 50;
    //    float angle = startingAngle;
    //    float angleIncrease = fov / rayCount;


    //    Vector3[] vertices = new Vector3[rayCount + 1 + 1];
    //    Vector2[] uv = new Vector2[vertices.Length];
    //    int[] triangles = new int[rayCount * 3];

    //    vertices[0] = origin;

    //    int vertexIndex = 1;
    //    int triangleIndex = 0;
    //    for (int i = 0; i <= rayCount; i++)
    //    {
    //        Vector3 vertex;
    //        RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, distance, layerMask);
    //        if (raycastHit2D.collider == null)
    //        {
    //            Debug.DrawRay(origin, direction * distance, Color.red);
    //            //No hit
    //            Debug.Log("No hit");
    //            vertex = origin + GetVectorFromAngle(angle) * distance;
    //        }
    //        else
    //        {
    //            Debug.DrawRay(origin, direction * raycastHit2D.distance, Color.yellow);
    //            //Hit object
    //            Debug.Log("Hit object");
    //            vertex = origin + GetVectorFromAngle(angle) * raycastHit2D.distance;//raycastHit2D.point;
    //        }

    //        // vertex = origin + GetVectorFromAngle(angle) * distance;

    //        vertices[vertexIndex] = vertex;

    //        if (i > 0)
    //        {
    //            triangles[triangleIndex + 0] = 0;
    //            triangles[triangleIndex + 1] = vertexIndex;
    //            triangles[triangleIndex + 2] = vertexIndex - 1;

    //            triangleIndex += 3;
    //        }
    //        vertexIndex++;

    //        angle += angleIncrease;
    //    }

    //    line.positionCount = vertices;
    //    mesh.uv = uv;
    //    mesh.triangles = triangles;

    //    mesh.RecalculateBounds();
    //    mesh.RecalculateNormals();
    //}

    //private void Update()
    //{
    //    float startingAngle = GetAngleFromVectorFloat(direction) - fov / 2f;
    //    float angle = startingAngle;
    //    float angleIncrease = fov / rayCount;

    //    line.SetPosition(1, origin + GetVectorFromAngle(angle) * distance);
    //}
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

}
