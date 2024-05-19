using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class End : MonoBehaviour
{
    PlayerMovement player;
    bool end;
    [SerializeField] Image fade;
    [SerializeField] GameObject thanks;
    Color black;

    void Start()
    {
        black = fade.color;
        thanks.SetActive(false);

        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(end){
            fade.color = black;
            if(black.a < 1.0f){
                black.a += Time.deltaTime;
            } else {
                black.a = 1.0f;
                thanks.SetActive(true);
                player.canRestart = true;
            }
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            end = true;
        }
    }
}
