using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    // 원하는 축 제어.
    [SerializeField] private bool x, y, z;
    [Header("Target Transform")]
    /* 쫓아가야할 대상 */
    [SerializeField] private Transform target;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (!target) return;
        // 있으면 타겟 위치, 없으면 현재 위치.
        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
}
