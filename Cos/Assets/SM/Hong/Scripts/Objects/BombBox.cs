using UnityEngine;

public class BombBox : MonoBehaviour
{
    Animator animator;
    public float hp = 1;
    public GameObject bombEffect;
    float explosionRange = 3;
    public int damegeAmount = 5;
    bool isBomb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        if (hp <= 0 && !isBomb)
        {
            isBomb = true;
            animator.SetBool("isBomb", true);
            Invoke("Explosion", 2);
        }
    }

    void Explosion()
    {
        Instantiate(bombEffect, transform.position, Quaternion.identity);
        Collider[] other = Physics.OverlapSphere(transform.position, explosionRange);

        foreach(Collider collider in other)
        {
            //범위 내 대상에게 데미지 부여
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,explosionRange);
    }

    void Hit()
    {
        //데미지 가하는 코드 구현
    }
}