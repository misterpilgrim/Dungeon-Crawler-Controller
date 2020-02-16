using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum Compass { North, South, East, West };
    private Compass compass;
    private Vector3 target;
    private Quaternion view;
    private bool moving;
    private bool turning;
    public float movespeed;
    public float rotatespeed;
    public float distance;

    private void Start()
    {
        target = transform.position;
        view = transform.rotation;
        compass = Compass.North;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, movespeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, view, rotatespeed * Time.deltaTime);

        TurnCheck();
        StepCheck();

        if (Input.GetKey(KeyCode.W)) // Forward
        {
            Debug.Log("Walked FORWARD");
            Step("Forward");
        }
        else if (Input.GetKeyDown(KeyCode.A)) // Left Turn
        {
            Debug.Log("Turned LEFT");
            Turn("Left");
        }
        else if (Input.GetKey(KeyCode.S)) // Backward
        {
            Debug.Log("Walked BACKWARD");
            Step("Backward");
        }
        else if (Input.GetKeyDown(KeyCode.D)) // Right Turn
        {
            Debug.Log("Turned RIGHT");
            Turn("Right");
        }
    }

    // determines which direction player takes forward/backward step based off current direction (N/S/E/W)
    void Step(string direction)
    {
        if (direction == "Forward" && moving == false && turning == false)
        {
            switch (compass)
            {
                case Compass.North:
                    target = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
                    break;

                case Compass.South:
                    target = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
                    break;

                case Compass.East:
                    target = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
                    break;

                case Compass.West:
                    target = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
                    break;
            }
        }

        else if (direction == "Backward" && moving == false && turning == false)
        {
            switch (compass)
            {
                case Compass.North:
                    target = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
                    break;

                case Compass.South:
                    target = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
                    break;

                case Compass.East:
                    target = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
                    break;

                case Compass.West:
                    target = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
                    break;
            }
        }
    }

    // determines which direction player faces on left/right turn based off current direction (N/S/E/W)
    void Turn(string direction)
    {
        // Left Turn
        if (direction == "Left" && turning == false && moving == false)
        {
            switch (compass)
            {
                case Compass.North:
                    Debug.Log("Facing WEST");
                    compass = Compass.West;
                    view = transform.rotation * Quaternion.Euler(0, -90f, 0);
                    break;

                case Compass.South:
                    Debug.Log("Facing EAST");
                    compass = Compass.East;
                    view = transform.rotation * Quaternion.Euler(0, -90f, 0);
                    break;

                case Compass.East:
                    Debug.Log("Facing NORTH");
                    compass = Compass.North;
                    view = transform.rotation * Quaternion.Euler(0, -90f, 0);
                    break;

                case Compass.West:
                    Debug.Log("Facing SOUTH");
                    compass = Compass.South;
                    view = transform.rotation * Quaternion.Euler(0, -90f, 0);
                    break;
            }
        }

        // Right Turn
        else if (direction == "Right" && turning == false && moving == false)
        {
            switch (compass)
            {
                case Compass.North:
                    Debug.Log("Facing EAST");
                    compass = Compass.East;
                    view = transform.rotation * Quaternion.Euler(0, 90f, 0);
                    break;

                case Compass.South:
                    Debug.Log("Facing WEST");
                    compass = Compass.West;
                    view = transform.rotation * Quaternion.Euler(0, 90f, 0);
                    break;

                case Compass.East:
                    Debug.Log("Facing SOUTH");
                    compass = Compass.South;
                    view = transform.rotation * Quaternion.Euler(0, 90f, 0);
                    break;

                case Compass.West:
                    Debug.Log("Facing NORTH");
                    compass = Compass.North;
                    view = transform.rotation * Quaternion.Euler(0, 90f, 0);
                    break;
            }
        }
    }

    // true/false determination on whether player is currently walking
    void StepCheck()
    {
        if (transform.position == target)
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

}