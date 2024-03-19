using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float hp = 1;
    bool isDestroy;
    public GameObject bombEffect;


    private void Start()
    {

    }
    private void Update()
    {
        if (hp <= 0 && !isDestroy)
        {
            isDestroy = true;
            Instantiate(bombEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    
}