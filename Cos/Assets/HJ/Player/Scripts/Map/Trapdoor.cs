using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] float enemyTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public IEnumerator TrapdoorOn()
    {
        _animator.SetTrigger("isOpen");
        foreach (var enemy in enemies)
        {
            // 적 스폰
            yield return new WaitForSeconds(enemyTime);
        }
    }
}
