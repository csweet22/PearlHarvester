using System;
using System.Collections.Generic;
using UnityEngine;

namespace Content.Scripts.Utilities
{
    public class Detector : MonoBehaviour
    {
        public Action<Collider> entityEntered;
        public Action<Collider> entityExited;

        public List<Collider> colliders = new List<Collider>();
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            colliders.Add(other);
            entityEntered?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log(other.gameObject.name);
            colliders.Remove(other);
            entityExited?.Invoke(other);
        }
    }
}
