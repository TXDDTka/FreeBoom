using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankHealthHandler : MonoBehaviour
{

    //public static BankHandler bankHandler;
    // Start is called before the first frame update
    private float maxHealth = 0;
    [SerializeField] private float currentHealth = 0;
    [SerializeField] private BankSettingsDatabase bankSettingsDatabase;
    public PhotonTeams.Team bankTeam = PhotonTeams.Team.None;
    public BankData._BankLevel bankLevel = BankData._BankLevel.First;

    [Tooltip("Cмещение по осям от игрока")]
    private CanvasGroup canvasGroup;
    [SerializeField] private Transform healthCircleTransform;
    private Vector2 bankDoorPosition;
    [SerializeField] private Transform bankDoor;

    [Tooltip("Текст здоровье игрока")]
    [SerializeField]
    private Text bankHealthText = null;

    [Tooltip("Слайдер здоровье банка")]
    [SerializeField] private Slider bankHealthSlider = null;
    [SerializeField] private Image healthInnerCircleImage = null;

    public PhotonView _PV = null;
    public BankHendler _bankHendler = null;
    void Awake()
    {
        // _PV = GetComponent<PhotonView>();
        //   canvasGroup = GetComponent<CanvasGroup>();
        //  healthCircleTransform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        _bankHendler = BankHendler.Instance;
    }

    //private void OnEnable()
    //{
    //    if(BankHandler.bankHandler == null)
    //    {
    //        BankHandler.bankHandler = this;
    //    }
    //}

    void Start()
    {
        
       // CheckBankHealth();
        bankDoorPosition = bankDoor.position;
    }

    private void Update()
    {
      //  if(healthCircleTransform != null)
      //  healthCircleTransform.position = Camera.main.WorldToScreenPoint(bankDoorPosition);
    }

    private void CheckBankHealth()
    {
        foreach(var bank in bankSettingsDatabase.bankList)
        {
            if(bank.BankLevel == bankLevel)
            {
                maxHealth = bank.Health;
                currentHealth = bank.Health;
                bankHealthSlider.maxValue = bank.Health;
                bankHealthSlider.value = bank.Health;
                bankHealthText.text = bank.Health.ToString();
            }
        }
    }
   
    [PunRPC]
    public void RPC_HandleDamage(float damage)
    {
        currentHealth -= damage;
        bankHealthSlider.value = currentHealth;
        bankHealthText.text = currentHealth.ToString();
        healthInnerCircleImage.fillAmount = currentHealth/maxHealth;
        if (currentHealth <= 0 )
        {
            currentHealth = 0;
            bankHealthText.text = "0";
            //If fortress is destroyed then finish the game with the fail
            //if(PhotonNetwork.IsMasterClient)
            //{
            //    _PV.RPC("Rpc_DestroyHealthCircleAll", RpcTarget.OthersBuffered, healthCircleTransform.gameObject);
            //    PhotonNetwork.Destroy(gameObject);
            //}
            //else
            //{
            //    _PV.RPC("Rpc_DestroyHealthCircleMasterClient", RpcTarget.MasterClient, gameObject);
            //}
            _PV.RPC("Rpc_DestroyHealthCircleMasterClient", RpcTarget.MasterClient, gameObject);
            Destroy(healthCircleTransform.gameObject);
          //  PhotonNetwork.Destroy(gameObject);
            //Destroy();
        }
    }

    //private void Destroy()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        //  PhotonNetwork.Destroy(healthInnerCircleImage.gameObject);
    //        // PhotonNetwork.Destroy(gameObject);
    //        //Destroy(healthInnerCircleImage.gameObject);
    //          _PV.RPC("Rpc_DestroyHealthCircle", RpcTarget.AllViaServer);
    //        //Destroy(healthCircleTransform.gameObject);
    //       // Destroy(gameObject);
    //    }
    //    // Destroy(healthInnerCircleImage.gameObject);
    //    // Destroy(gameObject);
    //}
    [PunRPC]
    public void Rpc_DestroyHealthCircleMasterClient(GameObject bankDoor)
    {
       // _PV.RPC("Rpc_DestroyHealthCircleAll", RpcTarget.OthersBuffered, healthCicrle);
        PhotonNetwork.Destroy(bankDoor);
    }

    //[PunRPC]
    //public void Rpc_DestroyHealthCircleAll(GameObject healthCircle)
    //{
    //    Destroy(healthCircle);
    //}
}
