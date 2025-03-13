using System;
using UnityEngine;
using UnityEngine.Events;

namespace Content.Scripts.Components
{
    public class InteractorComponent : MonoBehaviour
    {
        public UnityEvent startInteractEvent;
        public UnityEvent endInteractEvent;
        public event Action<InteractableComponent> StartInteractAction;
        public event Action<InteractableComponent> EndInteractAction;

        private bool _canInteract = true;
        public bool CanInteract => _canInteract;
        
        public void ActivateInteractable()
        {
            _canInteract = true;
        }

        public void DeactivateInteractable()
        {
            _canInteract = false;
        }

        public virtual void OnInteractStart(InteractableComponent interactableComponent)
        {
            if (!_canInteract)
                return;
            StartInteractAction?.Invoke(interactableComponent);
            startInteractEvent?.Invoke();
        }

        public virtual void OnInteractEnd(InteractableComponent interactableComponent)
        {
            if (!_canInteract)
                return;
            EndInteractAction?.Invoke(interactableComponent);
            endInteractEvent?.Invoke();
        }
    }
}