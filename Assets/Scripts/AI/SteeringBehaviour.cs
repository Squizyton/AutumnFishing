using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour
{

   protected Transform _transform;
   
   public virtual void OnStart(Transform passedTransform)
   {
      _transform = passedTransform;
   }
   
   //Pass in 
   public abstract (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData);
}
