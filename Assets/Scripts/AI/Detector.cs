using UnityEngine;

namespace AI
{
    public abstract class Detector : MonoBehaviour
    {
        public abstract void Detect(AIData aiData);
    }
}
