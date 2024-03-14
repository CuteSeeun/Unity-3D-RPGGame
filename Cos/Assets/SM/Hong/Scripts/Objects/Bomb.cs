using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator animator;
    public float hp = 1;
    ParticleSystem bombEffect;

    private void Start()
    {
        bombEffect = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        if(hp <= 0)
        {
            animator.SetBool("isBomb", true);
            Invoke("Destroy", 2);
        }
    }

    void Destroy()
    {
        
        Destroy(gameObject);
    }
}