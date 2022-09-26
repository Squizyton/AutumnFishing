using UnityEngine;

namespace Tools
{
    public abstract class Tool : MonoBehaviour,IItemAction
    {
        public abstract void OnLeftClick();
        public abstract void OnLetGo();
        public abstract void OnRightClick();
    }
}
