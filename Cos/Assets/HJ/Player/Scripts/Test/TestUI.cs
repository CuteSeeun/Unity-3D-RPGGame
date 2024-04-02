using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HJ
{
    public class TestUI : MonoBehaviour
    {
        [SerializeField] GameObject player;
        CharacterControllerForTest _characterController;
        PlayerControllerForTest _playerController;

        [SerializeField] TextMeshProUGUI HP;
        [SerializeField] TextMeshProUGUI SP;
        [SerializeField] TextMeshProUGUI Potion;

        private void Awake()
        {
            _characterController = player.GetComponent<CharacterControllerForTest>();
            _playerController = player.GetComponent<PlayerControllerForTest>();
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            HP.text = Mathf.FloorToInt(_characterController.hp).ToString();
            SP.text = Mathf.FloorToInt(_playerController.stamina).ToString();
            Potion.text = _playerController.potionNumber.ToString();
        }
    }
}
