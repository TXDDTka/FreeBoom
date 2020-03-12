using Photon.Pun;
using Photon.Realtime;
using Unity.Collections;
using UnityEngine;


[RequireComponent(typeof(PlayerManager))]
public class PlayerHealth : MonoBehaviour
{

    public  float currentHp = 0;
    public float currentShield = 0;
    [SerializeField]
    private GameObject playerUiPrefab = null;
    [SerializeField] private CharactersSettingsDatabase charactersSettings = null;
   
    private PlayerNetwork playerNetwork;
    public PlayerManager playerManager = null;

    public PlayerDataBar playerBar;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerNetwork = PlayerNetwork.Instance;
    }

    private void Start()
    {
        if (!playerManager.PV.IsMine) return;
            CheckCharacterHealth();
        playerManager.PV.RPC("CreatePlayerBar", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void CreatePlayerBar()
    {
        playerBar = Instantiate(playerUiPrefab).GetComponent<PlayerDataBar>(); ;
        playerBar.SetPlayer(this);
    }

   // [PunRPC]
    private void CheckCharacterHealth()
    {
        for (int i = 0; i < charactersSettings.charactersList.Count; i++)
        {
            var character = charactersSettings.charactersList[i];
            if (character.CharacterName == playerManager.player.GetCharacter().ToString())
            {
                playerManager.PV.RPC("SetPlayerHealth", RpcTarget.AllBuffered, character.Health);
                return;
            }
        }
    }

    public void GetHealthPoint(float addHP)
    {
        currentHp += addHP;

        if (currentHp > 200)
        {
            currentHp = 200;
        }
        Debug.Log($"Add hp: {addHP}");
        Debug.Log($"Current hp: {currentHp}");
    }

    public void GetShieldPoint(float addShield)
    {
        currentShield += addShield;

        if (currentShield > 200)
        {
            currentShield = 200;
        }
        Debug.Log($"Add Sheild point: { addShield}");
        Debug.Log($"Current Shield point: {currentShield}");
    }

    [PunRPC]
    public void SetPlayerHealth(float health)
    {
        currentHp = health;
    }

    [PunRPC]
    public void GetDamage(float damage, Player killer)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {

            if (!playerManager.PV.IsMine) return;  
                playerNetwork.PlayerDied(killer);
        }
    }
}
