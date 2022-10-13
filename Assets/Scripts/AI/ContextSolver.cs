using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Sirenix.OdinInspector;
using UnityEngine;

public class ContextSolver 
{

   [Title("Debug")] [SerializeField] private bool showGizmos = true;
   private float[] _interestGizmo = Array.Empty<float>();
   private Vector3 _resultDirection = Vector3.zero;
   //private float _rayLength = 1f;

   private void Start()
   {
      _interestGizmo = new float[8];
   }


   public Vector3 GetDirectionToMove(List<SteeringBehaviour> behaviours, AIData aiData)
   {
      var danger = new float[8];
      var interest = new float[8];

      //Loop through each behaviour
      foreach (var behaviour in behaviours)
      {
         (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
      }

      //Subtract danger values from interest array
      for (var i = 0; i < 8; i++)
      {
         //This removes directions that we don't want to move in - 0 being don't move
         interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
      }

      _interestGizmo = interest;
      
      //Get the average direction
      var outputDirection = Vector3.zero;
      
      
      //Get the average direction
      for(var i = 0; i < 8; i++)
      {
         outputDirection += Directions.eightDirections[i] * interest[i];
      }
      
      //Normalise the direction
      outputDirection.Normalize();
      //Set the result direction
      _resultDirection = outputDirection;
      //Return the direction
      return _resultDirection;
   }

   private void OnDrawGizmos()
   {
      if (!showGizmos) return;
      
      Gizmos.color = Color.yellow; 
      //Gizmos.DrawRay(transform.position, _resultDirection * _rayLength);
   }
}
