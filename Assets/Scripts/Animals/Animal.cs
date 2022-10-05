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

[RequireComponent(typeof(AIData))]
public class Animal : SerializedMonoBehaviour
{
    [Title("Transforms")] 
    [SerializeField] private Transform _head;


    [Title("AI")] [SerializeField] private AIData ai;

    //TODO: Move these to States, so they aren't running constantly
    [ReadOnly] private List<Detector> detectors;
    [ReadOnly] private List<SteeringBehaviour> steeringBehaviours;
    [SerializeField] private float detectionDelay = 0.05f;
    [Title("Base Variables")] [SerializeField]
    private Animator anim;

    [Required, OnValueChanged("@ChangeAIData()")]
    public AnimalSO animalInfo;

    public LayerMask foragableLayer;


    [Title("Stats")] 
    [SerializeField] private float happiness;

    [Title("States")] 
    [ReadOnly,SerializeField] private State currentState;


    [Title("Debug")] 
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        //Create a list of detectors and steering behaviours needed
        detectors = new List<Detector>();
        steeringBehaviours = new List<SteeringBehaviour>();

        //Create a base state
        currentState = new WanderAround();
        currentState.OnInitialized(this, ai);
        //Invoke Reating on Detecting to not kill performance
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

    //Update the current state
    public void Update()
    {
        currentState?.Update();
    }
    //Fixed update for physics
    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    
    //Change the current state
    public void TransitionToState(State nextState)
    {
        //Exit the current state - This is usually cleaning up
        currentState.OnExit();
        //Set the current state to the next state
        currentState = nextState;
        //Initialize the new state
        currentState.OnInitialized(this, ai);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, animalInfo.sightRadius);
    }

    //Return the behaviours
    public List<SteeringBehaviour> GetSteeringBehaviours()
    {
        return steeringBehaviours;
    }


    public void SetAnimState(string value)
    {
        anim.SetTrigger(value);
    }

    
    //Add/Remove detectors and steering behaviours
    #region AI

    public void AddBehaviour(SteeringBehaviour behaviour)
    {
        behaviour.OnStart(transform, ai);
        steeringBehaviours.Add(behaviour);
    }

    public void AddDetector(Detector detector)
    {
        detector.OnStart(transform, ai.obstaclesLayerMask);
        detectors.Add(detector);
    }

    public void RemoveEverything()
    {
        detectors.Clear();
        steeringBehaviours.Clear();
    }

    #endregion


    #region Editor Automatic Updates

    public void ChangeAIData()
    {
        ai.sightRadius = animalInfo.sightRadius;
        //Get the width of the collider bounds
        var width = transform.GetComponent<CapsuleCollider>().bounds.size.x;
        ai.colliderRadius = width / 2;
        ai.targetReachThreshold = animalInfo.targetReachedThreshold;
    }

    #endregion
}