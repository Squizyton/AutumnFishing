using System;
using System.Collections;
using Sirenix.OdinInspector;
using Tools;
using UI;
using Unity.VisualScripting;
using UnityEngine;

namespace ItemActions
{
    public class FishingRod : Tool
    {
        [Title("Charge Variables")] private float chargeTime = 0;
        private float _maxChargeTime = 2;
        public bool castedOut = false;
        public bool isCharging = false;


        [Title("Fishing Variables")] [SerializeField]
        private Animator fishingRodAnimator;

        [SerializeField] private GameObject bauble;

        [SerializeField] private Transform baubleSpawnPoint;

        [SerializeField] private Rigidbody baubleRb;
        [SerializeField] private float baseForce = 10;


        [Title("Line Renderer")] [SerializeField]
        private LineRenderer lineRenderer;

        
        
        private Coroutine lastCoroutine;
        
        private void Start()
        {
            UIManager.Instance.SetMaxFishingSlider(_maxChargeTime);
        }

        public override void OnLeftClick()
        {
            if (!castedOut)
            {
                if (!isCharging)
                {
                    lastCoroutine = StartCoroutine(StartChargingCooldown());
                }
            }
            else
            {
                //Cast the line back in
                //TODO: Add a cast back in animation/Make it better
                fishingRodAnimator.SetTrigger("castin");
            }
        }


        public void Update()
        {
            if (isCharging)
            {
                if (Input.GetMouseButton(0))
                    ChargeUp();
            }

            if (castedOut)
            {
                //lineRenderer.SetPosition(1, baubleSpawnPoint.position);
            }
        }


        public override void OnLetGo()
        {
            if (isCharging)
                fishingRodAnimator.SetTrigger("castout");
            else if(!castedOut)
            {
                StopCoroutine(lastCoroutine);
                chargeTime = 0;
                UIManager.Instance.SetFishingSliderActive(false);
            }
        }

        public override void OnRightClick()
        {
        }


        private IEnumerator StartChargingCooldown()
        {
            chargeTime = 0;
            UIManager.Instance.UpdateFishingSlider(chargeTime);
          
            yield return new WaitForSeconds(.4f);
            isCharging = true;
        }

        private void ChargeUp()
        {
            Debug.Log("Charging");
            chargeTime += Time.deltaTime * .55f;

            if (chargeTime > _maxChargeTime)
            {
                Debug.Log("Max Charge");
                chargeTime = _maxChargeTime;
            }

            if(chargeTime > 0)
                UIManager.Instance.SetFishingSliderActive(true);
            
            UIManager.Instance.UpdateFishingSlider(chargeTime);
        }

        private void CastOut()
        {
            //TODO: Maybe add a trajectory line
            Debug.Log("Cast Out");
            UIManager.Instance.SetFishingSliderActive(false);
            castedOut = true;
            isCharging = false;
            bauble.transform.SetParent(null);
            baubleRb.isKinematic = false;
            //TODO: Switch camera.main to a cached camera
            baubleRb.AddForce(Camera.main.transform.forward * (baseForce * chargeTime), ForceMode.Impulse);
        }

        private void CastIn()
        {
            bauble.transform.SetParent(baubleSpawnPoint);
            castedOut = false;
            //TODO: Add lerp the bauble back to the player
            baubleRb.isKinematic = true;
            baubleRb.velocity = Vector3.zero;
            
            bauble.transform.position =bauble.transform.parent.position;
        }
    }
}