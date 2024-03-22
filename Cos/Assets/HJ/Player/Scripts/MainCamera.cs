using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class MainCamera : MonoBehaviour
    {
        private Transform _transform;
        [SerializeField] Transform _playerTransform;
        [SerializeField] Vector3 _cameraOffset;

        void Start()
        {
            _transform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            _transform.position = _playerTransform.position + _cameraOffset;
        }
    }
}
