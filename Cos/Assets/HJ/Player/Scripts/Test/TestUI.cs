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
        CharacterController _characterController;
        PlayerController _playerController;

        [SerializeField] TextMeshProUGUI HP;
        [SerializeField] TextMeshProUGUI SP;
        [SerializeField] TextMeshProUGUI Potion;

        private void Awake()
        {
            _characterController = player.GetComponent<CharacterController>();
            _playerController = player.GetComponent<PlayerController>();
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
