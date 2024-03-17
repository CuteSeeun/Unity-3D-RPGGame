using UnityEngine;

public class EliteEnemyAIr : MonoBehaviour
{
    public float jumpForce = 50f;
    public float damageRadius = 3f;
    public int damageAmount = 10;
    public float jumpCooldown = 5f;
    public float fallSpeed = 100f;

    private Rigidbody rb;
    private Animator animator;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private bool isFall = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 점프 타이머 업데이트
        jumpTimer += Time.deltaTime;

        if (!isJumping && jumpTimer >= jumpCooldown)
        {
            animator.SetTrigger("Jump");
            isJumping = true;
            Invoke("Jump", 0.5f); // Update 메서드에서 호출하지 않습니다.
        }
    }

    private void FixedUpdate()
    {
        // Fixed Update 메서드에서 공중에서 떨어지는 로직을 처리합니다.
        if (isJumping)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity = Vector3.down * fallSpeed;         
                animator.SetBool("Fall",true);
                
                if (transform.position.y <= 0)
                {
                    isJumping = false;
                    animator.SetBool("Fall", false);
                    Debug.Log("착지" + isJumping);
                    jumpTimer = 0f;
                }
            }
        }
        if(transform.position.y > 30)
        {
            rb.velocity = Vector3.down * fallSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    private void Jump()
    {
        // 점프 애니메이션 재생
        

        // 보스 몬스터를 공중으로 올리고 점프 상태로 설정
        rb.velocity = Vector3.up * jumpForce;
        Debug.Log("점프" + isJumping);
         // 점프 후 타이머 초기화
    }

    /* Animation Event로 호출됨
    public void DealDamage()
    {
        // 주변에 있는 플레이어에게 데미지를 줌
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            }
        }
    }
    */
}
