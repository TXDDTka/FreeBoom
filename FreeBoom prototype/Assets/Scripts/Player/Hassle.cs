using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hassle : MonoBehaviour
{

    public GameObject hassleBarLink;
   // public GameObject camera;

    private Camera cameraObject;
    private CameraFollow cameraFollow;
    private float cameraOriginalSize;
    //private GameObject player;
   // private Collider2D playerCollider;
    private Vector2 playerPosition;
    private GameObject enemy;
   // private Rigidbody2D enemyRB;
   // private Collider2D enemyCollider;
    private Vector2 enemyPosition;
    private Vector2 centerOfHassle;
    private GameObject hassleBar;
    private HassleStatus hassleStatus;

    private int impulseForce = 4;
    private bool hassleIsBlocked = false;
    private bool inHassle = false;
    private bool balanceIsChanged = false;
    [SerializeField] private int playerTouchCount = 10;
    private int enemyTouchCount = 0;
    private int time = 4;

    [SerializeField] private Vector3 offset = Vector3.zero;
    private Vector3 originalOffset = Vector3.zero;
    private PlayerManager playerManager = null;

    private void PlayerHit() // Толчок игрока
    {
       // playerTouchCount++;
      //  enemyTouchCount--;
      //  balanceIsChanged = true;
    }

    private void EnemyHit() // Толчок врага (авто)
    {
        //playerForce--;
        //enemyForce++;
        //balanceIsChanged = true;
    }

    private void Timer() // Просто таймер (для отсчёта длины стычки)
    {
       // time--;
    }

    private void UnblockHassle() // Задержка разблокировки для следующей тычки
    {
        hassleIsBlocked = false;
    }

    void Awake() // Инициализация
    {
        //player = gameObject;
        // playerCollider = player.GetComponent<Collider2D>();
        cameraFollow = CameraFollow.Instance;
        cameraObject = CameraFollow.Instance.GetComponent<Camera>();
        playerManager = GetComponent<PlayerManager>();
        enemyTouchCount = 10;
    }

    void Update()
    {
        if (!playerManager.PV.IsMine) return;
        if (inHassle)
        {
            if (Input.GetMouseButtonDown(0)) // Клик игрока
            {
                PlayerHit();
            }
            if (balanceIsChanged) // Отрисовка полосы
            {
                hassleStatus.SetBalance(playerTouchCount, enemyTouchCount);
                balanceIsChanged = false;
            }
            if (playerTouchCount == 0 || enemyTouchCount == 0 || time == 0) // Конец стычки
            {
                // Востановление параметров
                CancelInvoke("Timer");
                CancelInvoke("EnemyHit");
                playerManager.rb.constraints = RigidbodyConstraints2D.None;
                playerManager.boxCollider.enabled = true;
              //  enemyRB.constraints = RigidbodyConstraints2D.None;
              //  enemyCollider.enabled = true;

                if (playerTouchCount == 0) // Победа врага
                {
                    // ЗАМЕНИТЬ НА ПОБЕДУ ВРАГА
                    if (playerPosition.x > enemyPosition.x)
                    {
                        playerManager.rb.AddForce(transform.right * impulseForce, ForceMode2D.Impulse);
                    //    enemyRB.AddForce(transform.right * -impulseForce, ForceMode2D.Impulse);
                    }
                    else
                    {
                        playerManager.rb.AddForce(transform.right * -impulseForce, ForceMode2D.Impulse);
                     //   enemyRB.AddForce(transform.right * impulseForce, ForceMode2D.Impulse);
                    }
                    Debug.Log("Red win");
                }
                if (enemyTouchCount == 0) // Победа игрока
                {
                    // ЗАМЕНИТЬ НА ПОБЕДУ ИГРОКА
                    if (playerPosition.x > enemyPosition.x)
                    {
                        playerManager.rb.AddForce(transform.right * impulseForce, ForceMode2D.Impulse);
                     //   enemyRB.AddForce(transform.right * -impulseForce, ForceMode2D.Impulse);
                    }
                    else
                    {
                        playerManager.rb.AddForce(transform.right * -impulseForce, ForceMode2D.Impulse);
                     //   enemyRB.AddForce(transform.right * impulseForce, ForceMode2D.Impulse);
                    }
                    Debug.Log("Blue win");
                }
                if (time == 0) // Ничья
                {
                    // ЗАМЕНИТЬ НА УРОН ОБЕИМ СТОРОНАМ
                    if (playerPosition.x > enemyPosition.x) // Расставляем объекты для стычки
                    {
                        playerManager.rb.AddForce(transform.right * impulseForce, ForceMode2D.Impulse);
                      //  enemyRB.AddForce(transform.right * -impulseForce, ForceMode2D.Impulse);
                    }
                    else
                    {
                        playerManager.rb.AddForce(transform.right * -impulseForce, ForceMode2D.Impulse);
                      //  enemyRB.AddForce(transform.right * impulseForce, ForceMode2D.Impulse);
                    }
                    Debug.Log("Draft");
                }

                // Востановление параметров
              //  Destroy(hassleBar);
                cameraObject.orthographicSize = cameraOriginalSize;
                balanceIsChanged = false;
                inHassle = false;
                time = 3;
                Invoke("UnblockHassle", 3);
            }

        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
      //   if (!playerManager.PV.IsMine) return;
        //  if (collision.gameObject.tag == "Enemy" )
        if (collision.gameObject.tag == "Player" && !hassleIsBlocked)
        {
            Debug.Log("Столкнулись");
            if (collision.gameObject.GetComponent<PhotonView>().Owner.GetTeam() != PhotonNetwork.LocalPlayer.GetTeam())
            {
               // if (playerManager.PV.IsMine) {
                    Debug.Log("Враги");
                    hassleIsBlocked = true;
                    inHassle = true; // Запрет на стычки

                    //                   enemy = collision.gameObject;                           // Получаем оппонента
                    //                   enemyRB = enemy.GetComponent<Rigidbody2D>();            // Получаем RigidBody
                    //                   enemyRB.constraints = RigidbodyConstraints2D.FreezeAll; // Замораживаем врага
                    //                  enemyCollider = enemy.GetComponent<Collider2D>();       // Получаем Collider
                    //                   enemyCollider.enabled = false;                          // Отключаем Collider
                    enemyPosition = collision.transform.position;
                    Debug.Log(enemyPosition);
                     playerManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;// Замораживаем игрока
                     playerManager.boxCollider.enabled = false;                         // Отключаем Collider
                 //   playerManager.moveJoystick.MoveJoystickPointerUp();
                 //   playerManager.shootJoystick.ShootJoystickPointerUp();
                // playerManager.PV.RPC("FreezePlayer", RpcTarget.AllBuffered, new object[] { RigidbodyConstraints2D.FreezeAll, false, true });
                //  playerManager.rb.isKinematic = true;

                playerPosition = transform.position;             // Записываем координаты игрока
                                                                     //  enemyPosition = enemy.transform.position;               // Записываем координаты врага
                    centerOfHassle = enemyPosition / 2 + playerPosition / 2;// Находим центр стычки


                    if (playerPosition.x > enemyPosition.x) // Расставляем объекты для стычки
                    {
                        Debug.Log("Выше");
                        transform.position = new Vector2(centerOfHassle.x + 0.5f, centerOfHassle.y);
                        //  enemy.transform.position = new Vector2(centerOfHassle.x - 0.5f, centerOfHassle.y);
                    }
                    else
                    {
                        Debug.Log("Ниже");
                        transform.position = new Vector2(centerOfHassle.x - 0.5f, centerOfHassle.y);
                        //   enemy.transform.position = new Vector2(centerOfHassle.x + 0.5f, centerOfHassle.y);
                    }

                    hassleStatus = Instantiate(hassleBarLink, new Vector2(centerOfHassle.x, centerOfHassle.y + 1), Quaternion.identity).GetComponent<HassleStatus>(); // Создание полосы стычки,получаем скрипт бара
                    //hassleStatus = hassleBar.GetComponent<HassleStatus>(); // Получение скрипта полосы

                    playerTouchCount = 10;                                   // Задаются базовые парамметры для стычки
                    enemyTouchCount = 10;
                    hassleStatus.SetBalance(playerTouchCount, enemyTouchCount);   // Отрисовывается ползунок

                    cameraObject.transform.position = new Vector3(centerOfHassle.x, centerOfHassle.y, cameraObject.transform.position.z);   // Камера занимает позицию центра стычки
                    cameraOriginalSize = cameraObject.orthographicSize;   // Запись предыдущего положения камеры
                    originalOffset = cameraFollow.offset; // Запись предыдущего положения отступов камеры
                    cameraObject.orthographicSize = 3;    // Приближает изображение
                    cameraFollow.offset = offset;         // Меняем отступы у камеры,варавниваем положение
                    InvokeRepeating("EnemyHit", 1, 0.2f);   // Включается автоклик у врага
                    InvokeRepeating("Timer", 1, 1f);      // Включается таймер на стычку
                }
        //    }
        }
        }
 
    [PunRPC]
    public void FreezePlayer(RigidbodyConstraints2D constraints2D, bool colliderOn, bool isKinematic)
    {
        playerManager.rb.constraints = constraints2D;// Замораживаем игрока
        playerManager.boxCollider.enabled = colliderOn; 
        playerManager.rb.isKinematic = isKinematic;
    }

}
