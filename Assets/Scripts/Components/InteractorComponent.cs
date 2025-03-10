using System;
using UnityEngine;
using UnityEngine.Events;

namespace Content.Scripts.Components
{
    public class InteractorComponent : MonoBehaviour
    {
        public UnityEvent startInteractEvent;
        public UnityEvent endInteractEvent;
        public event Action StartInteractAction;
        public event Action EndInteractAction;

        public bool canInteract;

        public void ActivateInteractable()
        {
            canInteract = true;
        }

        public void DeactivateInteractable()
        {
            canInteract = false;
        }

        public virtual void OnInteractStart(InteractableComponent interactableComponent)
        {
            if (!canInteract)
                return;
            Debug.Log("InteractorComponent: OnInteractStart");
            StartInteractAction?.Invoke();
            startInteractEvent?.Invoke();
        }

        public virtual void OnInteractEnd(InteractableComponent interactableComponent)
        {
            if (!canInteract)
                return;
            Debug.Log("InteractorComponent: OnInteractEnd");
            EndInteractAction?.Invoke();
            endInteractEvent?.Invoke();
        }
    }
}