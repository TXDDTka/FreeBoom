using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingTrajectory : MonoBehaviourPunCallbacks
{
    [Header("Trajectory")]
    [SerializeField] private int distance = 0;
    [SerializeField] private int crosshairIndex = 0;
    [SerializeField] private Vector2 trajectoryVelocity = Vector2.one;
    private Vector3 finalVelocity = Vector3.zero;



    [Header("Prefabs")]
    [SerializeField] private GameObject crosshairPrefab = null;
    [SerializeField] private GameObject pointPrefab = null;
    private GameObject pointsParent = null;

    [Space]
    [SerializeField] private List<GameObject> points = new List<GameObject>();

    private PhotonView PV = null;
    private PhotonPlayerShooting photonPlayerShooting;
    private ShootJoystick shootJoystick = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        photonPlayerShooting = GetComponent<PhotonPlayerShooting>();
        shootJoystick = ShootJoystick.Instance;
    }


    private void Start()
    {
        if (!PV.IsMine) return;

        PopulatePoints();
        shootJoystick.OnBeginDragEvent += EnablePoints;
    }
    
    void Update()
    {
        if (!PV.IsMine) return;

        if (shootJoystick.HasInput)
        {

            finalVelocity = shootJoystick.Direction * trajectoryVelocity;
            ShowTrajectory();
        }
    }

    private void PopulatePoints()
    {
        pointsParent = new GameObject("Points Parent");

        distance = photonPlayerShooting.mainWeapon.mainWeaponShootingDistance;

        for (int i = 0; i < distance; i++)
        {
            if (i < distance - 1)
            {
                points.Add(Instantiate(pointPrefab, pointPrefab.transform.position, Quaternion.identity)); 
                points[i].transform.parent = pointsParent.transform;
                points[i].name = "Point_" + i;
            }
            else
            {
                points.Add(Instantiate(crosshairPrefab, crosshairPrefab.transform.position, Quaternion.identity));
                points[i].transform.parent = pointsParent.transform;
                points[i].name = "Crosshair";
                crosshairIndex = i;
            }

        }

        pointsParent.SetActive(false);
    }

    private void ShowTrajectory()
    {
        Vector2 origin = photonPlayerShooting.mainWeapon.mainWeaponShootPoint.position;
        Vector2 velocity = finalVelocity;

        Vector2 trajectoryDirection = Vector2.zero;
        trajectoryDirection.x *= distance;

        for (int i = 0; i < distance; i++)
        {
            float time = i * 0.1f;
            points[i].transform.position = origin + velocity * time + trajectoryDirection * time * time / 2f;
        }
    }

    public void ChangeMainWeaponTrajectory()
    {

        distance = photonPlayerShooting.mainWeapon.mainWeaponShootingDistance;

        if (distance < points.Count)
        {
            for (int i = distance; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                {
                    ChangeIndexDown();
                }
                points[i].SetActive(false);
            }
        }
        else if (distance > points.Count)
        {
            AddTrajectory();
        }
        else
        {
            if (crosshairIndex < points.Count - 1)
            {
                for (int i = crosshairIndex; i < points.Count; i++)
                {
                    points[i].SetActive(true);
                }

                 ChangeIndexUp();
            }
        }
    }

    public void ChangeSecondWeaponTrajectory()
    {
        distance = photonPlayerShooting.secondWeapon.secondWeaponShootingDistance;

        if (distance < points.Count)
        {
            for (int i = distance; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                {
                    ChangeIndexDown();
                }
                points[i].SetActive(false);
            }
        }
        else if (distance > points.Count)
        {
            AddTrajectory();
        }
        else
        {
            if (crosshairIndex < points.Count - 1)
            {
                
                for (int i = crosshairIndex; i < points.Count; i++)
                {
                    points[i].SetActive(true);
                }

                ChangeIndexUp();
            }

        }
    }

    private void AddTrajectory()
    {

        for (int i = points.Count; i < distance; i++)
        {
            points.Add(Instantiate(pointPrefab, pointPrefab.transform.position, Quaternion.identity));
            points[i].transform.parent = pointsParent.transform;
            points[i].name = "Point_" + i;
        }

        ChangeIndexUp();
    }

    private void ChangeIndexUp()
    {
        var buf = points[points.Count - 1];
        points[points.Count - 1] = points[crosshairIndex];
        points[crosshairIndex] = buf;
        crosshairIndex = points.Count - 1;
    }

    private void ChangeIndexDown()
    {
        var buf = points[distance - 1];
        points[distance - 1] = points[points.Count - 1];
        points[points.Count - 1] = buf;
        crosshairIndex = distance - 1;
    }

    private void EnablePoints(bool enable)
    {
        pointsParent.SetActive(enable);
    }

    public  void Disable()
    {
        shootJoystick.OnBeginDragEvent -= EnablePoints;
    }
}
