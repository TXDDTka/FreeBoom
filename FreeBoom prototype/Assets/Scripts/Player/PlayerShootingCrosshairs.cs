using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingCrosshairs : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerManager playerManager = null;
    public enum CrosshairType { None, LineCrosshair }

    public CrosshairType crosshairTypeMainWeapon = CrosshairType.None;
    public CrosshairType crosshairTypeSecondWeapon = CrosshairType.None;

    public float shootingDistance = 0f;
    private GameObject crosshair;
    [SerializeField] private LayerMask[] layer;

    public Transform[] crosshairs;
    public LineRenderer line;

    public Vector3 localPosition = Vector3.zero;
    public Quaternion localRotation = Quaternion.identity;

    [SerializeField] FieldOfView fieldOfView;
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        fieldOfView = FieldOfView.Instance;
    }

    void Update()
    {
        //   RayCast();
      //  fieldOfView.SetAimDirection(playerManager.shootJoystick.Direction);
       // fieldOfView.SetOriging(playerManager.playerShooting.mainWeapon.shootPoint.position);
    }

    public Vector3 GetLookPosition()
    {
        Vector3 look = Vector3.zero;

        Vector3 aimPos = transform.position + Vector3.up * 1.3f;


        if (playerManager.shootJoystick.HasInput)
        {
            look = aimPos + playerManager.shootJoystick.Direction.normalized * 2;
        }
        else
        {
            look = aimPos;
            float x = playerManager.playerMovement.isFacingRight ? 2 : -2;
            look.x += x;
        }


        return look;
    }


    private void Start()
    {
        //Vector3 targetPosition = GetMouseWorldPosition();


       // fieldOfView.SetAimDirection(playerManager.shootJoystick.Direction);
        //fieldOfView.SetOriging(playerManager.playerShooting.mainWeapon.shootPoint.position);
    }

    // Get Mouse Position in World with Z = 0f

    public void CheckCrosshair()
    {
        if (playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon)
        {
            switch (crosshairTypeMainWeapon)
            {
                case CrosshairType.LineCrosshair:
                    if(line == null)
                    { 
                        line = Instantiate(crosshairs[0], crosshairs[0].transform.position, crosshairs[0].transform.rotation, playerManager.playerShooting.mainWeapon.shootPoint).GetComponent<LineRenderer>();
                        // line.transform.SetParent(playerManager.playerShooting.secondWeapon.shootPoint,false);
                        // line.transform.localPosition = localPosition;
                        // line.transform.localRotation = localRotation;
                        Debug.Log(playerManager.shootJoystick.Direction.normalized);
                        crosshair = line.gameObject;
                        line.SetPosition(0, playerManager.playerShooting.mainWeapon.shootPoint.position);
                        line.SetPosition(1, playerManager.playerShooting.mainWeapon.shootPoint.position + playerManager.shootJoystick.Direction.normalized * playerManager.playerShooting.mainWeapon.shootingDistance);
                        //line.SetPosition(1, playerManager.playerShooting.mainWeapon.shootPoint.forward * playerManager.playerShooting.mainWeapon.shootingDistance);
                        Debug.Log(playerManager.playerShooting.mainWeapon.shootingDistance);
                        Debug.Log("mAIN1");
                    }
                    else
                    {
                        Debug.Log("mAIN2");
                        // LineRendererAdvancedOptions lineRendererAdvancedOptions = line.GetComponent<LineRendererAdvancedOptions>();
                        //lineRendererAdvancedOptions.ChangeTransform();
                        Debug.Log(playerManager.playerShooting.mainWeapon.shootingDistance);
                        line.transform.SetParent(playerManager.playerShooting.mainWeapon.shootPoint);
                        line.SetPosition(0, CrosshairStartPosition());
                    }
                    break;
            }
        }
        else
        {
            switch (crosshairTypeSecondWeapon)
            {
                case CrosshairType.LineCrosshair:
                    if(line == null)
                    {
                        line = Instantiate(crosshairs[0], playerManager.playerShooting.secondWeapon.shootPoint.position, Quaternion.identity, playerManager.playerShooting.secondWeapon.shootPoint).GetComponent<LineRenderer>();
                        crosshair = line.gameObject;
                        line.SetPosition(0, CrosshairStartPosition());
                        Debug.Log(playerManager.playerShooting.secondWeapon.shootingDistance);
                        Debug.Log("sECOND1");
                    }
                    else
                    {
                        Debug.Log("sECOND2");
                       // LineRendererAdvancedOptions lineRendererAdvancedOptions = line.GetComponent<LineRendererAdvancedOptions>();
                      //  lineRendererAdvancedOptions.ChangeTransform();
                        line.transform.SetParent(playerManager.playerShooting.secondWeapon.shootPoint);
                        Debug.Log(playerManager.playerShooting.secondWeapon.shootingDistance);
                        line.SetPosition(0, CrosshairStartPosition());
                    }
                    break;
            }
        }
    }

    private Vector3 CrosshairStartPosition()
    {
        Vector3 startPosition = Vector3.zero;
        startPosition.z = playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon ? playerManager.playerShooting.mainWeapon.shootingDistance : playerManager.playerShooting.secondWeapon.shootingDistance;
       // Vector3 startPosition = playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon ? playerManager.playerShooting.mainWeapon.shootPoint.position : playerManager.playerShooting.secondWeapon.shootPoint.position;

        return startPosition;
    }

    public Vector3 JoystickDirection()
    {
        Vector3 dir = playerManager.shootJoystick.Direction.x > 0 || playerManager.shootJoystick.Direction.y > 0 ? playerManager.shootJoystick.Direction.normalized : playerManager.shootJoystick.Direction;
        return dir;
    }


    public void RayCast()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection(), out hit/*, Mathf.Infinity*/, 6, layer[0]) ||
            Physics.Raycast(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection(), out hit/*, Mathf.Infinity*/, 6, layer[1]))
        {

            Vector3 startPosition = Vector3.zero;
            startPosition.z = hit.distance;

           // line.SetPosition(0, playerManager.playerShooting.mainWeapon.shootPoint.position);
            line.SetPosition(1, playerManager.playerShooting.mainWeapon.shootPoint.forward * playerManager.playerShooting.mainWeapon.shootingDistance);

            Debug.DrawRay(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection() * hit.distance, Color.yellow);
            Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            Vector3 startPosition = Vector3.zero;
            startPosition.z = playerManager.playerShooting.mainWeapon.shootingDistance;

            line.SetPosition(1, startPosition);
            Debug.DrawRay(playerManager.playerShooting.mainWeapon.shootPoint.position, JoystickDirection() * 6, Color.white);
            // Debug.Log("Did not Hit");
        }

    }

    public void ChangeColor(Color color)
    {
        line.startColor = color;
        line.endColor = color;
    }

    public void EnablePoints(bool enable)
    {
        crosshair.gameObject.SetActive(enable);
    }
}
