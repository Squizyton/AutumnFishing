using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Player;
using Singleton;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace UI
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [Title("Crosshair")] [SerializeField] private Image crosshair;

        //TODO: Be able to change crosshair size
        //[SerializeField] private float crosshairSize = 10f;
        [SerializeField] private Sprite originalCrosshair;
        [SerializeField] private Sprite pickupCrosshair;
    
        
        
        
        
        [Title("Fishing Variables")] [SerializeField]
        private Slider fishingSlider;

        [SerializeField] private CanvasGroup fishingCanvasGroup;

        [Title("Camera Variables")]
        [SerializeField]private CanvasGroup cameraCanvasGroup;
        
        
        [Title("Photo View Variables")]
        [SerializeField]private CanvasGroup photoViewCanvasGroup;
        [SerializeField] private Image photoViewImage;
        private int screenWidth,screenHeight;

        private void Start()
        {
            screenHeight = 1080;
            screenWidth = 1920;

#if !UNITY_EDITOR
            screenWidth = Screen.width;
            screenHeight = Screen.height;
#endif
        }

        public void UpdateFishingSlider(float value)
        {
            fishingSlider.value = value;
        }

        public void SetMaxFishingSlider(float value)
        {
            fishingSlider.maxValue = value;
        }

        public void SetFishingSliderActive(bool value)
        {
            fishingCanvasGroup.alpha = value ? .45f : 0;
        }



        //TODO: Will probably need to change this to accomodate more than one crosshair
        public void SetCrosshair(Sprite sprite,float size)
        {
            if (!sprite && crosshair.sprite != originalCrosshair)
            {
                crosshair.gameObject.transform.localScale = new Vector3(size, size, 1);
                crosshair.sprite = originalCrosshair;
            }

            if (!sprite || crosshair.sprite == sprite) return;
            crosshair.gameObject.transform.localScale = new Vector3(size, size, 1);
            crosshair.sprite = sprite;
        }
        
        
        public void CameraUISwitch(bool value)
        {
          cameraCanvasGroup.alpha = value ? 1 : 0;
        }

        public void SetPicture(Texture2D image)
        {
            photoViewCanvasGroup.alpha = 1;
            
            var rectTransform = (RectTransform) photoViewImage.transform;

            var croppedImage = new Texture2D(450,380);
            
            //loop through the pixels of the image near the center
            for (var x = 0 ; x < croppedImage.width; x++)
            {
                var offsetX = x + 70;
                for (var y = 0; y < croppedImage.height; y++)
                {
                    var offsetY = y + 380;
                    //get the pixel from the image
                    var pixel = image.GetPixel(offsetX, offsetY);
                    //set the pixel in the cropped image
                    croppedImage.SetPixel(x, y, pixel);
                }
            }

            
            croppedImage.Apply();
            
            
            //DEBUG (FOR NOW)---------------------------
            var croppedBytes = croppedImage.EncodeToPNG();
            var normalBytes = image.EncodeToPNG();
            var dirPath = Application.dataPath + "/SaveImages/";
            Debug.Log(dirPath);
            if(!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + "CroppedImage" + ".png", croppedBytes);
            File.WriteAllBytes(dirPath + "NormalImage" + ".png", normalBytes);
            //-------------------------------
            
            
            var sprite = Sprite.Create(croppedImage, new Rect(0, 0,450,380),Vector3.zero);
            
            photoViewImage.sprite = sprite;
            
            PlayerInput.Instance.SetOverridenRightClickAction(() =>
            {
                photoViewImage.sprite = null;
                photoViewCanvasGroup.alpha = 0;
                PlayerMovement.Instance.SetState(0);
                Cursor.lockState = CursorLockMode.Locked;
            });
            
            Cursor.lockState = CursorLockMode.None;
        }

    }
}