using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    Animator anime;
    private float broken = 3f;
    public GameObject effectPrefab;
    private bool delay = false;
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {

            anime.SetTrigger("shaker");
            Invoke("Effect", 1.5f);
            Invoke("dest", broken);
        }
    }
    private void Start() => anime = GetComponent<Animator>();

    private void Effect()
    {
        if (effectPrefab != null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effectInstance, 1f);
        }
    }

    private void dest()
    {
        Destroy(this.gameObject);
    }
}
