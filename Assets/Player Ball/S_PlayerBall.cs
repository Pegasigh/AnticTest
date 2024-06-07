using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_PlayerBall : MonoBehaviour, I_Ball
{
    public float deceleration = 0.99f;
    public float maxInitialSpeed = 10.0f;
    private Vector3 velocity;
    public bool isMoving = false;
    public bool stopsAtWall = true;

    public GameObject dragHandle;
    public S_GameManager gameManager;

    public List<GameObject> collisions;


    void Update()
    {
        if (isMoving) MoveBall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            collisions.Add(other.gameObject);
        }
    }

    void MoveBall()
    {
        if (velocity.magnitude < 0.1f)
        {
            //if moving slow enough, ball stops
            isMoving = false;
            velocity = Vector3.zero;
            gameManager.moves--;

            //calculate scoring
            foreach (GameObject ball in collisions)
            {
                if (ball.GetComponent<Renderer>().material.color == GetComponent<Renderer>().material.color)
                {
                    gameManager.score++;
                }
                else gameManager.score--;
            }

            //destroy balls
            foreach (GameObject ball in collisions)
            {
                gameManager.RemoveBall(ball);

                if (ball.GetComponent<Renderer>().material.color == gameManager.ballColors[0])
                {
                    gameManager.goalBallsLeft--;
                }

                Destroy(ball);
            }

            //reset collisions list
            collisions.Clear();

            //end game checks
            //no moves left
            if (gameManager.moves <= 0)
            {
                gameManager.EndGame();
            }
            //no matching balls left
            else if (gameManager.goalBallsLeft <= 0)
            {
                gameManager.EndGame();
            }
            //enable handle again
            else if (gameManager.moves > 0)
            {
                dragHandle.GetComponent<S_DragHandle>().EnableHandle();
            }
        }
        else
        {
            //move ball. includes bouncing off walls

            Vector3 newPosition = transform.position + velocity * Time.deltaTime;
            if (newPosition.x < gameManager.getMinX() || newPosition.x > gameManager.getMaxX())
            {
                if (stopsAtWall) velocity = Vector3.zero;
                else velocity.x = -velocity.x;
                newPosition.x = Mathf.Clamp(newPosition.x, gameManager.getMinX(), gameManager.getMaxX());
            }

            if (newPosition.z < gameManager.getMinZ() || newPosition.z > gameManager.getMaxZ())
            {
                if (stopsAtWall) velocity = Vector3.zero;
                else velocity.z = -velocity.z;
                newPosition.z = Mathf.Clamp(newPosition.z, gameManager.getMinZ(), gameManager.getMaxZ());
            }

            transform.position = newPosition;
            velocity *= deceleration;
        }
    }


    public void SetDragVector(Vector2 dragVector)
    {
        if (!isMoving)
        {
            Vector3 dragVector3D = new Vector3(dragVector.x, 0, dragVector.y);
            velocity = -dragVector3D.normalized * Mathf.Clamp(dragVector.magnitude, 0, maxInitialSpeed);
            isMoving = true;
        }
    }
}
