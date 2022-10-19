using System;
using System.Collections;
using System.Collections.Generic;
using Player;
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
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
            PlayerInventory.Instance.SwitchTool(0);
        if(Input.GetKeyDown(KeyCode.Alpha2))
            PlayerInventory.Instance.SwitchTool(1);
        if(Input.GetKeyDown(KeyCode.Alpha3))
           PlayerInventory.Instance.SwitchTool(2);


        if (!PlayerInventory.Instance.currentTool) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            PlayerInventory.Instance.currentTool.OnLeftClick();
        if (Input.GetKeyUp(KeyCode.Mouse0))
            PlayerInventory.Instance.currentTool.OnLetGo();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            PlayerInventory.Instance.currentTool.OnRightClick();
        if(Input.GetKeyUp(KeyCode.Mouse1))
            PlayerInventory.Instance.currentTool.OnRightClickLetGo();

    }


    private void Raycasting()
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