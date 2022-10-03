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
    private Vector3 _targetPosition; // The position we are moving towards
    private Transform _transform;
    private Quaternion _lookRotation;
    
    
    private AIData animalAI;
    private ContextSolver contextSolver;
    public override void OnInitialized(Animal passedAnimal, AIData passedAI)
    {
        animal = passedAnimal;
        animalAI = passedAI;
        _transform = animal.transform;
        //Generate a random position to move towards
        _targetPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        _lookRotation =  Quaternion.LookRotation(_targetPosition - _transform.position);
        
        //TODO: Find a better way to do this
        var obstacleAvoidenceBehaviour = new GameObject();
        animal.AddBehaviour(obstacleAvoidenceBehaviour.AddComponent<ObstacleAvoidanceBehaviour>());
        var targetDetectionBehaviour = new GameObject().AddComponent<ObstacleDetector>();
        targetDetectionBehaviour.OnStartUp(animalAI.obstaclesLayerMask);
        animal.AddDetector(targetDetectionBehaviour);
        var seekBehaviour = new GameObject().AddComponent<SeekBehaviour>();
        animal.AddBehaviour(seekBehaviour);
        var solver = new GameObject().AddComponent<ContextSolver>();
        solver.transform.parent.SetParent(passedAnimal.gameObject.transform);
        contextSolver = solver;
        
        animalAI.FeedTarget(Object.Instantiate(new GameObject(), _targetPosition, Quaternion.identity).transform);
    }

    public override void Update()
    {
        CheckColliders();
    }

    public override void FixedUpdate()
    { 
        //Rotate it towards the target position
        _transform.rotation = Quaternion.Slerp(_transform.rotation, _lookRotation, Time.deltaTime * 5f);
        
        //Move towards the target position based on context solver
        _transform.position += contextSolver.GetDirectionToMove(animal.GetSteeringBehaviours(),animalAI) * (Time.deltaTime * animal.animalInfo.walkSpeed);
        

        //If we are close enough to the target position, generate a new one
        if (!(Vector3.Distance(animal.transform.position, _targetPosition) < 0.1f)) return;
        
        _targetPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        _lookRotation =  Quaternion.LookRotation(_targetPosition - _transform.position);
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
                hitColliders[i].TryGetComponent(out Vegetation foundFood);
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
        Gizmos.DrawWireSphere(_targetPosition, 0.5f);
    }
}