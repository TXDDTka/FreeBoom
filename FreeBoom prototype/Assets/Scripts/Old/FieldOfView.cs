using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float fov = 90f;
    [SerializeField] private int rayCount = 50;
    [SerializeField] private float viewDistance = 8f;
    
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    [SerializeField] private Vector3 origin;
    private float startingAngle;

    public static FieldOfView Instance { get; private set; }

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public void Awake()
    {
        InitializeSingleton();
    }

    void Start()
    {

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
     //   mesh.MarkDynamic();
       // fov = 90f;
       //  origin = Vector3.zero;
    }

    private void LateUpdate()
    {

      //  mesh.MarkModified();

       // mesh.Clear();

        rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;


        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            //RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            //if (raycastHit2D.collider == null)
            //{
            //    //No hit
            //    vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            //}
            //else
            //{
            //    //Hit object
            //    vertex = raycastHit2D.point;
            //}

            vertex = origin + GetVectorFromAngle(angle) * viewDistance;

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex;
                triangles[triangleIndex + 2] = vertexIndex - 1;

                triangleIndex += 3;
            }
            vertexIndex++;

            angle += angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * Mathf.Deg2Rad;//(Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void SetOriging(Vector3 origin)
    {
        this.origin = origin;
    }
    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) - fov / 2f;
    }
}
