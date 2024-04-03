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
            if (enemy.TryGetComponent(out CloseEnemyAI closeEnemy))
            {
                closeEnemy.isAct = true;
            }
            else if (enemy.TryGetComponent(out LongEnemyAI longEnemy))
            {
                longEnemy.isAct = true;
            }
            else if (enemy.TryGetComponent(out MageEnemyAI mageEnemy))
            {
                mageEnemy.isAct = true;
            }
            else if (enemy.TryGetComponent(out SpawnEnemyAI spawnEnemy))
            {
                enemy.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(enemyTime);
        }
    }
}
