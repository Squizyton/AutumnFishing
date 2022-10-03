using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [Title("Thresholds")]
    //In case Agent has lost sight of target, we need to know where it was last seen. This will tell us if we are close enough
    [SerializeField]
    private float targetReachedThreshold = 0.5f;

    private bool _reachedLastTarget = true;


    [Title("Debug Variables")] [SerializeField]
    private bool showGizmos = true;

    [ShowIf("@showGizmos")] [SerializeField]
    private Vector3 targetPositionCached;

    [ShowIf("@showGizmos")] [SerializeField]
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        //if we don't have a target stop seeking
        //else set a new Target
        if (_reachedLastTarget)
        {
            if (aiData.targets is not {Count: > 0})
            {
                aiData.currentTarget = null;
                return (danger, interest);
            }
            else
            {
                aiData.currentTarget = aiData.targets
                    .OrderBy(target => Vector3.Distance(target.position, transform.position)).FirstOrDefault();
                _reachedLastTarget = false;
            }
        }

        //Cache the last position only if we still see the target (if the targets collection is not empty)
        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
        {
            targetPositionCached = aiData.currentTarget.position;
        }


        //Frst check if we have reached the target
        if (Vector3.Distance(transform.position, targetPositionCached) < targetReachedThreshold)
        {
            _reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, interest);
        }


        //if we haven't yet reeach the target do the main logic of finding the interest directions
        var directionToTarget = (targetPositionCached - transform.position);

        for (var i = 0; i < interest.Length; i++)
        {
            var result = Vector3.Dot(directionToTarget.normalized, Directions.eightDirections[i]);


            //Accept only directions at the less than 90 degrees from the target direction
            if (!(result > 0)) continue;

            if (result > interest[i]) ;
            interest[i] = result;
        }

        interestsTemp = interest;
        return (danger, interest);
    }


    private void OnDrawGizmos()
    {
        if (!showGizmos) return;


        if (interestsTemp != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < interestsTemp.Length; i++)
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i]);
        }

        if (_reachedLastTarget == false)
            Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPositionCached, 0.1f);
    }
}