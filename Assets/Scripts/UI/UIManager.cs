using Singleton;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [Title("Fishing Variables")] [SerializeField]
        private Slider fishingSlider;
        [SerializeField]private  CanvasGroup fishingCanvasGroup;

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
    }
}