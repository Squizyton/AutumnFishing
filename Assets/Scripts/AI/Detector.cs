using UnityEngine;

namespace AI
{
    public abstract class Detector
    {
        
        
        protected Transform _transform;
        protected LayerMask _layerMask;
        public virtual void OnStart(Transform passedTransform, LayerMask passedLayerMask)
        {
           
            _transform = passedTransform;
            _layerMask = passedLayerMask;
        }
        public abstract void Detect(AIData aiData);
    }
}
