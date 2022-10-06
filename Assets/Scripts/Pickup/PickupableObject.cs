using System.Collections;
using System.Collections.Generic;
using ForagableMaterial;
using Player;
using UnityEngine;

public abstract class PickupableObject : MonoBehaviour
{
    [SerializeField] protected Vegetation flora;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected bool isPicked;

    public virtual void OnEaten()
    {
        Destroy(gameObject);
    }

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

    public bool IsPicked()
    {
        return isPicked;
    }

    public Vegetation ReturnFlora()
    {
        return flora;
    }
}