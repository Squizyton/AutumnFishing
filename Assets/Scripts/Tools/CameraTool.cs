using System;
using Cinemachine;
using UI;
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
            if (!isAiming) return;
            
            
        }

        public override void OnLetGo()
        {
       
        }
        
        private void ChangeFOV(float fov)
        {
            var clamp = Mathf.Clamp(cameraCamera.m_Lens.FieldOfView += fov, 30, 90);
            cameraCamera.m_Lens.FieldOfView = clamp;
        }

        public override void OnRightClick()
        {
            UIManager.Instance.CameraUISwitch(true);
            
            isAiming = true;
            
            
        }

        public override void OnRightClickLetGo()
        {
            UIManager.Instance.CameraUISwitch(false);
            cameraCamera.Priority = 0;
            cameraCamera.m_Lens.FieldOfView = 60;
            isAiming = false;
        }
    }
}
