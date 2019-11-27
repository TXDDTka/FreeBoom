using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

namespace Photon
{
    /// Поле ввода имени игрока. Пусть пользователь введет свое имя, появится над игроком в игре.
    //Сначала мы удостоверимся, что этот скрипт принудительно вводит InputField, потому что нам это нужно, это очень удобный и быстрый способ гарантировать беспроблемное использование этого скрипта.
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {

        // Сохраняем ключ PlayerPref, чтобы избежать опечаток
        const string playerNamePrefKey = "PlayerName";


        void Start()
        {
            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();
            if (_inputField != null)
            {
                //PlayerPrefs это простой список поиска парных записей (например, лист Excel с двумя столбцами), один - ключ, другой - значение
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }


        //Устанавливает имя игрока и сохраняет его в PlayerPrefs для будущих сессий.
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;


            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
    }
}
