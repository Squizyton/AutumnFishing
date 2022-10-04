using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
  public class ObstacleDetector : Detector
  {

    private Transform transform;
    
    //The range in which the AI can detect things
    [SerializeField] private float detectionRadius = 8.24f;
    

    [Title("Colliders")] private Collider[] colliders;
  
  
    [Title("Debug")] [SerializeField] private bool showGizmos = true;
    
    public override void Detect(AIData aiData)
    {
      //TODO: Change to NonAlloc
      colliders = Physics.OverlapSphere(_transform.position, detectionRadius, _layerMask);
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

