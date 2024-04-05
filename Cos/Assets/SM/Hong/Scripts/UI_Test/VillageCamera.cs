using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    /// <summary>
    /// 각도를 유지한 채 플레이어를 추격하는 카메라
    /// </summary>
    public class VillageCamera : MonoBehaviour
    {
        private Transform _transform;
        public Transform player;
        [SerializeField] Vector3 _cameraOffset;

        void Start()
        {
            _transform = GetComponent<Transform>();
            if (player == null)
            {
                player = GameObject.FindWithTag("Player").transform;
            }
        }

        void Update()
        {
            _transform.position = player.position + _cameraOffset;
        }
    }
}
