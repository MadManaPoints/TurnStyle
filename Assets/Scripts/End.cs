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
    [SerializeField] GameObject gameOver;
    Color black;

    void Start()
    {
        black = fade.color;
        thanks.SetActive(false);
        gameOver.SetActive(false);

        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(player.lose){
            end = true;
        }

        if(end){
            fade.color = black;
            if(black.a < 1.0f && !player.lose){
                black.a += Time.deltaTime;
            } else {
                black.a = 1.0f;
                if(player.lose){
                    gameOver.SetActive(true);
                } else {
                    thanks.SetActive(true);
                }
                
                player.canRestart = true;
                Time.timeScale = 0;
            }
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            end = true;
        }
    }
}
