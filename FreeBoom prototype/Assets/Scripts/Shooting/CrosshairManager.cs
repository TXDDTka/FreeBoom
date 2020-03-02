using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour {

    [Header("CrosshairParameters :")]
    [SerializeField] private LineRenderer LineRendererObject;
    public enum CrosshairType { None, LineCrosshair, ShotgunСrosshair }
    public CrosshairType currentCrosshair = CrosshairType.None;

    public CrosshairType mainWeaponCrosshair = CrosshairType.None;
    public CrosshairType secondWeaponCrosshair = CrosshairType.None;

    private Vector3 origin = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    public float distance = 0f;

    public bool crosshairActive = false;

    public static CrosshairManager Instance { get; private set; }

    [Header("LineCrosshairParameters :")]
    [SerializeField] private float StartWidth = 0f;
    [SerializeField] private float EndWidth = 0f;

    [Header("ShootGunCrosshairParameters :")]
    [SerializeField] private float fov = 90f;
    [SerializeField] private int rayCount = 50;
   // [SerializeField] private float viewDistance = 8f;

    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float startingAngle;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

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

    void Start () 
    {
                LineRendererObject.startWidth = StartWidth;
                LineRendererObject.endWidth = EndWidth;

                mesh = new Mesh();
                meshFilter.mesh = mesh;
    }

   
    public void LateUpdate()
    {
        if (crosshairActive)
        {
            switch (currentCrosshair)
            {
                case CrosshairType.LineCrosshair:
                    CrosshairLine();
                    break;
            case CrosshairType.ShotgunСrosshair:
                CrosshairShootGun();
                break;
        }

        }
    }

    private void CrosshairShootGun()
    {
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
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, distance, layerMask);
            if (raycastHit2D.collider == null)
            {
                //No hit
                Debug.Log("No hit");
                vertex = origin + GetVectorFromAngle(angle) * distance;
            }
            else
            {
                Debug.DrawRay(origin, direction * raycastHit2D.distance, Color.yellow);
                //Hit object
                Debug.Log("Hit object");
                vertex = origin + GetVectorFromAngle(angle) * raycastHit2D.distance;//raycastHit2D.point;
            }

           // vertex = origin + GetVectorFromAngle(angle) * distance;

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

    public void CrosshairLine()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, distance, layerMask);
        if (raycastHit2D.collider == null)
        {
            LineRendererObject.SetPosition(0, origin);
            LineRendererObject.SetPosition(1, origin + direction * distance);
        }
        else
        {

            LineRendererObject.SetPosition(0, origin);
            LineRendererObject.SetPosition(1, origin + direction * raycastHit2D.distance);
        }
    }

    public void SetParemeters(Vector3 origin, Vector3 direction, float distance)
    {
        this.origin = origin;
        this.direction = direction;
        this.distance = distance;

        if(currentCrosshair == CrosshairType.ShotgunСrosshair)
        startingAngle = GetAngleFromVectorFloat(this.direction) - fov / 2f;

    }

    public void ChangeColor(Color color)
    {
        switch (currentCrosshair)
        {
            
            case CrosshairType.LineCrosshair:
                LineRendererObject.material.color = color;
                break;
            case CrosshairType.ShotgunСrosshair:
                meshRenderer.material.color = color;
                break;
        }
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        //float angleRad = angle * Mathf.Deg2Rad;//(Mathf.PI / 180f122200 ,   );
        float angleRad =                                          1* (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
                                                                                                                                                                                                                                                                                                                                          
        return n;
    }

    public void EnableCrosshair(bool enable)
    {
        switch (currentCrosshair)
        {
            case CrosshairType.LineCrosshair:
                LineRendererObject.enabled = enable;
                break;
            case CrosshairType.ShotgunСrosshair:
                meshRenderer.enabled = enable;
                break;
        }
    }
}
