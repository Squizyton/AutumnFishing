using System.Collections.Generic;
using System.Linq;
using ForagableMaterial;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Player;
namespace Tools
{
    public class Throwing : MonoBehaviour
    {
        [Title("Litter Fall")] [SerializeField]
        private PlayerInventory.StoredItem currentItem;
        private Transform spawnPoint;
        private GameObject currentLitterObject;

        [Title("Current Variables")]
        [SerializeField] private float currentThrowForce;

        [SerializeField] private int currentIndex;
        
        [Title("Max Variables")]
        [SerializeField] private float maxThrowForce;






        public void SwitchObject(int value)
        {
            currentIndex = (currentIndex + value) % PlayerInventory.Instance.foragableMaterials.Count;
            currentItem = PlayerInventory.Instance.foragableMaterials.Values.ToList().ElementAt(currentIndex);
            
            
            
            Destroy(currentLitterObject);
            currentLitterObject = Instantiate(currentItem.vegetation.prefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
