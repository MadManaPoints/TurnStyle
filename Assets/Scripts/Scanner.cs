using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.SearchService;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    SceneManager gm; 
    int tries = 0; 
    [SerializeField] Texture blue;
    [SerializeField] Texture red;
    void Start()
    {
        gm = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
    }

    void Update()
    {
        if(tries >= 3){
            gm.scanError = true;
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Phone"){
            tries += 1;
            GetComponent<Renderer>().material.SetTexture("_MainTex", red);
        }
    }

    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Phone"){
            GetComponent<Renderer>().material.SetTexture("_MainTex", blue);
        }
    }
}
