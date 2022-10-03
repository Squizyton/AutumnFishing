using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
  public class ObstacleDetector : Detector
  {
  
    //The range in which the AI can detect things
    [SerializeField] private float detectionRadius = 8.24f;
  
    [SerializeField] private LayerMask layerMask;

    [Title("Colliders")] private Collider[] colliders;
  
  
    [Title("Debug")] [SerializeField] private bool showGizmos;


    public override void Detect(AIData aiData)
    {
      //TODO: Change to NonAlloc
      colliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);
      aiData.obstacles = colliders;
    }


    private void OnDrawGizmos()
    {
      if (!showGizmos) return;

      if (!Application.isPlaying || colliders == null) return;
      
      Gizmos.color = Color.red;
        
      foreach(var obstacle in colliders)
        Gizmos.DrawSphere(obstacle.transform.position, 0.2f);
    }
  }
}

