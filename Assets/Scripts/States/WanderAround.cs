using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI;
using ForagableMaterial;
using States;
using UnityEngine;


//TODO: Add context steering
public class WanderAround : State
{
    private Transform _transform;
   
    
    
    private AIData animalAI;
    private ContextSolver contextSolver;
    private Transform waypoint;
    public override void OnInitialized(Animal passedAnimal, AIData passedAI)
    {
        animal = passedAnimal;
        animalAI = passedAI;
        _transform = animal.transform;
        waypoint = new GameObject().transform;
        waypoint.position = new Vector3(24.5f,0,91.7f);
        
        //Ai things
        animal.AddDetector(new ObstacleDetector());
        animal.AddDetector(new TargetDetector());
        animal.AddBehaviour(new ObstacleAvoidanceBehaviour());
        animal.AddBehaviour(new SeekBehaviour());
        contextSolver = new ContextSolver();
        
        animalAI.FeedTargets(new List<Transform>{waypoint});
    }

    public override void Update()
    {
        CheckColliders();
    }

    public override void FixedUpdate()
    { 
     
        
        //Rotate it based on its forward vector
        _transform.rotation = Quaternion.Slerp(_transform.rotation,Quaternion.LookRotation(_transform.forward + contextSolver.GetDirectionToMove(animal.GetSteeringBehaviours(),animalAI)),Time.deltaTime * 5f);
        
        //Move towards the target position based on context solver
        _transform.position += (_transform.forward + contextSolver.GetDirectionToMove(animal.GetSteeringBehaviours(),animalAI)) * (Time.deltaTime * animal.animalInfo.walkSpeed);
        

        //If we are close enough to the target position, generate a new one
        if (!(Vector3.Distance(animal.transform.position,waypoint.position) < 0.1f)) return;
      
    }   



    private void CheckColliders()
    {
        //Colision detection
        const int maxColliders = 10;
        var hitColliders = new Collider[maxColliders];
        var numColliders = Physics.OverlapSphereNonAlloc(_transform.position, 0.5f, hitColliders, animal.foragableLayer);

        if (!hitColliders[0]) return;
        
        for (var i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].gameObject.CompareTag("Food"))
            {
                hitColliders[i].TryGetComponent(out PickupableObject foundFood);
                if (animal.animalInfo.willEatAnything)
                {
                    var weight = Random.Range(0, 1f);

                    if (weight < animal.animalInfo.stopWeight)
                        //TODO: Change state to eating state
                        return;
                }

                var favoriteFood = animal.animalInfo.foodItWillEat.FirstOrDefault(eatFood => eatFood.food == foundFood);

                if (favoriteFood != null)
                {
                    var weightGenerated = Random.Range(0, 1f);

                    if (weightGenerated < favoriteFood.weight)
                    {
                        //TODO: Switch State to eating state;
                    }
                }
            }

            if (hitColliders[i].gameObject.CompareTag("Player"))
            {
                //Check if player is sprinting, if so, change state to runaway
                
            }
        }
    }

    public override void OnExit()
    {
        animal.RemoveEverything();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(_targetPosition, 0.5f);
    }
}