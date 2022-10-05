using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

public class BerryBush : PickupableObject
{

    [Title("The Berries")]
    [SerializeField] private GameObject berries;

    [SerializeField] private bool isPicked;
    [Title("Respawn Time")]
    [SerializeField] private float respawnTime = 240f;

    
    
    public override async void OnEaten()
    {
        berries.SetActive(false);
        var task = new Task(() =>
        {
            Regrow(respawnTime);
        });
        
        await Task.WhenAll(task);
        
        Debug.Log("Berries have respawned");
        
    }

    public override  void OnPickup()
    {
        if (isPicked) return;
        
        if (PlayerInventory.Instance.AddToInventory(flora))
        {
            Debug.Log("Berries have been added to the inventory");
            berries.SetActive(false);
            isPicked = true;
            Regrow(duration:respawnTime);
        }
        else
        {
            //TODO: Add a shake animation
            //Shake Animation
        }
    }


    private async void Regrow(float duration)
    {
        var end = Time.time + duration;
        while (Time.time < end)
        {
            // Wait for the next tick.
            await Task.Yield();
        }
        // Regrow the berries.
        berries.SetActive(true);
        isPicked = false;
        Debug.Log("Berries have respawned");
    }
}
