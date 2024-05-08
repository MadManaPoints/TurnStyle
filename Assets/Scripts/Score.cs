using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] TextMeshProUGUI scoreText;
    public float score = 290;
    int finalScore;

    void Start(){
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update(){
        finalScore = Mathf.RoundToInt(score);

        if(player.end){
            scoreText.text = "Score: " + finalScore;
        } else {
            scoreText.text = "All trains delayed";
        }
    }
    public void SubtractScore(){
        //Debug.Log(score);
        score -= 50 * Time.deltaTime;
    }
}
