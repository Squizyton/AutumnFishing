using UnityEngine;

namespace ForagableMaterial
{
    [CreateAssetMenu(menuName = "New Fauna", fileName = "New Fauna")]
    public class Vegetation : ScriptableObject
    {
        //Material name
        public string materialName;
        public GameObject prefab;
        public Sprite sprite;   
        public int maxStack;
    }
}