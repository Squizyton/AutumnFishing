using System;
using System.Collections;
using Sirenix.OdinInspector;
using Tools;
using UI;
using UnityEngine;

namespace ItemActions
{
    public class FishingRod : Tool
    {
        [Title("Charge Variables")] private float chargeTime = 0;
        private float maxChargeTime = 2;


        public bool castedOut = false;
        public bool isCharging = false;


        private void Start()
        {
            UIManager.Instance.SetMaxFishingSlider(maxChargeTime);
        }

        public override void OnLeftClick()
        {
            UIManager.Instance.SetFishingSliderActive(true);
        
            if (!castedOut)
            {
                if (!isCharging)
                {
                    StartCoroutine(StartChargingCooldown());
                }
            }
            else
            {
                //Cast the line back in
              CastIn();
            }
        }

        public override void OnLetGo()
        {
         if(isCharging)
             CastOut();
        }

        public override void OnRightClick()
        {
            throw new System.NotImplementedException();
        }


        private IEnumerator StartChargingCooldown()
        {
            yield return new WaitForSeconds(.5f);
            isCharging = true;
            ChargeUp();   
        }

        private void ChargeUp()
        {
            while (isCharging)
            {
                Debug.Log("Charging");
                chargeTime += Time.deltaTime;
                UIManager.Instance.UpdateFishingSlider(chargeTime / maxChargeTime);
            }
        }

        public void CastOut()
        {
            Debug.Log("Cast Out");
            UIManager.Instance.SetFishingSliderActive(false);
            castedOut = true;
            isCharging = false;
        }
    
        private void CastIn()
        {
            castedOut = false;
            Debug.Log("cast back in");
        }
    }
}