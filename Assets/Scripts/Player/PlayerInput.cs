using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UI;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private CameraController cameraController;
    [SerializeField] private Sprite pickupSprite;
    [SerializeField] private LayerMask interactableLayer;
    public void Update()
    {

        Raycasting();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            PlayerInventory.Instance.currentTool.OnLeftClick();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            PlayerInventory.Instance.currentTool.OnRightClick();
        if (Input.GetKeyUp(KeyCode.Mouse0))
            PlayerInventory.Instance.currentTool.OnLetGo();
    }


    public void Raycasting()
    {
        RaycastHit hit;
        
        
        
        //shoots a raycast from the center of the screen
        if (Physics.Raycast(
                cameraController.unityCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0)),
                out hit, 5, interactableLayer))
        {
            if (!hit.collider.TryGetComponent(out PickupableObject pickup)) return;

            Debug.Log(hit.collider.name);
            
            if (pickup)
                UIManager.Instance.SetCrosshair(pickupSprite, .3f);

            
            if(Input.GetKeyDown(KeyCode.F))
                pickup.OnPickup();
            
        }else
        {
            UIManager.Instance.SetCrosshair(null,.1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cameraController.unityCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0)));
    }
}