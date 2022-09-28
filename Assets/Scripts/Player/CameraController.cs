using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Title("Camera Settings")] public Camera unityCamera;
    public CinemachineVirtualCamera playerCamera;
    [SerializeField]private float mouseSensitivity = 100f;
    
    
    [Title("Other settings")]
    [SerializeField] private Transform playerBody;

    private float xRotation;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void Update()
    {
        ChangeCameraRotation();
    }
    
    private void ChangeCameraRotation()
    {
  
        //Get the Mouse Input * Mouse Sensitivity * Time.deltaTime (to make it not rely on frame rate)
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Inverse the Y axis so that it moves in the correct direction: up is up and down is down
        xRotation -= mouseY;
        //Clamp the rotation so that the player can't look too far up or down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        
        //Rotate the camera on the X axis
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Rotate the player on the x axis
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
