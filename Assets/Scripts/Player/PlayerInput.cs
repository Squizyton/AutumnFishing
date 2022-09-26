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
       {
           PlayerInventory.Instance.currentItem.OnLeftClick();
       }
       if(Input.GetKeyUp(KeyCode.Mouse1))
           PlayerInventory.Instance.currentItem.OnRightClick();
       if(Input.GetKeyUp(KeyCode.Mouse0))
           PlayerInventory.Instance.currentItem.OnLetGo();
   }
}
