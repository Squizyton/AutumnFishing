using Sirenix.OdinInspector;
using UnityEngine;

namespace Singleton
{
    public class SingletonBehaviour<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    if (!_instance)
                    {
                         var obj = new GameObject();
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}

