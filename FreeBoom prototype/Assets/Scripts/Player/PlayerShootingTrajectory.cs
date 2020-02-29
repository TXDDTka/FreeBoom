using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerManager))]
public class PlayerShootingTrajectory : MonoBehaviour
{
 //   public LayerMask[] layer;

 //   [Header("Trajectory")]

 //   public Vector2 trajectoryVelocity = Vector2.one;
 //   public Vector2 finalVelocity = Vector2.zero;
 //   public Vector2 originPosition = Vector2.one;

 //   public float time = 0.2f;

 //   [Header("Prefabs")]
 //  // [SerializeField] private GameObject crosshairPrefab = null;
 //   [SerializeField] private GameObject pointPrefab = null;
 //   public Transform lineRenderer;
 //   public LineRenderer line;
 //   //public Color teamColor;

 //   private PlayerManager playerManager = null;
 //   private CrosshairPointsParent crosshairPointsParent = null;
 //   private Color currentColor;

 //   public Transform crosshair;

 ////   public Vector3 lookPosition = Vector3.zero;

 ////   private float counter;
 // //  public float lineDrawSpeed = 6f;
 // //  public float dist;
 //   public void Awake()
 //   {
 //       playerManager = GetComponent<PlayerManager>();
 //       crosshairPointsParent = CrosshairPointsParent.Instance;
 //   }

 //   private void Start()
 //   {
 //       if (crosshairPointsParent.currentColor != Color.white)
 //           ChangeReloadingColor(Color.white);


 //       Vector3 startPosition = Vector3.zero;
 //       startPosition.z = playerManager.playerShooting.mainWeapon.shootingDistance;
 //       //  crosshair.localPosition = startPosition;//new Vector3(0,0, playerManager.playerShooting.mainWeapon.shootingDistance);

 //       LineRenderer line = Instantiate(lineRenderer.gameObject, playerManager.playerShooting.mainWeapon.shootPoint.position, Quaternion.identity, playerManager.playerShooting.mainWeapon.shootPoint).GetComponent<LineRenderer>();

 //      // line.transform.localPosition = new Vector3(0, 0,0);
 //       //line.transform.localRotation = Quaternion.identity;
 //       // lineRenderer.gameObject.transform.parent(playerManager.playerShooting.mainWeapon.shootPoint);

 //       line.SetPosition(0, startPosition);
 //     //  line.startWidth(.4f);
 //       //  lineRenderer.SetPosition(1, startPosition);


 //       //   dist = Vector3.Distance(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection() * 6);
 //   }



 //   void Update()
 //   {
 //       if (!playerManager.PV.IsMine) return;

 //       if (playerManager.shootJoystick.HasInput && crosshair != null)
 //       {
 //           originPosition = playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon ? playerManager.playerShooting.mainWeapon.shootPoint.position : playerManager.playerShooting.secondWeapon.shootPoint.position;
 //           finalVelocity = playerManager.shootJoystick.Direction* trajectoryVelocity;
 //           ShowTrajectory();

 //       }
 //   }



 //   public void ShowTrajectory()
 //   {

 //       Vector2 trajectoryDirection = Vector2.zero;
 //       trajectoryDirection.x *= crosshairPointsParent.distance;


 // //      RayCast();
 //       // Vector2 finalPosition2 = new Vector2(playerManager.shootJoystick.Vertical, playerManager.shootJoystick.Horizontal);

 //       ///  Vector2 trajectoryDirection2 = finalPosition2;
 //       // trajectoryDirection2.x += 6;

 //       //for (int i = 0; i < crosshairPointsParent.distance; i++)
 //       //{
 //       //    float time = i * 0.2f;
 //       //    crosshairPointsParent.crossHair[i].points.transform.position = originPosition + finalVelocity * time + trajectoryDirection * time * time / 2f;
 //       //}
 //       //crosshairPointsParent.crossHair[crosshairPointsParent.crossHair.Count - 1].points.transform.position = originPosition + finalVelocity + trajectoryDirection;

 //       //Vector3 direction2 = transform.TransformDirection(playerManager.playerShooting.mainWeapon.shootPoint.position) * crosshairPointsParent.distance;

 //       //   crosshair.transform.TransformDirection((playerManager.playerShooting.mainWeapon.shootPoint.position) * crosshairPointsParent.distance); // /*playerManager.playerShooting.mainWeapon.shootPoint.position +*/ direction2;

