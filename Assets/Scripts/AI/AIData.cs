using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AIData : MonoBehaviour
{

   public LayerMask obstaclesLayerMask;
   
   //Targets detected by AI, in case we want multiple targets
   public List<Transform> targets = null;
   //Store all obstacles around enemy
   public Collider[] obstacles;

   //AI's current target
   public Transform currentTarget;
   
   //Return TargetsCount
   public int GetTargetsCount() => targets?.Count ?? 0;

   [Title("Behaviour/Detector variables")]
   public float sightRadius;
   public float colliderRadius;
   public float targetReachThreshold;
   
   
   
   public void FeedTargets(List<Transform> newTargets)
   {
      this.targets = newTargets;
   }
}
