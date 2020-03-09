using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject forwardTarget; // directional targets for vector references
    public GameObject backTarget;
    public GameObject leftTarget;
    public GameObject rightTarget;
    private Vector3 targetVector; // coordinates the controller is constantly moving towards
    private Quaternion view; // new angle to set controller rotation as for a turn
    private bool moving; // is currently moving?
    private bool turning; // is currently turning?
    private float movespeed = 20f; // preference: 20
    private float rotatespeed = 500f; // preference: 500
    private float distance = 5f; // distance controller travels every step
    private float delay = .1f; // the amount of delay time between steps

    private void Start()
    {
        // properly set target positions away from controller based on given distance
        forwardTarget.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
        backTarget.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
        leftTarget.transform.position = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
        rightTarget.transform.position = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);

        // set target position + rotation as controller's for spawn
        targetVector = transform.position;
        view = transform.rotation;
    }

    void Update()
    {
        // move towards target position + rotation every frame (determined by input)
        transform.position = Vector3.MoveTowards(transform.position, targetVector, movespeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, view, rotatespeed * Time.deltaTime);

        // manages true/false for if controller is moving or turning
        StepCheck();
        TurnCheck();

        if (Input.GetKey(KeyCode.W)) // forward
        {
            Debug.Log("Walked FORWARD");
            StartCoroutine(StepDelay("Forward"));
        }
        else if (Input.GetKeyDown(KeyCode.A)) // left turn
        {
            Debug.Log("Turned LEFT");
            Turn("Left");
        }
        else if (Input.GetKey(KeyCode.S)) // backward
        {
            Debug.Log("Walked BACKWARD");
            StartCoroutine(StepDelay("Backward"));
        }
        else if (Input.GetKeyDown(KeyCode.D)) // right turn
        {
            Debug.Log("Turned RIGHT");
            Turn("Right");
        }
        else if (Input.GetKey(KeyCode.Q)) // left sidestep
        {
            Debug.Log("Slid LEFT");
            StartCoroutine(StepDelay("Left"));
        }
        else if (Input.GetKey(KeyCode.E)) // right sidestep
        {
            Debug.Log("Slid RIGHT");
            StartCoroutine(StepDelay("Right"));
        }
    }

    // determines which direction player takes forward/backward step based off current direction (N/S/E/W)
    void Step(string direction)
    {
        // handles forward movements
        if (direction == "Forward" && moving == false && turning == false)
        {
            if (WallCheck(transform.forward))
            {
                return;
            }
            targetVector = forwardTarget.transform.position;
        }

        // handles backward movements
        else if (direction == "Backward" && moving == false && turning == false)
        {
            if (WallCheck(transform.forward * -1))
            {
                return;
            }
            targetVector = backTarget.transform.position;
        }

        // handles left slides
        else if (direction == "Left" && moving == false && turning == false)
        {
            if (WallCheck(transform.right * -1))
            {
                return;
            }
            targetVector = leftTarget.transform.position;
        }

        // handles right slides
        else if (direction == "Right" && moving == false && turning == false)
        {
            if (WallCheck(transform.right))
            {
                return;
            }
            targetVector = rightTarget.transform.position;
        }
    }

    // determines which direction player faces on left/right turn based off current direction (N/S/E/W)
    void Turn(string direction)
    {
        // handles left turns
        if (direction == "Left" && turning == false && moving == false)
        {
            view = transform.rotation * Quaternion.Euler(0, -90f, 0);
        }

        // handles right turns
        else if (direction == "Right" && turning == false && moving == false)
        {
            view = transform.rotation * Quaternion.Euler(0, 90f, 0);
        }
    }

    // determines whether or not there is a wall in front of player, and how to react
    bool WallCheck(Vector3 direction)
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

    // true/false determination on whether player is currently walking
    void StepCheck()
    {
        if (transform.position == targetVector)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }
    }

    // true/false determination on whether player is currently turning
    void TurnCheck()
    {
        if (transform.rotation == view)
        {
            turning = false;
        }
        else
        {
            turning = true;
        }
    }
    
    // adjustable pause effect before taking steps
    IEnumerator StepDelay(string direction)
    {
        if (moving == false && turning == false)
        {
            yield return new WaitForSeconds(delay);
            Step(direction);
        }
    }
}