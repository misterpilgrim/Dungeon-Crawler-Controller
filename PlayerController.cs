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

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, movespeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, view, rotatespeed * Time.deltaTime);

        TurnCheck();
        StepCheck();

        if (Input.GetKey(KeyCode.W)) // forward
        {
            Debug.Log("Walked FORWARD");
            Step("Forward");
        }
        else if (Input.GetKeyDown(KeyCode.A)) // left turn
        {
            Debug.Log("Turned LEFT");
            Turn("Left");
        }
        else if (Input.GetKey(KeyCode.S)) // backward
        {
            Debug.Log("Walked BACKWARD");
            Step("Backward");
        }
        else if (Input.GetKeyDown(KeyCode.D)) // right turn
        {
            Debug.Log("Turned RIGHT");
            Turn("Right");
        }
    }

    // determines which direction player takes forward/backward step based off current direction (N/S/E/W)
    void Step(string direction)
    {
        // handles forward movements
        if (direction == "Forward" && moving == false && turning == false)
        {
            switch (compass)
            {
                case Compass.North:
                    if (WallCheck(Vector3.forward))
                    {
                        break;
                    }
                    target = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
                    break;

                case Compass.South:
                    if (WallCheck(Vector3.back))
                    {
                        break;
                    }
                    target = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
                    break;

                case Compass.East:
                    if (WallCheck(Vector3.right))
                    {
                        break;
                    }
                    target = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
                    break;

                case Compass.West:
                    if (WallCheck(Vector3.left))
                    {
                        break;
                    }
                    target = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
                    break;
            }
        }

        // handles backward movements
        else if (direction == "Backward" && moving == false && turning == false)
        {
            switch (compass)
            {
                case Compass.North:
                    if (WallCheck(Vector3.back))
                    {
                        break;
                    }
                    target = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
                    break;

                case Compass.South:
                    if (WallCheck(Vector3.forward))
                    {
                        break;
                    }
                    target = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
                    break;

                case Compass.East:
                    if (WallCheck(Vector3.left))
                    {
                        break;
                    }
                    target = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
                    break;

                case Compass.West:
                    if (WallCheck(Vector3.right))
                    {
                        break;
                    }
                    target = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
                    break;
            }
        }
    }

    // determines which direction player faces on left/right turn based off current direction (N/S/E/W)
    void Turn(string direction)
    {
        // handles left turns
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

        // handles right turns
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