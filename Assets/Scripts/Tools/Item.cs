using UnityEngine;

namespace Tools
{
    public abstract class Tool : MonoBehaviour,IItemAction
    {

        public abstract void OnEquip();
        public abstract void OnUnequip();
        public abstract void OnLeftClick();
        public abstract void OnLetGo();
        public abstract void OnRightClick();
    }
}
