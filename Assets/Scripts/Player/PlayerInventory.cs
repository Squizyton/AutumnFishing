using System;
using System.Collections.Generic;
using System.Diagnostics;
using ForagableMaterial;
using Singleton;
using Sirenix.OdinInspector;
using Tools;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Player
{
    public class PlayerInventory : SingletonBehaviour<PlayerInventory>
    {
        [Title("Current item")] public Tool currentTool;

        [Title("Tools")] public List<Tool> tools;

        [Title("Foragable Materials")] public Dictionary<Vegetation, StoredItem> foragableMaterials;

        private void Start()
        {
            foragableMaterials = new Dictionary<Vegetation, StoredItem>();
        }

        //Add the material to the inventory
        public bool AddToInventory(Vegetation flora)
        {
            //If the material is already in the inventory, add 1 to the amount
            if (foragableMaterials.ContainsKey(flora))
                //If the amount is less than the max amount, add 1 to the amount
                if (foragableMaterials[flora].amount < flora.maxStack)
                    foragableMaterials[flora].amount++;
                else
                {
                    //return false if the amount is equal to the max amount
                    return false;
                }
            else
                foragableMaterials.Add(flora, new StoredItem(flora));

            return true;
        }


        public void SwitchTool(int number)
        {
            if (currentTool)
            {
                currentTool.OnUnequip();
                currentTool.gameObject.SetActive(false);
            }

            currentTool = tools[number];

            currentTool.gameObject.SetActive(true);
            currentTool.OnEquip();
        }

        public class StoredItem
        {
            public int amount;
            public Vegetation vegetation;
            public bool playerThrown;
            public StoredItem(Vegetation vegetation)
            {
                this.vegetation = vegetation;
                amount = 1;
            }


            public void IsThrown(bool value)
            {
                playerThrown = value;
            }
        }
    }
}