using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingCrosshairs : MonoBehaviour
{
    // Start is called before the first frame update
  //  public PlayerManager playerManager = null;
  //  public enum CrosshairType { None, LineCrosshair, ShotgunСrosshair }

  //  public CrosshairType crosshairTypeMainWeapon = CrosshairType.None;
  //  public CrosshairType crosshairTypeSecondWeapon = CrosshairType.None;

  //  public CrosshairType currentCrosshair = CrosshairType.None;

  //  public float shootingDistance = 0f;
  //  private GameObject crosshair;
  //  [SerializeField] private LayerMask[] layer;

  //  public Transform[] crosshairs;
  ////  public LineRenderer line;

  //  public Vector3 localPosition = Vector3.zero;
  //  public Quaternion localRotation = Quaternion.identity;

  //  [SerializeField] FieldOfView fieldOfView;
  //  [SerializeField] LineRendererAdvancedOptions lineRendererAdvancedOptions;
  //  private void Awake()
  //  {
  //      playerManager = GetComponent<PlayerManager>();
  //      fieldOfView = FieldOfView.Instance;
  //      lineRendererAdvancedOptions = LineRendererAdvancedOptions.Instance;
  //  }

  //  void Update()
  //  {
  //      //   RayCast();
  //      //  fieldOfView.SetAimDirection(playerManager.shootJoystick.Direction);
  //      // fieldOfView.SetOriging(playerManager.playerShooting.mainWeapon.shootPoint.position);
  //      // lineRendererAdvancedOptions.SetParemeters(this);
  //      // lineRendererAdvancedOptions.SetParemeters(this);//(playerManager.playerShooting.mainWeapon.shootPoint.position, playerManager.shootJoystick.Direction.normalized, playerManager.playerShooting.mainWeapon.shootingDistance);

  //  //    if (playerManager.shootJoystick.HasInput)
  //  //    {
  //  //        // originPosition = playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon ? playerManager.playerShooting.mainWeapon.shootPoint.position : playerManager.playerShooting.secondWeapon.shootPoint.position;
  //  //        //  finalVelocity = playerManager.shootJoystick.Direction * trajectoryVelocity;
  //  //        // ShowTrajectory();
  //  //        if (currentCrosshair == CrosshairType.LineCrosshair)
  //  //        {
  //  //            lineRendererAdvancedOptions.SetParemeters(playerManager.playerShooting.mainWeapon.shootPoint.position, playerManager.shootJoystick.Direction.normalized, playerManager.playerShooting.mainWeapon.shootingDistance);
  //  //        }
  //  //        else if(currentCrosshair == CrosshairType.ShotgunСrosshair)
  //  //        {

  //  //        }
  //  //    }
  //  //}



  //  private void Start()
  //  {
  //      //Vector3 targetPosition = GetMouseWorldPosition();


  //      // fieldOfView.SetAimDirection(playerManager.shootJoystick.Direction);
  //      //fieldOfView.SetOriging(playerManager.playerShooting.mainWeapon.shootPoint.position);
        
  //  }

  //  // Get Mouse Position in World with Z = 0f

   

  //  public void CheckCrosshair()
  //  {
  //      if (playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon)
  //      {
  //          switch (crosshairTypeMainWeapon)
  //          {
  //              case CrosshairType.LineCrosshair:
  //                  //if (line == null)
  //                  //{

  //                  //}
  //                  //else
  //                  //{
  //                  //    Debug.Log("mAIN2");
  //                  //    // LineRendererAdvancedOptions lineRendererAdvancedOptions = line.GetComponent<LineRendererAdvancedOptions>();
  //                  //    //lineRendererAdvancedOptions.ChangeTransform();
  //                  //    Debug.Log(playerManager.playerShooting.mainWeapon.shootingDistance);
  //                  //  //  line.transform.SetParent(playerManager.playerShooting.mainWeapon.shootPoint);
  //                  // //   line.SetPosition(0, CrosshairStartPosition());
  //                  //}
  //                  break;
  //          }
  //      }
  //      else
  //      {
  //          switch (crosshairTypeSecondWeapon)
  //          {
  //              case CrosshairType.LineCrosshair:
  //                  //if (line == null)
  //                  //{
  //                  //    line = Instantiate(crosshairs[0], playerManager.playerShooting.secondWeapon.shootPoint.position, Quaternion.identity, playerManager.playerShooting.secondWeapon.shootPoint).GetComponent<LineRenderer>();
  //                  //    crosshair = line.gameObject;
  //                  //   // line.SetPosition(0, CrosshairStartPosition());
  //                  //    Debug.Log(playerManager.playerShooting.secondWeapon.shootingDistance);
  //                  //    Debug.Log("sECOND1");
  //                  //}
  //                  //else
  //                  //{
  //                  //    Debug.Log("sECOND2");
  //                  //    // LineRendererAdvancedOptions lineRendererAdvancedOptions = line.GetComponent<LineRendererAdvancedOptions>();
  //                  //    //  lineRendererAdvancedOptions.ChangeTransform();
  //                  //    line.transform.SetParent(playerManager.playerShooting.secondWeapon.shootPoint);
  //                  //    Debug.Log(playerManager.playerShooting.secondWeapon.shootingDistance);
  //                  //  //  line.SetPosition(0, CrosshairStartPosition());
  //                  //}
  //                  break;
  //          }
  //      }
  //  }

  //  //private Vector3 CrosshairStartPosition()
  //  //{
  //  //    Vector3 startPosition = Vector3.zero;
  //  //    startPosition.z = playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon ? playerManager.playerShooting.mainWeapon.shootingDistance : playerManager.playerShooting.secondWeapon.shootingDistance;
  //  //   // Vector3 startPosition = playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon ? playerManager.playerShooting.mainWeapon.shootPoint.position : playerManager.playerShooting.secondWeapon.shootPoint.position;

  //  //    return startPosition;
  //  //}

  //  //public Vector3 JoystickDirection()
  //  //{
  //  //    Vector3 dir = playerManager.shootJoystick.Direction.x > 0 || playerManager.shootJoystick.Direction.y > 0 ? playerManager.shootJoystick.Direction.normalized : playerManager.shootJoystick.Direction;
  //  //    return dir;
  //  //}


  //  //public void RayCast()
  //  //{
  //  //    RaycastHit hit;

  //  //    if (Physics.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection(), out hit/*, Mathf.Infinity*/, 6, layer[0]) ||
  //  //        Physics.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection(), out hit/*, Mathf.Infinity*/, 6, layer[1]))
  //  //    {

  //  //        Vector3 startPosition = Vector3.zero;
  //  //        startPosition.z = hit.distance;

  //  //       // line.SetPosition(0, playerManager.playerShooting.mainWeapon.shootPoint.position);
  //  //        line.SetPosition(1, playerManager.playerShooting.mainWeapon.shootPoint.forward * playerManager.playerShooting.mainWeapon.shootingDistance);

  //  //        Debug.DrawRay(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection() * hit.distance, Color.yellow);
  //  //        Debug.Log(hit.collider.gameObject.name);
  //  //    }
  //  //    else
  //  //    {
  //  //        Vector3 startPosition = Vector3.zero;
  //  //        startPosition.z = playerManager.playerShooting.mainWeapon.shootingDistance;

  //  //        line.SetPosition(1, startPosition);
  //  //        Debug.DrawRay(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection() * 6, Color.white);
  //  //        // Debug.Log("Did not Hit");
  //  //    }

  //  //}

  //  public void ChangeColor(Color color)
  //  {
  //    //  line.startColor = color;
  //    //  line.endColor = color;
  //  }

  //  //public void EnablePoints(bool enable)
  //  //{
  //  //    //  crosshair.gameObject.SetActive(enable);
  //  //    lineRendererAdvancedOptions.LineRendererObject.enabled = enable;
  //  //}
}
