using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AI;
using Sirenix.OdinInspector;
using States;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Animal : SerializedMonoBehaviour
{

    [Title("AI")] 
    [SerializeField] private AIData ai;
    //TODO: Move these to States, so they aren't running constantly
    [SerializeField,ReadOnly] private List<Detector> detectors;
    [SerializeField,ReadOnly]private List<SteeringBehaviour> steeringBehaviours;
    [SerializeField] private float detectionDelay = 0.05f;
    [SerializeField] private Transform DetectorHolder;
    [SerializeField] private Transform SteeringBehaviourHolder;
    
    
    [Title("Base Variables")]
    [SerializeField] private Animator anim;
    public AnimalSO animalInfo;
    public LayerMask foragableLayer;


    [Title("Stats")] [SerializeField] private float happiness;
    
    [Title("States")]
    [SerializeField]private State currentState;
    

    [Title("Debug")] [SerializeField] private Vector3 offset;
    [SerializeField] private LayerMask obstacleMask;
    private void Start()
    {
        detectors = new List<Detector>();
        steeringBehaviours = new List<SteeringBehaviour>();
        
        
        currentState = new WanderAround();
        currentState.OnInitialized(this,ai);

        InvokeRepeating(nameof(PerformDetection), 0, detectionDelay);
    }

    //Detector's stay on the animal, so they can be updated here
    private void PerformDetection()
    {
        foreach (var detector in detectors)
        {
            detector.Detect(aiData: ai);
        }
        
    }
    
    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
    
    public void TransitionToState(State nextState)
    {
        currentState.OnExit();
        currentState = nextState;
        currentState.OnInitialized(this,ai);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, animalInfo.sightRadius);
    }
    
    public List<SteeringBehaviour> GetSteeringBehaviours()
    {
        return steeringBehaviours;
    }
    
    public void AddBehaviour(SteeringBehaviour behaviour)
    {
        behaviour.OnStart(transform);
        steeringBehaviours.Add(behaviour);
        
    }
    
    public void AddDetector(Detector detector)
    {
        detector.OnStart(transform,ai.obstaclesLayerMask);
        detectors.Add(detector);
      
    }

    public void RemoveEverything()
    {
        detectors.Clear();
        steeringBehaviours.Clear();
    }
}
