using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ForagableMaterial;
using Player;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

using Unity.VisualScripting;

namespace Tools
{
    public class Throwing : Tool
    {
        [Title("Litter Fall")] [SerializeField]
        private PlayerInventory.StoredItem currentItem;
        [SerializeField]private Transform spawnPoint;
        private GameObject currentLitterObject;
        [SerializeField]private Rigidbody litterRb;
        
        
        [Title("Current Variables")]
        [SerializeField] private float currentThrowForce;
        
        [SerializeField] private int currentIndex;
        
        [Title("Max Variables")]
        [SerializeField] private float maxThrowForce;

        //private variables
        [SerializeField] private bool isCharging;

        [Title("Throwing")]
        [SerializeField] private float throwAngle;
        
        
        private Coroutine lastCoroutine;
        
        private void Update()
        {
           
            //We can't switch if charging
            if(!isCharging)
                if(Input.mouseScrollDelta.y != 0)
                  SwitchObject(Input.mouseScrollDelta.y);


            if (!isCharging) return;
            
            if (Input.GetMouseButton(0))
                ChargeUp();
        }

        private void SwitchObject(float value)
        {
            Debug.Log("Switching");
            
            if (PlayerInventory.Instance.foragableMaterials.Count <= 0) return;
            
            
            
            currentIndex = (currentIndex + ((int)value * -1)) % PlayerInventory.Instance.foragableMaterials.Count;
           
            
            currentItem = PlayerInventory.Instance.foragableMaterials.Values.ToList().ElementAt(currentIndex);
            if(currentLitterObject)
             Destroy(currentLitterObject);
            
            currentLitterObject = Instantiate(currentItem.vegetation.prefab, spawnPoint.position,currentItem.vegetation.prefab.transform.rotation, spawnPoint);

            //Try Get Component is less expensive than GetComponent
            currentLitterObject.TryGetComponent(out Rigidbody rb);
            litterRb = rb;
    
            litterRb.isKinematic = true;
            
        }


        public override void OnEquip()
        {
            UIManager.Instance.SetMaxFishingSlider(maxThrowForce);
        }

        public override void OnUnequip()
        {
        
        }

        public override void OnLeftClick()
        {
            if (!isCharging)
            {
                lastCoroutine = StartCoroutine(StartChargingCooldown());
            }
        }
        
        //TODO: Might make sense to uh, move this into the Tool class
        private IEnumerator StartChargingCooldown()
        {
            currentThrowForce = 0;
            UIManager.Instance.UpdateFishingSlider(currentThrowForce);
          
            yield return new WaitForSeconds(.4f);
            isCharging = true;
        }

        
        private void ChargeUp()
        {
            Debug.Log("Charging");
            currentThrowForce += Time.deltaTime * .55f;

            if (currentThrowForce > maxThrowForce)
            {
                Debug.Log("Max Charge");
                currentThrowForce = maxThrowForce;
            }

            if(currentThrowForce > 0)
                UIManager.Instance.SetFishingSliderActive(true);
            
            UIManager.Instance.UpdateFishingSlider(currentThrowForce);
        }
        
        public override void OnLetGo()
        {
            if (isCharging)
                ThrowThing();

        }
        
        void ThrowThing()
        {
            currentLitterObject.transform.parent = null;
            litterRb.isKinematic = false;
            
            var rotation = Quaternion.Euler(0,0,throwAngle);
            
            litterRb.AddForce(Vector3.up * (throwAngle * currentThrowForce), ForceMode.Impulse);
        }
        
        public override void OnRightClick()
        {
            throw new System.NotImplementedException();
        }

       

    }
}
