using System.Collections;
using System.Collections.Generic;
using ForagableMaterial;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animal", menuName = "New Animal Info")]
public class AnimalSO : ScriptableObject
{
    public string animalName;
    
    
    [Title("Food")]
    [Space(4)]
    public bool willEatAnything;
    [ShowIf("@willEatAnything")] public float stopWeight;
    
    [ShowIf("@!willEatAnything")] public List<EatFood> foodItWillEat;
    



    [Title("Stats")] 
    public float walkSpeed;
    public float runSpeed;
    public float sightRadius;
    
    
    [System.Serializable]
    public class EatFood
    {
        public Vegetation food;
        public float weight;
    }
}
