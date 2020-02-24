using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerShootingTrajectory : MonoBehaviour
{

    [Header("Trajectory")]

    public Vector2 trajectoryVelocity = Vector2.one;
    public Vector2 finalVelocity = Vector2.zero;
    public Vector2 originPosition = Vector2.one;

    [Header("Prefabs")]
    [SerializeField] private GameObject crosshairPrefab = null;
    [SerializeField] private GameObject pointPrefab = null;


    public Color teamColor;

    private PlayerManager playerManager = null;
    private CrosshairPointsParent crosshairPointsParent = null;


    public void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        crosshairPointsParent = CrosshairPointsParent.Instance;
    }


    void Update()
    {
        if (!playerManager.PV.IsMine) return;

        if (playerManager.shootJoystick.HasInput)
        {
            originPosition = playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon ? playerManager.playerShooting.mainWeapon.mainWeaponShootPoint.position : playerManager.playerShooting.secondWeapon.secondWeaponShootPoint.position;
            finalVelocity = playerManager.shootJoystick.Direction * trajectoryVelocity;
            ShowTrajectory();
        }
    }


    public void ShowTrajectory()
    {

        //float dist = Vector3.Distance(originPosition, transform.position);

        Vector2 trajectoryDirection = Vector2.zero;
        trajectoryDirection.x *= crosshairPointsParent.distance;
        //trajectoryDirection.x *= (distance - crosshairCheckCollision.distance.x);

        for (int i = 0; i < crosshairPointsParent.distance; i++)
        {
            float time = i * 0.1f;
            crosshairPointsParent.crossHair[i].points.transform.position = originPosition + finalVelocity * time + trajectoryDirection * time * time / 2f;
            // crossHair[i].points.SetActive();
        }
    }

    private void ChangeTeamColor()
    {

        for (int i = 0; i < crosshairPointsParent.crossHair.Count; i++)
        {
            crosshairPointsParent.crossHair[i].pointsSprites.color = teamColor;
        }
    }

    public void PopulatePoints(int getDistance, Color getColor)
    {


        if (crosshairPointsParent.pointsParent == null)
        {
            crosshairPointsParent.distance = getDistance;

            teamColor = getColor;

            crosshairPointsParent.pointsParent = new GameObject("Points Parent");
            crosshairPointsParent.pointsParent.transform.SetParent(crosshairPointsParent.gameObject.transform);

            for (int i = 0; i < crosshairPointsParent.distance; i++)
            {
                if (i < crosshairPointsParent.distance - 1)
                {
                    GameObject point = Instantiate(pointPrefab, pointPrefab.transform.position, Quaternion.identity, crosshairPointsParent.pointsParent.transform);
                    crosshairPointsParent.crossHair.Add(new CrosshairPointsParent.CrossHair { points = point, pointsSprites = point.GetComponent<SpriteRenderer>() });

                    crosshairPointsParent.crossHair[i].points.name = "Point_" + (i + 1);
                    crosshairPointsParent.crossHair[i].pointsSprites.color = teamColor;
                }
                else
                {

                    GameObject crosshair = Instantiate(crosshairPrefab, crosshairPrefab.transform.position, Quaternion.identity, crosshairPointsParent.pointsParent.transform);
                    crosshairPointsParent.crossHair.Add(new CrosshairPointsParent.CrossHair { points = crosshair, pointsSprites = crosshair.GetComponent<SpriteRenderer>() });

                    crosshairPointsParent.crossHair[i].points.name = "Crosshair_" + (i + 1);
                    crosshairPointsParent.crossHair[i].pointsSprites.color = teamColor;

                    crosshairPointsParent.crosshairIndex = i;
                }


            }
            crosshairPointsParent.pointsParent.SetActive(false);
        }
        else
        {

            if (teamColor != getColor)
            {
                teamColor = getColor;
                ChangeTeamColor();
            }
            ChangeWeaponTrajectory(getDistance);
        }

    }


    public void ChangeWeaponTrajectory(int changeDistance)
    {
        //Полученная дистанция больше текущей
        if (changeDistance > crosshairPointsParent.distance)
        {
            crosshairPointsParent.distance = changeDistance;

            if (crosshairPointsParent.distance > crosshairPointsParent.crossHair.Count)//Полученная дистанция больше максимальной дистанция
            {
                AddTrajectory(); //Увеличиваем дистанцию
            }
            else //Полученная дистанция меньше или равна максимальной дистанция
            {

                for (int i = crosshairPointsParent.crosshairIndex; i < crosshairPointsParent.distance; i++)
                {
                    crosshairPointsParent.crossHair[i].points.SetActive(true);
                }

                ChangeIndex();
            }
        }
        //Полученная дистанция меньше текущей
        else if (changeDistance < crosshairPointsParent.distance)
        {

            crosshairPointsParent.distance = changeDistance;



            ChangeIndex();

            for (int i = crosshairPointsParent.distance; i < crosshairPointsParent.crossHair.Count; i++)
            {

                crosshairPointsParent.crossHair[i].points.SetActive(false);
            }


        }

    }

    private void ChangeIndex()
    {
        var buf = crosshairPointsParent.crossHair[crosshairPointsParent.crosshairIndex];
        var buf2 = crosshairPointsParent.crossHair[crosshairPointsParent.distance - 1];
        crosshairPointsParent.crossHair[crosshairPointsParent.distance - 1] = buf;
        crosshairPointsParent.crossHair[crosshairPointsParent.crosshairIndex] = buf2;
        crosshairPointsParent.crosshairIndex = crosshairPointsParent.distance - 1;
    }


    private void AddTrajectory()
    {

        for (int i = crosshairPointsParent.crossHair.Count; i < crosshairPointsParent.distance; i++)
        {

            GameObject point = Instantiate(pointPrefab, pointPrefab.transform.position, Quaternion.identity, crosshairPointsParent.pointsParent.transform);
            crosshairPointsParent.crossHair.Add(new CrosshairPointsParent.CrossHair { points = point, pointsSprites = point.GetComponent<SpriteRenderer>() });

            crosshairPointsParent.crossHair[i].points.name = "Point_" + (i + 1);
            crosshairPointsParent.crossHair[i].pointsSprites.color = teamColor;

        }

        for (int i = crosshairPointsParent.crosshairIndex; i < crosshairPointsParent.distance; i++)
        {
            crosshairPointsParent.crossHair[i].points.SetActive(true);
        }

        ChangeIndex();
    }


    public void EnablePoints(bool enable)
    {
        crosshairPointsParent.pointsParent.SetActive(enable);
    }
}