 //       //  RayCast();
 //       //  LineRender();

 //       // LineRender();

 //       //  lookPosition = GetLookPosition();

 //       //  Vector3 test = new Vector3(originPosition.x, originPosition.y, 6);
 //       //  test.z += 6;
 //       //  finalVelocity = playerManager.shootJoystick.Direction * finalPosition2;
 //       // float rotationAngle = Mathf.Atan2(playerManager.shootJoystick.Vertical, playerManager.shootJoystick.Horizontal) * Mathf.Rad2Deg;

 //       //   crosshair.transform.position = test/*+ finalPosition2*/; //*+ finalVelocity;// + trajectoryDirection;
 //       //  crosshair.transform.eulerAngles = new Vector3(0, 0, rotationAngle);                                                                                                                             //  crosshair.transform.rotation = playerManager.shootJoystick.transform.rotation;
 //       //   Vector3 forward = transform.TransformDirection(playerManager.shootJoystick.Direction) * 6;
 //       // Debug.DrawRay(playerManager.playerShooting.mainWeapon.shootPoint.position, forward, Color.green);

 //       //  RaycastHit2D hitInfo = Physics2D.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, playerManager.playerShooting.mainWeapon.shootPoint.right * 6);
 //       //  lineRenderer.SetPosition(0, originPosition);
 //       // lineRenderer.SetPosition(1, originPosition.rig);
 //   }

 //   //private void ChangeTeamColor()
 //   //{

 //   //    for (int i = 0; i < crosshairPointsParent.crossHair.Count; i++)
 //   //    {
 //   //        crosshairPointsParent.crossHair[i].pointsSprites.color = teamColor;
 //   //    }
 //   //}

 //   void LineRender()
 //       {

 //    //   RaycastHit2D hitInfo = Physics2D.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, playerManager.playerShooting.mainWeapon.shootPoint.right);

 //     //  lineRenderer.SetPosition(0, playerManager.playerShooting.mainWeapon.shootPoint.position);
 //     //  lineRenderer.SetPosition(1, playerManager.playerShooting.mainWeapon.shootPoint.position + playerManager.playerShooting.mainWeapon.shootPoint.right * 6);
 //       //   Vector3 dist = Vector3.Distance(playerManager.playerShooting.mainWeapon.shootPoint.position);
 //   }

 //   public Vector3 GetLookPosition()
 //   {
 //       Vector3 look = Vector3.zero;

 //       Vector3 aimPos = transform.position + Vector3.up * 1.3f;


 //       if (playerManager.shootJoystick.HasInput)
 //       {
 //           look = aimPos + playerManager.shootJoystick.Direction.normalized * 2;
 //       }
 //       else
 //       {
 //       }


 //       return look;
 //   }
    
 //   public Vector3 JoystickDirection()
 //   {
 //       Vector3 dir = playerManager.shootJoystick.Direction.x > 0 || playerManager.shootJoystick.Direction.y > 0 ? playerManager.shootJoystick.Direction.normalized : playerManager.shootJoystick.Direction;
 //       return dir;
 //   }

 //   void RayCast()
 //   {
 //       int layerMask = 10;
 //       //lineRenderer.SetPosition(0, originPosition);

 //       //   RaycastHit2D view = Physics2D.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, playerManager.shootJoystick.Direction, 6);
 //       //  Debug.DrawLine(playerManager.playerShooting.mainWeapon.shootPoint.position, crosshair.position, Color.red);
 //       //if (view.collider != null)
 //       //{
 //       //  lineRenderer.SetPosition(0, playerManager.playerShooting.mainWeapon.shootPoint.position);
 //       // lineRenderer.SetPosition(1, JoystickDirection() * 6);
 //       //    lineRenderer.SetWidth(.45f, .45f);
 //       //}
 //       //if (counter < dist)
 //       //{
 //       //    counter += .1f / lineDrawSpeed;

 //       //    float x = Mathf.Lerp(0, dist, counter);

 //       //    Vector3 pointA = playerManager.playerShooting.mainWeapon.shootPoint.position;
 //       //    Vector3 pointB = JoystickDirection() * 6;

 //       //    Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA + pointA);

 //       //    lineRenderer.SetPosition(0, playerManager.playerShooting.mainWeapon.shootPoint.position);
 //       //    lineRenderer.SetWidth(.45f, .45f);
 //       //    lineRenderer.SetPosition(1, pointAlongLine);

