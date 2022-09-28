using System;
using System.Collections;
using System.Collections.Generic;
using ForagableMaterial;
using Singleton;
using Sirenix.OdinInspector;
using Tools;
using UnityEngine;

public class PlayerInventory : SingletonBehaviour<PlayerInventory>
{
    [Title("Current item")] public Tool currentTool;

    [Title("Tools")] public List<Tool> tools;

    [Title("Foragable Materials")] public Dictionary<Vegetation, int> foragableMaterials;

    private void Start()
    {
        foragableMaterials = new Dictionary<Vegetation, int>();
    }

    //Add the material to the inventory
    public bool AddToInventory(Vegetation flora)
    {
        //If the material is already in the inventory, add 1 to the amount
        if (foragableMaterials.ContainsKey(flora))
            //If the amount is less than the max amount, add 1 to the amount
            if (foragableMaterials[flora] < flora.maxStack)
                foragableMaterials[flora]++;
            else
            {
                //return false if the amount is equal to the max amount
                return false;
            }
        else
            foragableMaterials.Add(flora, 1);

        return true;
    }
}