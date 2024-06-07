using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class S_EndGameUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI scoreText;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    private void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void EndGame(int goalBallsStarting, int goalBallsLeft, int points, int movesUsed)
    {
        GetComponent<Canvas>().enabled = true;

        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        if (goalBallsLeft <= 0)
        {
            statusText.text = "Victory";
            scoreText.text = "You earned " + points.ToString() + " points in " + movesUsed.ToString() + " turns";


            //first star - 0 points or less
            star1.SetActive(true);
            //second star - positive points
            if (points >= 0) star2.SetActive(true);
            //third star - only hit all goal balls
            if (points == goalBallsStarting) star3.SetActive(true);

        }
        else
        {
            statusText.text = "Game Over";
            scoreText.text = "You missed " + goalBallsLeft.ToString() + " balls";
        }
    }
}
