using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairPointsParent : MonoBehaviour
{
	public static CrosshairPointsParent Instance { get; private set; }

    //[Header("Trajectory")]
    //public int distance = 0;
    //[SerializeField] private int crosshairIndex = 0;
    //public Vector2 trajectoryVelocity = Vector2.one;
    //public Vector2 finalVelocity = Vector2.zero;
    //public Vector2 originPosition = Vector2.one;

    //[Header("Prefabs")]
    //[SerializeField] private GameObject crosshairPrefab = null;
    //[SerializeField] private GameObject pointPrefab = null;
    public GameObject pointsParent;// = new GameObject("Points Parent");

   // public CrosshairCheckCollision crosshairCheckCollision;

    [System.Serializable]
    public class CrossHair
    {
        public GameObject points;
        public SpriteRenderer pointsSprites;
    }

    public List<CrossHair> crossHair;

    //public Color teamColor;

    public int crosshairIndex = 0;
    public int distance = 0;
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

    //public void Start()
    //{
    //    pointsParent = new GameObject("Points Parent");
    //    pointsParent.transform.SetParent(gameObject.transform);
    //}

    //public void ShowTrajectory()
    //{

    //    //float dist = Vector3.Distance(originPosition, transform.position);

    //    Vector2 trajectoryDirection = Vector2.zero;
    //    trajectoryDirection.x *= distance;
    //    //trajectoryDirection.x *= (distance - crosshairCheckCollision.distance.x);

    //    for (int i = 0; i < distance; i++)
    //    {
    //        float time = i * 0.1f;
    //        crossHair[i].points.transform.position = originPosition + finalVelocity * time + trajectoryDirection * time * time / 2f;
    //       // crossHair[i].points.SetActive();
    //    }
    //}

    //private void ChangeTeamColor()
    //{

    //    for (int i = 0; i < crossHair.Count; i++)
    //    {
    //        crossHair[i].pointsSprites.color = teamColor;
    //    }
    //}

    //public void PopulatePoints(int getDistance, Color getColor)
    //{


    //    if (pointsParent == null)
    //    {
    //        distance = getDistance;

    //        teamColor = getColor;

    //        pointsParent = new GameObject("Points Parent");
    //        pointsParent.transform.SetParent(gameObject.transform);

    //        for (int i = 0; i < distance; i++)
    //        {
    //            if (i < distance - 1)
    //            {
    //                 GameObject point = Instantiate(pointPrefab , pointPrefab.transform.position, Quaternion.identity, pointsParent.transform);
    //                 crossHair.Add(new CrossHair { points = point, pointsSprites = point.GetComponent<SpriteRenderer>() });

    //                crossHair[i].points.name = "Point_" + (i + 1);
    //                crossHair[i].pointsSprites.color = teamColor;
    //            }
    //            else
    //            {

    //                GameObject point = Instantiate(crosshairPrefab, crosshairPrefab.transform.position, Quaternion.identity, pointsParent.transform);

    //             //   crosshairCheckCollision = point.GetComponent<CrosshairCheckCollision>();

    //                crossHair.Add(new CrossHair { points = point, pointsSprites = point.GetComponent<SpriteRenderer>() });

    //                crossHair[i].points.name = "Crosshair_" + (i + 1);
    //                crossHair[i].pointsSprites.color = teamColor;

    //                crosshairIndex = i;
    //            }


    //        }
    //        pointsParent.SetActive(false);
    //    }
    //    else
    //    {

    //        if (teamColor != getColor)
    //        {
    //            teamColor = getColor;
    //            ChangeTeamColor();
    //        }
    //        ChangeWeaponTrajectory(getDistance);
    //    }

    //}


    //public void ChangeWeaponTrajectory(int changeDistance)
    //{

    //    Debug.Log("ChangeMainWeaponTrajectory");
    //        //Полученная дистанция больше текущей
    //        if(changeDistance > distance)
    //        {
    //        Debug.Log("Полученная дистанция больше текущей");
    //        distance = changeDistance;
                
    //            if (distance > crossHair.Count)//Полученная дистанция больше максимальной дистанция
    //                 {
    //            Debug.Log("Полученная дистанция больше максимальной дистанция");
    //            AddTrajectory(); //Увеличиваем дистанцию
    //                 }
    //            else //Полученная дистанция меньше или равна максимальной дистанция
    //                {
    //            Debug.Log("Полученная дистанция меньше или равна максимальной дистанция");

    //            for (int i = crosshairIndex; i < distance; i++)
    //            {
    //                crossHair[i].points.SetActive(true);
    //            }

    //            ChangeIndex();
    //        }
    //        }
    //        //Полученная дистанция меньше текущей
    //        else if (changeDistance < distance) 
    //        {
    //        Debug.Log("Полученная дистанция меньше текущей");

    //        distance = changeDistance;



    //        ChangeIndex();

    //        for (int i = distance; i < crossHair.Count; i++)
    //        {

    //            crossHair[i].points.SetActive(false);
    //        }

            
    //    }

    //}

    //private void ChangeIndex()
    //{
    //    var buf = crossHair[crosshairIndex];
    //    var buf2 = crossHair[distance - 1];
    //    crossHair[distance - 1] = buf;
    //    crossHair[crosshairIndex] = buf2;
    //    crosshairIndex = distance - 1;
    //}


    //private void AddTrajectory()
    //{

    //    for (int i = crossHair.Count; i < distance; i++)
    //    {

    //        GameObject point = Instantiate(pointPrefab, pointPrefab.transform.position, Quaternion.identity, pointsParent.transform);
    //        crossHair.Add(new CrossHair { points = point, pointsSprites = point.GetComponent<SpriteRenderer>() });

    //        crossHair[i].points.name = "Point_" + (i + 1);
    //        crossHair[i].pointsSprites.color = teamColor;

    //    }

    //    for (int i = crosshairIndex; i < distance; i++)
    //    {
    //        crossHair[i].points.SetActive(true);
    //    }

    //    ChangeIndex();
    //}


    //public void EnablePoints(bool enable)
    //{
    //    pointsParent.SetActive(enable);
    //}


}
