using System;
using Cinemachine;
using Player;
using UI;
using UnityEngine;

namespace Tools
{
    public class CameraTool : Tool
    {
        [SerializeField]private CinemachineVirtualCamera cameraCamera;
        [SerializeField]private bool isAiming;

        [SerializeField] public float previousFOV;

        private void Start()
        {
            previousFOV = cameraCamera.m_Lens.FieldOfView;
        }

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

            var image = CreateRenderTexture.RTImage();
            PlayerMovement.Instance.SetState(1);

            UIManager.Instance.SetPicture(image);
        }

        public override void OnLetGo()
        {
            OnRightClickLetGo();
        }
        
        private void ChangeFOV(float fov)
        {
            var clamp = Mathf.Clamp(cameraCamera.m_Lens.FieldOfView += fov, 30, 90);
            cameraCamera.m_Lens.FieldOfView = clamp;
        }

        public override void OnRightClick()
        {
            UIManager.Instance.CameraUISwitch(true);
            cameraCamera.m_Lens.FieldOfView = previousFOV;
            isAiming = true;
        }

        public override void OnRightClickLetGo()
        {
            UIManager.Instance.CameraUISwitch(false);
            previousFOV = cameraCamera.m_Lens.FieldOfView;
            cameraCamera.m_Lens.FieldOfView = 60;
            isAiming = false;
        }
    }
}
