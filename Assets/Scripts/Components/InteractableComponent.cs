using System;
using UnityEngine;
using UnityEngine.Events;

namespace Content.Scripts.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        public UnityEvent startInteractEvent;
        public UnityEvent endInteractEvent;
        public event Action StartInteractAction;
        public event Action EndInteractAction;
        
        public virtual void OnInteractStart(InteractorComponent interactorComponent = null)
        {
            if (interactorComponent == null){
                Debug.LogError("No Interactor Found.");
                return;
            }

            interactorComponent.OnInteractStart(this);
            
            Debug.Log("InteractableComponent: OnInteractStart");
            StartInteractAction?.Invoke();
            startInteractEvent?.Invoke();
        }
        
        public virtual void OnInteractEnd(InteractorComponent interactorComponent = null)
        {
            if (interactorComponent == null){
                Debug.LogError("No Interactor Found.");
                return;
            }

            interactorComponent.OnInteractEnd(this);
            
            Debug.Log("InteractableComponent: OnInteractEnd");
            EndInteractAction?.Invoke();
            endInteractEvent?.Invoke();
        }
    }
}