 //       //}
 //       // lineRenderer.w(6f);

        

 //       RaycastHit hit;

 //       if (Physics.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection(), out hit/*, Mathf.Infinity*/, 6, layer[0]) ||
 //           Physics.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection(), out hit/*, Mathf.Infinity*/, 6, layer[1]))
 //       {

 //           Vector3 startPosition = Vector3.zero;
 //           startPosition.z = hit.distance;

 //           line.SetPosition(1, startPosition);

 //           Debug.DrawRay(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection() * hit.distance, Color.yellow);
 //           Debug.Log(hit.collider.gameObject.name);
 //       }
 //       else
 //       {
 //           Vector3 startPosition = Vector3.zero;
 //           startPosition.z = playerManager.playerShooting.mainWeapon.shootingDistance;

 //           line.SetPosition(1, startPosition);
 //           Debug.DrawRay(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection() * 6, Color.white);
 //          // Debug.Log("Did not Hit");
 //       }

 //       //     Debug.Log("Go");
 //       //     Debug.DrawLine(playerManager.playerShooting.mainWeapon.shootPoint.position, view.point, Color.red);

 //       //     //if (view.collider.CompareTag("Player"))
 //       //     //{
 //       //     //    player = view.collider.gameObject.GetComponent<Transform>();
 //       //     //    health = view.collider.gameObject.GetComponent<PlayerHealth>();
 //       //     //    if (Vector2.Distance(transform.position, player.position) > stoppingdistance && canmovetime <= timer)
 //       //     //    {
 //       //     //        //transform.position = player.position * speed * Time.deltaTime);
 //       //     //        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
 //       //     //    }
 //       //     //    if (player.transform.position.x < transform.position.x)
 //       //     //    {
 //       //     //        transform.localRotation = Quaternion.Euler(0, 180, 0);
 //       //     //    }
 //       //     //    else
 //       //     //    {
 //       //     //        transform.localRotation = Quaternion.Euler(0, 0, 0);
 //       //     //    }
 //       //     //    if (timer < canmovetime)
 //       //     //    {
 //       //     //        timer += Time.deltaTime;
 //       //     //    }
 //       //     //    if (Vector2.Distance(transform.position, player.position) <= stoppingdistance && timer >= canmovetime)
 //       //     //    {
 //       //     //        timer = 0;
 //       //     //        RaycastHit2D attack = Physics2D.Raycast(transform.position, transform.right);

 //       //     //        if (attack.collider.CompareTag("Player"))
 //       //     //        {
 //       //     //            health.hp -= damage;
 //       //     //        }
 //       //     //    }
 //       //     //}
 //       // }
 //       // else
 //       // {
 //       //     Debug.DrawLine(transform.position, transform.position + transform.right * crosshairPointsParent.distance, Color.green);
 //       // }
 //   }

 //   public void ChangeReloadingColor(Color color)
 //   {
 //       currentColor = color;
 //       crosshairPointsParent.currentColor = color;

 //       for (int i = 0; i < crosshairPointsParent.crossHair.Count; i++)
 //       {
 //           crosshairPointsParent.crossHair[i].pointsSprites.color = currentColor;
 //       }
 //   }

 //   public void PopulatePoints(int getDistance)//, Color getColor)
 //   {

        


 //       //if (crosshairPointsParent.pointsParent == null)
 //       //{
 //       //    crosshairPointsParent.distance = getDistance;

 //       // //   teamColor = getColor;

 //       crosshairPointsParent.pointsParent = new GameObject("Points Parent");
 //       crosshairPointsParent.pointsParent.transform.SetParent(crosshairPointsParent.gameObject.transform);

 //       //    for (int i = 0; i < crosshairPointsParent.distance; i++)
 //       //    {
 //       //        if (i < crosshairPointsParent.distance - 1)
 //       //        {
 //       //            GameObject point = Instantiate(pointPrefab, pointPrefab.transform.position, Quaternion.identity, crosshairPointsParent.pointsParent.transform);
 //       //            crosshairPointsParent.crossHair.Add(new CrosshairPointsParent.CrossHair { points = point, pointsSprites = point.GetComponent<SpriteRenderer>() });

 //       //            crosshairPointsParent.crossHair[i].points.name = "Point_" + (i + 1);
 //       //         //   crosshairPointsParent.crossHair[i].pointsSprites.color = teamColor;
 //       //        }
 //       //        else
 //       //        {

