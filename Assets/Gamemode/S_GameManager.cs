using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class S_GameManager : MonoBehaviour
{
    public Color[] ballColors;
    public int numberOfColorsUsed;
    public int numberOfBalls = 10;
    public float distanceBetweenBalls = 1f;
    float radius;
    public GameObject ballPrefab;
    public int score;
    public int moves;
    public int totalMoves = 10;
    public int goalBallsStarting = 5;
    public int goalBallsLeft;

    public List<GameObject> balls = new List<GameObject>();

    //field
    public GameObject fieldPlane;
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    //UI
    public S_EndGameUI endGameUI;


    // Start is called before the first frame update
    void Start()
    {
        radius = ballPrefab.transform.localScale.x / 2;
        moves = totalMoves;

        ballColors = new Color[] {
        Color.black,
        Color.cyan,
        Color.magenta,
        Color.green,
        Color.blue,
        Color.red };

        S_PlayerBall playerBall = FindObjectOfType<S_PlayerBall>();
        playerBall.GetComponent<Renderer>().material.color = ballColors[0];

        CalculateBoundaries();      
        SearchSceneForBalls();
    }

    private void CalculateBoundaries()
    {
        Vector3 planeSize = fieldPlane.transform.localScale * 10; //default Plane has a size of 10 units
        Vector3 planeCenter = fieldPlane.transform.position;

        minX = planeCenter.x - planeSize.x / 2;
        maxX = planeCenter.x + planeSize.x / 2;
        minZ = planeCenter.z - planeSize.z / 2;
        maxZ = planeCenter.z + planeSize.z / 2;
    }

    private void SearchSceneForBalls()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in components)
            {
                if (component is I_Ball)
                {
                    AddBall(obj);
                    break;
                }
            }
        }
    }

    public void AddBall(GameObject ball)
    {
        if (!balls.Contains(ball)) balls.Add(ball);
    }

    public void RemoveBall(GameObject ball)
    {
        if (balls.Contains(ball)) balls.Remove(ball);
    }

    public void SpawnBalls()
    {
        //number of goalballs can never exceed number of moves. This would make the game impossible to beat
        goalBallsStarting = Mathf.Min(totalMoves, goalBallsStarting);
        goalBallsLeft = goalBallsStarting;

        for (int i = 0; i < numberOfBalls; i++)
        {
            Vector3 randomPosition = FindValidPosition();
            if (randomPosition != Vector3.zero) //FindValidPosition returns zero of no position found. Vector3 can't be null
            {
                GameObject newBall = Instantiate(ballPrefab, randomPosition, Quaternion.identity);

                //to assure there are a certain number of target balls equal to the total number of moves
                if (i < goalBallsStarting || numberOfColorsUsed == 1) newBall.GetComponent<Renderer>().material.color = ballColors[0];
                else newBall.GetComponent<Renderer>().material.color = ballColors[Random.Range(1, numberOfColorsUsed)];

                AddBall(newBall);
            }
        }
    }

    //returns Vector3.zero if no valid position found
    private Vector3 FindValidPosition()
    {
        int maxAttempts = 1000;
        int attempts = 0;
        Vector3 randomPosition = Vector3.zero;

        do
        {
            randomPosition = new Vector3(Random.Range(minX, maxX), ballPrefab.transform.localScale.x/2, Random.Range(minZ, maxZ));
            bool validPosition = true;

            foreach (GameObject ball in balls)
            {
                if (Vector3.Distance(randomPosition, ball.transform.position) < distanceBetweenBalls + (2 * radius))
                {
                    //too close to another ball
                    validPosition = false;
                }
            }

            if (validPosition)
            {
                //valid position found
                return randomPosition;
            }
        }
        while (attempts < maxAttempts);

        Debug.LogWarning("Failed to find a valid position after " + maxAttempts + " attempts!");
        return Vector3.zero;
    }

    public void EndGame()
    {
        endGameUI.EndGame(goalBallsStarting, goalBallsLeft, score, totalMoves - moves);
    }

    public float getMinX() { return minX; }
    public float getMaxX() { return maxX; }
    public float getMinZ() { return minZ; }
    public float getMaxZ() { return maxZ; }
}
