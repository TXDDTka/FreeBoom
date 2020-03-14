using Photon.Pun;
using Photon.Realtime;
using Unity.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public  float currentHp = 0;
    [SerializeField]
    private GameObject playerUiPrefab = null;
    [SerializeField] private CharactersSettingsDatabase charactersSettings = null;
   
    private PlayerNetwork playerNetwork;
    private PlayerManager playerManager = null;

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
        playerBar = Instantiate(playerUiPrefab).GetComponent<PlayerDataBar>();
        //  playerBar.SetPlayer(this);
        playerBar.SetPlayer(transform, GetComponent<BoxCollider2D>(), playerManager.PV.Owner.NickName, currentHp);
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

    [PunRPC]
    public void SetPlayerHealth(float health)
    {
        currentHp = health;
    }

    [PunRPC]
    public void GetDamage(float damage, Player killer)
    {
        currentHp -= damage;
        playerBar.SetPlayerHealth(currentHp);
        if (currentHp <= 0)
        {

            if (!playerManager.PV.IsMine) return;  
                playerNetwork.PlayerDied(killer);
        }
    }

    
}
