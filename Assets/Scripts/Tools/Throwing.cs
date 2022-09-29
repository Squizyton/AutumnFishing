using System;
using System.Collections.Generic;
using System.Linq;
using ForagableMaterial;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Player;
using Unity.VisualScripting;

namespace Tools
{
    public class Throwing : Tool
    {
        [Title("Litter Fall")] [SerializeField]
        private PlayerInventory.StoredItem currentItem;
        private Transform spawnPoint;
        private GameObject currentLitterObject;
        [SerializeField]private Rigidbody litterRb;
        
        
        [Title("Current Variables")]
        [SerializeField] private float currentThrowForce;
        
        [SerializeField] private int currentIndex;
        
        [Title("Max Variables")]
        [SerializeField] private float maxThrowForce;

        //private variables
        [SerializeField] private bool isCharging;
        

        private void Update()
        {
            if(Input.mouseScrollDelta.y != 0)
                SwitchObject(Input.mouseScrollDelta.y);
            
            
            
        }
        
        


        private void SwitchObject(float value)
        {
            currentIndex = (currentIndex + (int)value) % PlayerInventory.Instance.foragableMaterials.Count;
            currentItem = PlayerInventory.Instance.foragableMaterials.Values.ToList().ElementAt(currentIndex);
            
            if(currentLitterObject)
             Destroy(currentLitterObject);
            
            currentLitterObject = Instantiate(currentItem.vegetation.prefab, spawnPoint.position, Quaternion.identity);
        }


        public override void OnEquip()
        {
            throw new NotImplementedException();
        }

        public override void OnUnequip()
        {
            throw new NotImplementedException();
        }

        public override void OnLeftClick()
        {
            throw new System.NotImplementedException();
        }

        public override void OnLetGo()
        {
            if (isCharging)
                ThrowThing();

        }

        public override void OnRightClick()
        {
            throw new System.NotImplementedException();
        }

        void ThrowThing()
        {
            
        }

    }
}
