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

            if (!interactorComponent.CanInteract)
                return;

            interactorComponent.OnInteractStart(this);

            StartInteractAction?.Invoke(interactorComponent);
            startInteractEvent?.Invoke();
        }

        public virtual void OnInteractEnd(InteractorComponent interactorComponent = null)
        {
            if (interactorComponent == null){
                Debug.LogError("No Interactor Found.");
                return;
            }

            if (!interactorComponent.CanInteract && !interactorComponent.CompareTag("AxeProjectile"))
                return;

            interactorComponent.OnInteractEnd(this);

            EndInteractAction?.Invoke(interactorComponent);
            endInteractEvent?.Invoke();
        }
    }
}