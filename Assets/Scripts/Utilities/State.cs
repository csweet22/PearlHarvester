using System;
using UnityEngine;

namespace Content.Scripts.Utilities
{
    public class State : MonoBehaviour
    {
        public Action<string> transitionRequest;
        
        public virtual void OnEnter()
        {
            // Debug.Log($"Entered {gameObject.name}");
        }

        public virtual void OnExit()
        {
            // Debug.Log($"Exited {gameObject.name}");
        }

        public virtual void UpdateTick()
        {
            
        }

        public virtual void FixedUpdateTick()
        {
            
        }

        protected void EmitTransitionRequest(string newStateName)
        {
            transitionRequest?.Invoke(newStateName);
        }
    }
}
