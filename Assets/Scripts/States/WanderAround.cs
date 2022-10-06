using System.Collections.Generic;
using System.Linq;
using AI;
using Player;
using UnityEngine;


//TODO: Add context steering
namespace States
{
    public class WanderAround : State
    {
        private Transform _transform;


    
        private ContextSolver contextSolver;
        private Transform waypoint;

        private Collider[] cachedColliders;

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

            animal.SetAnimState("walk");
        }

        public override void Update()
        {
            CheckColliders();
        }

        public override void FixedUpdate()
        {
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
            //Colision detection
            const int maxColliders = 10;
            //Create a hitColliders array based on the maxColliders
            var hitColliders = new Collider[maxColliders];
            //Get all the colliders that are within the radius of the animal using Overlap sphere
            var numColliders =
                Physics.OverlapSphereNonAlloc(_transform.position, animal.animalInfo.sightRadius, hitColliders, animal.foragableLayer);

            if (cachedColliders == hitColliders) return;
            
            
            
            cachedColliders = hitColliders;
            //Loop through all the colliders
            for (var i = 0; i < numColliders; i++)
            {
                //If the collider is a foragable object
                if (!hitColliders[i].gameObject.CompareTag("Food")) continue;

                
        
                //Get the foragable component
                hitColliders[i].TryGetComponent(out PickupableObject foundFood);


                if (foundFood.IsPicked()) return;
                
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

                Debug.Log(weightGenerated);
                //If the weight is less than the food weight
                if (!(weightGenerated > favoriteFood.weight)) continue;
                
                
                animal.TransitionToState(new EatingState(hitColliders[i].gameObject,foundFood));
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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(_targetPosition, 0.5f);
        }
    }
}