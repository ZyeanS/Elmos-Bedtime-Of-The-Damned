using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Variables
    public float mouseSensitivity;
    public Transform orientation;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
        //orientation.Rotate(Vector3.up * mouseX);
    }
}
