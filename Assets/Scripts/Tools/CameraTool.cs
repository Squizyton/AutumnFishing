using System;
using Cinemachine;
using UnityEngine;

namespace Tools
{
    public class CameraTool : Tool
    {
        [SerializeField]private CinemachineVirtualCamera cameraCamera;
        [SerializeField]private bool isAiming;
        
        
        public override void OnEquip()
        {
            cameraCamera.Priority = 65;
        }

        public override void OnUnequip()
        {
            cameraCamera.Priority = 0;
            cameraCamera.m_Lens.FieldOfView = 60;
        }


        private void Update()
        {
            if(!isAiming) return;
            
            
            if(Input.mouseScrollDelta.y != 0)
                ChangeFOV(Input.mouseScrollDelta.y);
        }

        public override void OnLeftClick()
        {
       
        }

        public override void OnLetGo()
        {
       
        }
        
        private void ChangeFOV(float fov)
        {
            cameraCamera.m_Lens.FieldOfView += Mathf.Clamp(cameraCamera.m_Lens.FieldOfView, 20,90);
        }

        public override void OnRightClick()
        {
            isAiming = true;
        }

        public override void OnRightClickLetGo()
        {
            isAiming = false;
        }
    }
}
