using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace AI
{
    public class ObstacleAvoidanceBehaviour : SteeringBehaviour
    {
    
        //Debug valu
        [Title("Debug")] [SerializeField] private bool showGizmos = true;
        private float[] dangersResultTemp = null;


        public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
        {
            foreach (var obstacleCollider in aiData.obstacles)
            {
                //Get the direction to the obstacle based on how close it is to the agent on the closest point
                var directionToObstacle = obstacleCollider.ClosestPoint(_transform.position) - _transform.position;

                //
                var distanceToObstacle = directionToObstacle.magnitude;

                //Calculate weight based on the Distance animal<---->Obstacle
                var weight = distanceToObstacle <= agentColliderSize ? 1 : (radius - distanceToObstacle) / radius;

                var directionNormalized = directionToObstacle.normalized;


                //Add obstacle paremeters to the danger array
                for (var i = 0; i < Directions.eightDirections.Count; i++)
                {
                    //Get the angle between the direction to the obstacle and the direction of the current index
                    var result = Vector3.Dot(directionNormalized, Directions.eightDirections[i]);

                
                    var valueToPutIn = result * weight;

                    //override value only if it is higher then the current one stored in the danger array
                    if (valueToPutIn > danger[i])
                        danger[i] = valueToPutIn;
                }
            }

            dangersResultTemp = danger;
            return (danger, interest);
        }


        private void OnDrawGizmos()
        {
            if (!showGizmos) return;
        
            if(dangersResultTemp == null) return;

            Gizmos.color = Color.red;
        
            for(var i = 0; i < dangersResultTemp.Length; i++)
            {
                Gizmos.DrawRay(_transform.position, Directions.eightDirections[i] * dangersResultTemp[i]);
            }
        }
    }




//Static class to store all the directions
    public static class Directions
    {
        public static List<Vector3> eightDirections = new()
        {
            Vector3.forward,
            Vector3.forward + Vector3.right,
            Vector3.right,
            Vector3.right + Vector3.back,
            Vector3.back,
            Vector3.back + Vector3.left,
            Vector3.left,
            Vector3.left + Vector3.forward
        };
    }
}