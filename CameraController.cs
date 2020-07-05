using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    private Quaternion defaultView;
    private float rotationX = 0f;
    private float rotationY = 0f;
    public float min;
    public float max;
    public float sensitivity;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        defaultView = transform.localRotation;
    }

    void Update()
    {
        MouseCheck();

        if (Input.GetMouseButton(1) && !player.moving && !player.waiting)
        {
            rotationY += Input.GetAxisRaw("Mouse X") * sensitivity;
            rotationX += Input.GetAxisRaw("Mouse Y") * sensitivity;
            rotationY = Mathf.Clamp(rotationY, min, max);
            rotationX = Mathf.Clamp(rotationX, min, max);

            transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);
        }
        else
        {
            rotationX = 0;
            rotationY = 0;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, defaultView, 100 * Time.deltaTime);
        }
    }

    // prevent player movement if right mouse button is held
    void MouseCheck()
    {
        if (Input.GetMouseButton(1)) player.active = false;
        else player.active = true;
    }
}
