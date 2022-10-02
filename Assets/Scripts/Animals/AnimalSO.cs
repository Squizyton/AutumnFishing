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
    [ShowIf("@!willEatAnything")]
    public Vegetation favoriteFood;
    [ShowIf("@!willEatAnything")]
    public Vegetation secondFavoriteFood;
    [ShowIf("@!willEatAnything")]
    public Vegetation dislikedFood;



    [Title("Stats")] 
    public float walkSpeed;
    public float runSpeed;
    
}
