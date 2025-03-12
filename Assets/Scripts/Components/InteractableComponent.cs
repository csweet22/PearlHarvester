using System;
using UnityEngine;
using UnityEngine.Events;

namespace Content.Scripts.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        public UnityEvent startInteractEvent;
        public UnityEvent endInteractEvent;
        public event Action<InteractorComponent> StartInteractAction;
        public event Action<InteractorComponent> EndInteractAction;

        public virtual void OnInteractStart(InteractorComponent interactorComponent = null)
        {
            if (interactorComponent == null){
                Debug.LogError("No Interactor Found.");
                return;
            }

            if (!interactorComponent.canInteract)
                return;

            interactorComponent.OnInteractStart(this);

            Debug.Log("InteractableComponent: OnInteractStart");
            StartInteractAction?.Invoke(interactorComponent);
            startInteractEvent?.Invoke();
        }

        public virtual void OnInteractEnd(InteractorComponent interactorComponent = null)
        {
            if (interactorComponent == null){
                Debug.LogError("No Interactor Found.");
                return;
            }

            if (!interactorComponent.canInteract && !interactorComponent.CompareTag("AxeProjectile"))
                return;

            interactorComponent.OnInteractEnd(this);

            Debug.Log("InteractableComponent: OnInteractEnd");
            EndInteractAction?.Invoke(interactorComponent);
            endInteractEvent?.Invoke();
        }
    }
}