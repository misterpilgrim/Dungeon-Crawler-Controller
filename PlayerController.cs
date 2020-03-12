using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject forwardTarget; // directional targets for vector references
    public GameObject backTarget;
    public GameObject leftTarget;
    public GameObject rightTarget;
    private Vector3 targetVector; // coordinates the player is moving towards every frame
    private Quaternion view; // new angle to set player rotation as for a turn
    private float height; // keeps track of player's y axis in relation to the ground
    private bool moving; // is currently moving?
    private bool turning; // is currently turning?
    private bool waiting; // true/false toggle to prevent multiple InputDelay coroutines firing
    private float movespeed = 17f;
    private float rotatespeed = 350f;
    private float distance = 5f; // distance player travels every step
    private float delay = .15f; // the amount of delay time between steps

    private void Start()
    {
        // properly set target positions away from player based on given distance
        forwardTarget.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
        backTarget.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
        leftTarget.transform.position = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
        rightTarget.transform.position = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);

        // set target position + rotation as player's for spawn
        targetVector = transform.position;
        view = transform.rotation;
    }

    void Update()
    {
        // move towards target position + rotation every frame (determined by input)
        transform.position = Vector3.MoveTowards(transform.position, targetVector, movespeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, view, rotatespeed * Time.deltaTime);

        // managing true/false for moving + turning, floor positioning detection
        FloorPosition();
        StepCheck();
        TurnCheck();

        // detect movement inputs
        if (Input.GetKey(KeyCode.W)) // forward
        {
            Debug.Log("Walked FORWARD");
            StartCoroutine(InputDelay("Step", "Forward"));
        }
        else if (Input.GetKey(KeyCode.A)) // left turn
        {
            Debug.Log("Turned LEFT");
            StartCoroutine(InputDelay("Turn", "Left"));
        }
        else if (Input.GetKey(KeyCode.S)) // backward
        {
            Debug.Log("Walked BACKWARD");
            StartCoroutine(InputDelay("Step", "Backward"));
        }
        else if (Input.GetKey(KeyCode.D)) // right turn
        {
            Debug.Log("Turned RIGHT");
            StartCoroutine(InputDelay("Turn", "Right"));
        }
        else if (Input.GetKey(KeyCode.Q)) // left sidestep
        {
            Debug.Log("Slid LEFT");
            StartCoroutine(InputDelay("Step", "Left"));
        }
        else if (Input.GetKey(KeyCode.E)) // right sidestep
        {
            Debug.Log("Slid RIGHT");
            StartCoroutine(InputDelay("Step", "Right"));
        }
    }

    // adjustable pause effect before taking steps
    IEnumerator InputDelay(string type, string direction)
    {
        switch (type)
        {
            case "Step":
                if (waiting == false && moving == false && turning == false)
                {
                    waiting = true;
                    yield return new WaitForSeconds(delay);

                    Step(direction);
                    waiting = false;
                }
                break;

            case "Turn":
                if (waiting == false && moving == false && turning == false)
                {
                    waiting = true;
                    yield return new WaitForSeconds(delay);

                    Turn(direction);
                    waiting = false;
                }
                break;
        }
    }

    // performs step in direction based off given input (W/S/Q/E keys)
    void Step(string direction)
    {
        if (moving == false && turning == false)
        {
            if (direction == "Forward") // forward movements
            {
                if (WallDetect(transform.forward))
                {
                    return;
                }
                targetVector = forwardTarget.transform.position;
            }

            else if (direction == "Backward") // backward movements
            {
                if (WallDetect(transform.forward * -1))
                {
                    return;
                }
                targetVector = backTarget.transform.position;
            }

            else if (direction == "Left") // left slides
            {
                if (WallDetect(transform.right * -1))
                {
                    return;
                }
                targetVector = leftTarget.transform.position;
            }

            else if (direction == "Right") // right slides
            {
                if (WallDetect(transform.right))
                {
                    return;
                }
                targetVector = rightTarget.transform.position;
            }
        }
    }

    // rotates player in direction based off input (A/D keys)
    void Turn(string direction)
    {
        if (moving == false && turning == false)
        {
            if (direction == "Left") // left turns
            {
                view = transform.rotation * Quaternion.Euler(0, -90f, 0);
            }

            else if (direction == "Right") // right turns
            {
                view = transform.rotation * Quaternion.Euler(0, 90f, 0);
            }
        }
    }

    // returns true/false on whether there is a wall in front of player
    bool WallDetect(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            if (hit.collider.tag == "Wall")
            {
                Debug.Log("Wall hit");
                return true;
            }
        }
        return false;
    }

    // adjusts player's height positioning along floor by raycasting down
    void FloorPosition()
    {
        if (moving)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, distance * 2))
            {
                height = hit.point.y + (transform.localScale.y / 2);
                targetVector.y = height;
            }
        }
    }

    // true/false determination on whether player is currently walking
    void StepCheck()
    {
        if (transform.position == targetVector) moving = false;
        else moving = true;
    }

    // true/false determination on whether player is currently turning
    void TurnCheck()
    {
        if (transform.rotation == view) turning = false;
        else turning = true;
    }
}