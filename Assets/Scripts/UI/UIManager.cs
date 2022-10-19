using System.Drawing;
using Singleton;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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
        
    }
}