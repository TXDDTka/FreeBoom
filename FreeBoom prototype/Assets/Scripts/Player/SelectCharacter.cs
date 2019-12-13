using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private PlayerController[] characters = null;
    [SerializeField] private Button[] selectButtons = null;

    private void Start()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            var player = characters[i];
            selectButtons[i].onClick.AddListener(() => Select(player));
        }
    }

    private void Select(PlayerController character)
    {
        Instantiate(character, spawnPoint.position, Quaternion.identity);
    }
}
