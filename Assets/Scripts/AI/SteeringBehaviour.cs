using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
   //Pass in 
   public abstract (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData);
}
