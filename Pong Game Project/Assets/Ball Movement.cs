using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10; // Initial speed of ball
    [SerializeField] private float speedIncrease = 0.25f; // Amount by which speed increases after each hit
    [SerializeField] private Text playerScore; // UI elemenent displaying player's score
    [SerializeField] private Text AIScore; // UI element displaying AI's score

    private int hitCounter; // Counter to keep track of how many times the ball has been hit
    private Rigidbody2D rb; // Reference to the RigidBody of the ball

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        Invoke("StartBall", 2f); // Start ball movement after 2 seconds of delay
    }

    private void FixedUpdate()
    {
        // Just making sure the ball does not exceed the max speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, initialSpeed + (speedIncrease * hitCounter));
    }

    private void StartBall()
    {
        rb.velocity = new Vector2(-1, 0) * (initialSpeed + speedIncrease * hitCounter);
    }

    private void ResetBall()
    {
        // Reset the position, velocity, and hit counter of the ball
        rb.velocity = new Vector2(0, 0);
        transform.position = new Vector2(0, 0);
        hitCounter = 0;
        Invoke("StartBall", 2f);
    }

    private void PlayerBounce(Transform myObject)
    {
        hitCounter++;

        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position;

        float xDirection, yDirection;
        if (transform.position.x > 0)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = 1;
        }
        yDirection = (ballPos.y - playerPos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
        if (yDirection == 0)
        {
            yDirection = 0.25f;
        }
        rb.velocity = new Vector2(xDirection, yDirection) * (initialSpeed + (speedIncrease * hitCounter));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" || collision.gameObject.name == "AI")
        {
            PlayerBounce(collision.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the ball passes the goal line
        if (transform.position.x > 0) // Ball passes player's goal
        {
            // Reset the ball and change player score
            ResetBall();
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
        }
        else if(transform.position.x < 0)
        {
            // Reset ball and change AI score
            ResetBall();
            AIScore.text = (int.Parse(AIScore.text) + 1).ToString();
        }
    }

}
