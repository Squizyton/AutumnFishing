using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using States;
using UnityEngine;

public class EatingState : State
{
    private Transform _head;
    private Quaternion directionToTurn;
    private GameObject _foodGoingToEat;
    private bool aboveFood;
    private bool eating;
    private PickupableObject _food;

    public override void OnInitialized(Animal passedAnimal, AIData passedData)
    {
        animal = passedAnimal;
        animalAI = passedData;
        _head = animal.head;
        directionToTurn = Quaternion.LookRotation(_foodGoingToEat.transform.position - animal.transform.position);
    }

    public EatingState(GameObject food, PickupableObject foodScript)
    {
        _foodGoingToEat = food.gameObject;
        _food = foodScript;
    }

    public override void Update()
    {
        if (!Physics.Raycast(_head.transform.position, Vector3.down, out var hit, 1f, animal.foragableLayer)) return;
        if (hit.collider.gameObject == _foodGoingToEat)
        {
            aboveFood = true;
        }
    }

    public override void FixedUpdate()
    {
        if (animal.transform.rotation != directionToTurn)
            animal.transform.rotation =
                Quaternion.RotateTowards(animal.transform.rotation, directionToTurn, 100f * Time.deltaTime);

        if (!aboveFood)
            animal.transform.Translate(Vector3.forward * (animal.animalInfo.walkSpeed * Time.deltaTime));

        if (!aboveFood) return;
        if (eating) return;

        animal.SetAnimState("eat");
        EatFood(Random.Range(5f, 10f));
        eating = true;
    }


    private async void EatFood(float duration)
    {
        var endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            await Task.Yield();
        }

        _food.OnEaten();
        animal.TransitionToState(new WanderAround());
    }

    public override void OnExit()
    {
    }
}