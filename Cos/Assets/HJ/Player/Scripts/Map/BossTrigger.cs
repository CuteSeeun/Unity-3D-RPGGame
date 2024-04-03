using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss;
    private bool _isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered == false && other.gameObject.layer == 11)
        {
            _isTriggered = true;
            boss.GetComponent<Animator>().SetTrigger("isStand");
            SFX_Manager.Instance.BGMPLAY(6);
        }
    }
}
