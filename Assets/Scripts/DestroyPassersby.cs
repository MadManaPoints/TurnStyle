using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPassersby : MonoBehaviour
{

    void Update()
    {
        if(transform.position.z > 31.0f || transform.position.z < -5.5f){
            Destroy(gameObject); 
        }
    }
}
