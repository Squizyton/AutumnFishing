using Interface;
using UnityEngine;

namespace ForagableMaterial
{
    [CreateAssetMenu(menuName = "New Fauna", fileName = "New Fauna")]
    public class Vegetation : ScriptableObject, IPicturable
    {
        //Material name
        public string materialName;
        //The material itself
        public GameObject prefab;
        //Sprite for UI
        public Sprite sprite;
        //The amount of material that can be gathered
        public int maxStack;
        
        //Fullness of the material
        public float fullness;
        public string ReturnName()
        {
            return materialName;
        }
    }
}