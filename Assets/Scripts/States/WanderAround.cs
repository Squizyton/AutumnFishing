using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;


//TODO: Add context steering
namespace States
{
    public class WanderAround : State
    {
        private Transform _transform;


    
        private ContextSolver contextSolver;
        private Transform waypoint;
        
        
        private Collider[] hitColliders;
        private Collider[] cachedColliders;
        
        private PickupableObject[] foodFoundNotEaten;
        private int removeRateTime = 30;
        private float timer = 0;
        public override void OnInitialized(Animal passedAnimal, AIData passedAI)
        {
            animal = passedAnimal;
            animalAI = passedAI;
            _transform = animal.transform;
            waypoint = new GameObject().transform;
            waypoint.position = new Vector3(24.5f, 0, 91.7f);

            //Ai things
            animal.AddDetector(new ObstacleDetector());
            animal.AddDetector(new TargetDetector());
            animal.AddBehaviour(new ObstacleAvoidanceBehaviour());
            animal.AddBehaviour(new SeekBehaviour());
            contextSolver = new ContextSolver();

            animalAI.FeedTargets(new List<Transform> {waypoint});
            cachedColliders = Array.Empty<Collider>();
            hitColliders = new Collider[10];
            foodFoundNotEaten = Array.Empty<PickupableObject>();
            
            animal.SetAnimState("walk");
        }

        public override void Update()
        {
      CheckDistanceFromCachedColliders();


            if (!(timer > 0)) return;
            
            timer -= Time.deltaTime;
            if(timer <= 0)
                RemoveRandomFood();
        }

        public override void FixedUpdate()
        {
            CheckColliders();
            
            //Rotate it based on its forward vector
            _transform.rotation = Quaternion.Slerp(_transform.rotation,
                Quaternion.LookRotation(_transform.forward +
                                        contextSolver.GetDirectionToMove(animal.GetSteeringBehaviours(), animalAI)),
                Time.deltaTime * 5f);

            //Move towards the target position based on context solver
            _transform.position +=
                (_transform.forward + contextSolver.GetDirectionToMove(animal.GetSteeringBehaviours(), animalAI)) *
                (Time.deltaTime * animal.animalInfo.walkSpeed);


            //If we are close enough to the target position, generate a new one
            if (!(Vector3.Distance(animal.transform.position, waypoint.position) < 0.1f)) return;
        }
        
        private void CheckColliders()
        {
          
            //Create a hitColliders array based on the maxColliders
            //Get all the colliders that are within the radius of the animal using Overlap sphere
            var numColliders =
                Physics.OverlapSphereNonAlloc(_transform.position, animal.animalInfo.sightRadius, hitColliders, animal.foragableLayer);

      
            
            //Loop through all the colliders
            for (var i = 0; i < numColliders; i++)
            {

                if (!cachedColliders.Contains(hitColliders[i]))
                    cachedColliders = cachedColliders.Append(hitColliders[i]).ToArray();
                else return;
                
                //If the collider is a foragable object
                if (!hitColliders[i].gameObject.CompareTag("Food")) continue;
                
                    //Debug.Log(hitColliders[i].gameObject.name);
                
                
                
                
                //Get the foragable component
                hitColliders[i].TryGetComponent(out PickupableObject foundFood);

                

                if (foundFood.IsPicked() || foodFoundNotEaten.Contains(foundFood)) return;
                
                //If the animal will eat anything, 
                if (animal.animalInfo.willEatAnything)
                {
                    //Generate a weight
                    var weight = Random.Range(0, 1f);

                    //If the weight is less than the animals stopWeight
                    if (weight < animal.animalInfo.stopWeight)
                        //Eat the food
                        animal.TransitionToState(new EatingState(hitColliders[i].gameObject,foundFood));
                    return;
                }

                //If the animal will only eat certain foods
                var favoriteFood =
                    animal.animalInfo.foodItWillEat.FirstOrDefault(eatFood => eatFood.food == foundFood.ReturnFlora());

                //Check to see if the food in list
                if (favoriteFood == null) continue;
                
                
                //If the food is marked as favorite, increase the multiplyer
                var multiplier = favoriteFood.isFavorite ? 2f : 1f;

                //TODO: Add if Food was thrown by player, increase the multiplyer
                
                //generate a weight
                var weightGenerated = Random.Range(0, 1f) * multiplier;
            

                //The more full the animal is, the less likely it is to eat
                //Lower the number, the less likely it is to eat
                weightGenerated -= animal.hunger / 100f;
                
                
                //Debug.Log(weightGenerated);
                
                //If the weight is less than the food weight
                if (weightGenerated < favoriteFood.weight)
                    animal.TransitionToState(new EatingState(hitColliders[i].gameObject,foundFood));
                //If we decided not to eat it, add it to the list of food we found but didn't eat
                else
                {
                    Debug.Log("Found food but didn't eat it");
                    
                    if(foodFoundNotEaten.Length <= 0)
                        timer = removeRateTime;
                    
                    foodFoundNotEaten.ToList().Add(foundFood);
                }
            }


            //Check distance from player
            if (!(Vector3.Distance(animal.transform.position, PlayerMovement.Instance.GetPosition()) < 1.5f)) return;

            if (PlayerMovement.Instance.isSprinting)
                Debug.Log("RUN AWAY AAAAAAH");
            //animal.TransitionToState(new RunawayState());
            
            
        }

        public override void OnExit()
        {
            animal.RemoveEverything();
        }

        
        private void RemoveRandomFood()
        {
            foodFoundNotEaten.ToList().RemoveAt(Random.Range(0, foodFoundNotEaten.Length));
            
            if(foodFoundNotEaten.Length > 0)
                timer = removeRateTime;
        }


        private void CheckDistanceFromCachedColliders()
        {
            //Check distance from cached colliders
            for (var i = 0; i < cachedColliders.Length; i++)
            {
                if (!cachedColliders[i]) continue;
                
                if (!(Vector3.Distance(animal.transform.position, cachedColliders[i].transform.position) <  animal.animalInfo.sightRadius)) continue;
                
                cachedColliders.ToList().RemoveAt(i);
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(_targetPosition, 0.5f);
        }
    }
}