 //       //            GameObject crosshair = Instantiate(crosshairPrefab, crosshairPrefab.transform.position, Quaternion.identity, crosshairPointsParent.pointsParent.transform);
 //       //            crosshairPointsParent.crossHair.Add(new CrosshairPointsParent.CrossHair { points = crosshair, pointsSprites = crosshair.GetComponent<SpriteRenderer>() });

 //       //            crosshairPointsParent.crossHair[i].points.name = "Crosshair_" + (i + 1);
 //       //           // crosshairPointsParent.crossHair[i].pointsSprites.color = teamColor;

 //       //            crosshairPointsParent.crosshairIndex = i;
 //       //        }


 //       //    }
 //       //    crosshairPointsParent.pointsParent.SetActive(false);
 //       //}
 //       //else
 //       //{
 //       //    ChangeWeaponTrajectory(getDistance);
 //       //}

 //       //crosshair = Instantiate(crosshairPrefab, playerManager.playerShooting.mainWeapon.shootPoint.position, playerManager.playerShooting.mainWeapon.shootPoint.rotation, playerManager.playerShooting.mainWeapon.shootPoint);
 //     //  crosshair.position.z += Ve;
 //       // new Vector3(0, 0, 6);
 //       //crosshair.transform.rotation = Quaternion.Euler(0, 90, 0);
 //   }


 //   public void ChangeWeaponTrajectory(int changeDistance)
 //   {
 //       //Полученная дистанция больше текущей
 //       if (changeDistance > crosshairPointsParent.distance)
 //       {
 //           crosshairPointsParent.distance = changeDistance;

 //           if (crosshairPointsParent.distance > crosshairPointsParent.crossHair.Count)//Полученная дистанция больше максимальной дистанция
 //           {
 //               AddTrajectory(); //Увеличиваем дистанцию
 //           }
 //           else //Полученная дистанция меньше или равна максимальной дистанция
 //           {

 //               for (int i = crosshairPointsParent.crosshairIndex; i < crosshairPointsParent.distance; i++)
 //               {
 //                   crosshairPointsParent.crossHair[i].points.SetActive(true);
 //               }

 //               ChangeIndex();
 //           }
 //       }
 //       //Полученная дистанция меньше текущей
 //       else if (changeDistance < crosshairPointsParent.distance)
 //       {

 //           crosshairPointsParent.distance = changeDistance;



 //           ChangeIndex();

 //           for (int i = (int)crosshairPointsParent.distance; i < crosshairPointsParent.crossHair.Count; i++)
 //           {

 //               crosshairPointsParent.crossHair[i].points.SetActive(false);
 //           }


 //       }

 //   }

 //   private void ChangeIndex()
 //   {
 //       var buf = crosshairPointsParent.crossHair[crosshairPointsParent.crosshairIndex];
 //       var buf2 = crosshairPointsParent.crossHair[(int)crosshairPointsParent.distance - 1];
 //       crosshairPointsParent.crossHair[(int)crosshairPointsParent.distance - 1] = buf;
 //       crosshairPointsParent.crossHair[(int)crosshairPointsParent.crosshairIndex] = buf2;
 //       crosshairPointsParent.crosshairIndex = (int)crosshairPointsParent.distance - 1;
 //   }


 //   private void AddTrajectory()
 //   {

 //       for (int i = crosshairPointsParent.crossHair.Count; i < crosshairPointsParent.distance; i++)
 //       {

 //           GameObject point = Instantiate(pointPrefab, pointPrefab.transform.position, Quaternion.identity, crosshairPointsParent.pointsParent.transform);
 //           crosshairPointsParent.crossHair.Add(new CrosshairPointsParent.CrossHair { points = point, pointsSprites = point.GetComponent<SpriteRenderer>() });

 //           crosshairPointsParent.crossHair[i].points.name = "Point_" + (i + 1);
 //           //crosshairPointsParent.crossHair[i].pointsSprites.color = currentColor;

 //       }

 //       for (int i = crosshairPointsParent.crosshairIndex; i < crosshairPointsParent.distance; i++)
 //       {
 //           crosshairPointsParent.crossHair[i].points.SetActive(true);
 //       }

 //       ChangeIndex();
 //   }


 //   public void EnablePoints(bool enable)
 //   {
 //       crosshairPointsParent.pointsParent.SetActive(enable);
 //   }
}
