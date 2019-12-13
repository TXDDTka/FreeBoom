using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEngineer : PlayerController
{
    [Header("Engineer Properties")]
    [SerializeField] private Transform turretSpawn = null;
    [SerializeField] private EngineerTurret turretPrefab = null;
    [SerializeField] private Vector2 turretVelocity = Vector2.one;
    [SerializeField] private int turretGravity = 5;

    [Header("Trajectory")]
    [SerializeField] private int pointsCount = 20;
    [SerializeField] private Transform pointPrefab = null;

    private EngineerTurret activeTurret = null;
    private Vector3 turretFinalVelocity = Vector3.zero;
    private GameObject pointsParent = null;
    private Transform[] points = null;

    protected override void Start()
    {
        base.Start();

        PopulatePoints();

        shootJoystick.OnBeginDragEvent += EnablePoints;
    }

    private void PopulatePoints()
    {
        pointsParent = new GameObject("Points Parent");
        points = new Transform[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            points[i] = Instantiate(pointPrefab, pointsParent.transform);
            points[i].name = "Point_" + i;
        }

        pointsParent.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (shootJoystick.HasInput)
        {
            turretFinalVelocity = shootJoystick.Direction * turretVelocity;
            ShowTrajectory();
        }
    }

    protected override void UseUltimateAbility()
    {
        if (activeTurret == null)
        {
            activeTurret = Instantiate(turretPrefab, turretSpawn.position, Quaternion.identity, transform);
            shootJoystick.OnUpEvent += SpawnTurret;
        }
        else
        {
            Destroy(activeTurret.gameObject);
            activeTurret = null;
            shootJoystick.OnUpEvent -= SpawnTurret;
        }
    }

    protected override void Flip()
    {
        base.Flip();
        Vector3 pos = turretSpawn.localPosition;
        pos.x = -pos.x;
        turretSpawn.localPosition = pos;

        if (activeTurret) activeTurret.transform.localPosition = pos;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireSphere(turretSpawn.position, 0.2f);
    }

    private void SpawnTurret()
    {
        if (activeTurret != null)
        {
            activeTurret.LauchTurret(turretFinalVelocity, turretGravity);
            activeTurret = null;
            turretFinalVelocity = Vector3.zero;
        }

        shootJoystick.OnUpEvent -= SpawnTurret;
    }

    private void ShowTrajectory()
    {
        Vector2 origin = turretSpawn.position;
        Vector2 velocity = turretFinalVelocity;

        Vector2 gravity = Physics2D.gravity;
        gravity.y *= turretGravity;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i].position = origin + velocity * time + gravity * time * time / 2f;
        }
    }

    private void EnablePoints(bool enable)
    {
        if (activeTurret != null)
        {
            pointsParent.SetActive(enable);
        }
    }
}
