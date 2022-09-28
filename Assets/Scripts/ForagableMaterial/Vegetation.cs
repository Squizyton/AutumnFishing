using UnityEngine;

namespace ForagableMaterial
{
    [CreateAssetMenu(menuName = "New Fauna", fileName = "New Fauna")]
    public class Vegetation : ScriptableObject
    {
        public string materialName;
        public Sprite sprite;   
        public int maxStack;
    }
}