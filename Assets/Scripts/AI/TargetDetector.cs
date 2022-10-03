using System.Collections;
using System.Collections.Generic;
using AI;
using Sirenix.OdinInspector;
using UnityEngine;

public class TargetDetector : Detector
{
    private float targetDetectionRange = 8.24f;

    [SerializeField] private LayerMask obstaclesLayer, playerLayerMask;


    [Title("Debug Values")] [SerializeField]
    private bool showGizmos;

    [SerializeField] private List<Transform> colliders;

    
     public void OnStartUp(LayerMask layer)
    {
        this.obstaclesLayer = layer;
    }
    
    public override void Detect(AIData aiData)
    {
        //Find out if player is near
        var playerCollider = Physics.OverlapSphere(transform.position, targetDetectionRange, playerLayerMask);


        if (playerCollider.Length > 0)
        {
            
            Debug.Log("Dick Tit Fuck McGuck");
            var direction = (playerCollider[0].transform.position - transform.position).normalized;

            Physics.Raycast(transform.position, direction, out var hit, targetDetectionRange, obstaclesLayer);

            if (hit.collider && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                colliders = new List<Transform>() {playerCollider[0].transform};
            }
            else
            {
                Debug.Log("This is being called");
                colliders = null;
            }
        }
        else colliders = null;
        
        aiData.targets = colliders;
    }


    private void OnDrawGizmosSelected()
    {
        if (showGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}