using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankHendler : MonoBehaviour
{
    // Start is called before the first frame update
    public static BankHendler Instance { get; private set; }

    [SerializeField] private GameObject _readTeamBankDoor = null;
    [SerializeField] private GameObject _blueTeamBankDoor = null;
    [SerializeField] private Transform _redTeamBank = null;
    [SerializeField] private Transform _blueTeamBank = null;
    [SerializeField] private Vector3 _readTeamBankPosition = new Vector3();
    [SerializeField] private Vector3 _blueTeamBankPosition = new Vector3();

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        //else if (Instance != this)
        //    Destroy(this);
    }


    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
     GameObject redTeamBankDoor =   PhotonNetwork.Instantiate(_readTeamBankDoor.name, _readTeamBankDoor.transform.position, Quaternion.identity);
     redTeamBankDoor.transform.SetParent(_redTeamBank,false);

     GameObject blueTeamBankDoor =   PhotonNetwork.Instantiate(_blueTeamBankDoor.name, _blueTeamBankDoor.transform.position, Quaternion.identity);
     blueTeamBankDoor.transform.SetParent(_blueTeamBank,false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
