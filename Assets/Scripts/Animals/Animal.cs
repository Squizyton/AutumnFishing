using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using States;
using UnityEngine;

public class Animal : SerializedMonoBehaviour
{
    [Title("Base Variables")]
    [SerializeField] private Animator anim;
    public AnimalSO animalInfo;
    public LayerMask foragableLayer;


    [Title("Stats")] [SerializeField] private float happiness;
    
    [Title("States")]
    [SerializeField]private State currentState;
    

    [Title("Debug")] [SerializeField] private Vector3 offset;


    private void Start()
    {
        currentState = new WanderAround();
        currentState.OnInitialized(this);
    }

    public void Update()
    {
        currentState.Update();
    }
    
    
    
    public void TransitionToState(State nextState)
    {
        currentState.OnExit();
        currentState = nextState;
        currentState.OnInitialized(this);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, animalInfo.sightRadius);
    }
}
