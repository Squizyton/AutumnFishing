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
    
    
    [Title("AI")]
    //Radius is used to check for obstacles
    public float sightRadius;
    //Avoid at all cost if distance is less than or equal to this value
    public float colliderRadius;
    //In case Agent has lost sight of target, we need to know where it was last seen. This will tell us if we are close enough
    private float targetReachedThreshold = 0.5f;
    
    
    [System.Serializable]
    public class EatFood
    {
        public Vegetation food;
        public float weight;
    }
}
