using Unity;
using System;

namespace States
{
    public abstract class State
    {
        protected Animal animal;
        
        public abstract void OnInitialized(Animal passedAnimal);
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void OnExit();
    
    }
}
