using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    SceneManager gm; 
    PlayerMovement player;
    int tries = 0; 
    [SerializeField] Texture blue;
    [SerializeField] Texture red;
    public bool beepBoop;
    void Start()
    {
        gm = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
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
            beepBoop = true;
            GetComponent<Renderer>().material.SetTexture("_MainTex", red);
        }
    }

    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Phone"){
            beepBoop = false;
            GetComponent<Renderer>().material.SetTexture("_MainTex", blue);
        }
    }
}
