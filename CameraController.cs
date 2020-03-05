﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotationX = 0;
    private float rotationY = 0f;
    private float min = -20;
    private float max = 20;
    public float sensitivity;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        rotationY += Input.GetAxisRaw("Mouse X") * sensitivity;
        rotationX += Input.GetAxisRaw("Mouse Y") * sensitivity;

        rotationY = Mathf.Clamp(rotationY, min, max);
        rotationX = Mathf.Clamp(rotationX, min, max);

        transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);
    }
}
