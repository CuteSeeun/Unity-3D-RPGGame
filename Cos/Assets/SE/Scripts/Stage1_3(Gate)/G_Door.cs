using GSpawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Door : MonoBehaviour
{
    private Animator door; //애니메이터 컴포넌트 참조
    public bool isOpen; //문 초기 상태 닫힘

    public GameObject Light1;
    public GameObject Light2;
    public GameObject DoorLock;

    private void Start()
    {
        door = GetComponent<Animator>(); //가져오기: 스크립트에서 애니메이션 제어.
    }

    private void Update()
    {
        Light light1 = Light1.GetComponent<Light>();
        Light light2 = Light2.GetComponent<Light>();

        if (light1.enabled && light2.enabled)
        {
            DoorLock.SetActive(false);

            if (Input.GetKeyDown(KeyCode.F))
            {
                isOpen = true;
                door.SetBool("isOpen", true);
            }
        }
    }
}
