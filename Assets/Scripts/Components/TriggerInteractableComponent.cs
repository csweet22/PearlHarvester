using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Content.Scripts.Utilities;
using UnityEngine.Events;

namespace Content.Scripts.Components
{
    public class TriggerInteractableComponent : TriggerComponent
    {
        [SerializeField]
        private InteractableComponent interactableComponent;

        [SerializeField]
        private bool interactStartOnEnter;
        [SerializeField]
        private bool interactEndOnEnter;
        [SerializeField]
        private bool interactStartOnExit;
        [SerializeField]
        private bool interactEndOnExit;

        protected override void OnTriggerEnter(Collider other)
        {
            InteractorComponent interactor = null;
            if (other.gameObject.TryGetComponent<InteractorComponent>(out InteractorComponent _interactor))
                interactor = _interactor;
            if (interactor == null)
                return;
            
            base.OnTriggerEnter(other);
            if (interactStartOnEnter)
                interactableComponent.OnInteractStart(interactor);
            if (interactEndOnEnter)
                interactableComponent.OnInteractEnd(interactor);
        }

        protected override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
            InteractorComponent interactor = null;
            if (other.gameObject.TryGetComponent<InteractorComponent>(out InteractorComponent _interactor))
                interactor = _interactor;
            if (interactor == null)
                return;

            if (!interactor.canInteract)
                return;
            
            // Stupid solution
            if (interactStartOnEnter)
                interactableComponent.OnInteractStart(interactor);
            if (interactEndOnEnter)
                interactableComponent.OnInteractEnd(interactor);
            
            interactor.DeactivateInteractable();
        }

        protected override void OnTriggerExit(Collider other)
        {
            InteractorComponent interactor = null;
            if (other.gameObject.TryGetComponent<InteractorComponent>(out InteractorComponent _interactor))
                interactor = _interactor;
            if (interactor == null)
                return;
            
            base.OnTriggerExit(other);
            if (interactStartOnExit)
                interactableComponent.OnInteractStart(interactor);
            if (interactEndOnExit)
                interactableComponent.OnInteractEnd(interactor);
        }
    }
}