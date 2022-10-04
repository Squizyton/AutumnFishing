using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class TargetDetector : Detector
    {
        private float targetDetectionRange = 8.24f;

        private LayerMask  playerLayerMask;


        [Title("Debug Values")] [SerializeField]
        private bool showGizmos = true;

        [SerializeField] private List<Transform> colliders;

    
    
        public override void Detect(AIData aiData)
        {
            //Find out if player is near
            var playerCollider = Physics.OverlapSphere(_transform.position, targetDetectionRange, playerLayerMask);


            if (playerCollider.Length > 0)
            {
                var direction = (playerCollider[0].transform.position - _transform.position).normalized;

                Physics.Raycast(_transform.position, direction, out var hit, targetDetectionRange, _layerMask);

                if (hit.collider && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Debug.DrawRay(_transform.position, direction * targetDetectionRange, Color.magenta);
                    colliders = new List<Transform>() {playerCollider[0].transform};
                }
                else
                {
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

            Gizmos.DrawWireSphere(_transform.position, targetDetectionRange);

            if (colliders == null)
                return;
            Gizmos.color = Color.magenta;
            foreach (var item in colliders)
            {
                Gizmos.DrawSphere(item.position, 0.3f);
            }
        }
    }
}