using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AniOnEvent : MonoBehaviour
{
    public DestroyObject Destroy_VFX;
    private void LateUpdate()
    {
 
        Destroy(gameObject);
    }
}





