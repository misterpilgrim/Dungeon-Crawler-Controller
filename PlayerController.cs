using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject forwardTarget; // directional targets for vector references
    public GameObject backTarget;
    public GameObject leftTarget;
    public GameObject rightTarget;
    public AudioSource stepSound;
    [SerializeField] private Vector3 targetVector; // coordinates the player is moving towards every frame
    [SerializeField] private Quaternion view; // new angle to set player rotation as for a turn
    [SerializeField] private float height; // keeps track of player's y axis in relation to the ground
    public bool active { get; set; } // whether or not player accepts inputs
    public bool moving { get; private set; } // is currently moving?
    public bool waiting { get; private set; } // true/false toggle to prevent multiple coroutines firing
    public float movespeed;
    public float rotatespeed;
    public float distance;
    public float delay; // the amount of delay time between steps/turns

    private void Start()
    {
        active = true;

        // properly set target positions away from player based on given distance
        forwardTarget.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
        backTarget.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
        leftTarget.transform.position = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
        rightTarget.transform.position = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);

        // set target position and rotation as player's for spawn
        targetVector = transform.position;
        view = transform.rotation;
    }

    void Update()
    {
        UpdatePosition();
        MoveCheck();
        
        // detect movement inputs
        if (active && !waiting && !moving)
        {
            // forward
            if (Input.GetKey(KeyCode.W)) StartCoroutine(InputDelay("Step", "Forward"));
            // left turn
            else if (Input.GetKey(KeyCode.A)) StartCoroutine(InputDelay("Turn", "Left"));
            // backward
            else if (Input.GetKey(KeyCode.S)) StartCoroutine(InputDelay("Step", "Backward"));
            // right turn
            else if (Input.GetKey(KeyCode.D)) StartCoroutine(InputDelay("Turn", "Right"));
            // left sidestep
            else if (Input.GetKey(KeyCode.Q)) StartCoroutine(InputDelay("Step", "Left"));
            // right sidestep
            else if (Input.GetKey(KeyCode.E)) StartCoroutine(InputDelay("Step", "Right"));
        }
    }

    // pause effect before taking steps
    IEnumerator InputDelay(string type, string direction)
    {
        // wait before stepping/turning
        waiting = true;
        yield return new WaitForSeconds(delay);

        // commence step/turn after wait
        switch (type)
        {
            case "Step":
                Step(direction);
                stepSound.Play();
                break;

            case "Turn":
                Turn(direction);
                break;
        }
        // no longer waiting, ready for next input
        waiting = false;
    }

    // performs step in direction based off given input (W/S/Q/E keys)
    void Step(string direction)
    {
        if (moving) return;

        switch (direction)
        {
            case "Forward": // forward movements
                if (WallDetect(transform.forward)) return;
                targetVector = forwardTarget.transform.position;
                break;

            case "Backward": // backward movements
                if (WallDetect(transform.forward * -1)) return;
                targetVector = backTarget.transform.position;
                break;

            case "Left": // left slides
                if (WallDetect(transform.right * -1)) return;
                targetVector = leftTarget.transform.position;
                break;

            case "Right": // right slides
                if (WallDetect(transform.right)) return;
                targetVector = rightTarget.transform.position;
                break;
        }
    }

    // rotates player in direction based off input (A/D keys)
    void Turn(string direction)
    {
        if (moving) return;

        if (direction == "Left") view = transform.rotation * Quaternion.Euler(0, -90f, 0);
        else if (direction == "Right") view = transform.rotation * Quaternion.Euler(0, 90f, 0);
    }

    // returns true/false on whether there is a wall in front of player
    bool WallDetect(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            if (hit.collider.tag == "Non-Wall") return false;
            else return true;
        }
        else return false;
    }

    // adjusts player's positioning and rotation
    void UpdatePosition()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance * 2))
        {
            height = hit.point.y + (transform.localScale.y / 2);
            targetVector.y = height;
        }

        // move towards target position + rotation every frame (determined by input)
        transform.position = Vector3.MoveTowards(transform.position, targetVector, movespeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, view, rotatespeed * Time.deltaTime);
    }

    // true/false determination on whether player is currently stepping/turning
    void MoveCheck()
    {
        if (transform.position != targetVector || transform.rotation != view) moving = true;
        else moving = false;
    }
}