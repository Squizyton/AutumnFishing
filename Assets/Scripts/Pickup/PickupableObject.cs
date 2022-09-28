using System.Collections;
using System.Collections.Generic;
using ForagableMaterial;
using UnityEngine;

public abstract class PickupableObject : MonoBehaviour
{
    [SerializeField] private Vegetation flora;
    [SerializeField] private Rigidbody rb;
    
    
    public virtual void OnPickup()
    {
        if(PlayerInventory.Instance.AddToInventory(flora))
            Destroy(gameObject);
        else
        {
            //add force in random direction
            rb.AddForce(Random.insideUnitSphere * 10, ForceMode.Impulse);
        }
    }
}