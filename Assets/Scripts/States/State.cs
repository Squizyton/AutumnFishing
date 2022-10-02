using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    public abstract void Update();
    public abstract void OnIntialized();
    public abstract void OnExit();
    
}
