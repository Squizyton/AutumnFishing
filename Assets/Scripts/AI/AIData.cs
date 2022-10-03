using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
   
   //Targets detected by AI, in case we want multiple targets
   public List<Transform> targets = null;
   //Store all obstacles around enemy
   public Collider[] obstacles;

   //AI's current target
   public Transform currentTarget;
   
   //Return TargetsCount
   public int GetTargetsCount() => targets?.Count ?? 0;
   
   
}
