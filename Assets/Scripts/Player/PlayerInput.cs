using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            PlayerInventory.Instance.currentTool.OnLeftClick();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            PlayerInventory.Instance.currentTool.OnRightClick();
        if (Input.GetKeyUp(KeyCode.Mouse0))
            PlayerInventory.Instance.currentTool.OnLetGo();
    }
}