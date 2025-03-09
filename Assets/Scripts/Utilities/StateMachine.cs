using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Content.Scripts.Utilities
{
    public class StateMachine : MonoBehaviour
    {
        private State currentState;
        public State CurrentState => currentState;

        [SerializeField] private State startingState;
        
        [SerializeField] private GameObject statesRoot;
        
        private Dictionary<string, State> states = new Dictionary<string, State>();
        
        public string currentStateName = "";

        [SerializeField] private TextMeshPro debugText; 
        
        private void Awake()
        {
            for (int i = 0; i < statesRoot.transform.childCount; i++){
                Transform childState = statesRoot.transform.GetChild(i);
                State stateComponent = childState.GetComponent<State>();
                // Debug.Log($"{childState.gameObject.name} | {stateComponent}");
                stateComponent.transitionRequest = EnterState;
                states.Add(childState.name, stateComponent);
            }
            
            EnterState(startingState);
        }

        public void EnterState(State nextState)
        {
            currentState?.OnExit();
            currentState = nextState;
            debugText.text = nextState.name;
            currentState?.OnEnter();
            currentStateName = currentState.gameObject.name;
        }

        public void EnterState(string nextStateName)
        {
            State nextState;
            if (states.TryGetValue(nextStateName, out nextState))
                EnterState(nextState);
            else
                Debug.LogError($"Could not find State with name {nextStateName}");
        }

        void Update()
        {
            currentState?.UpdateTick();
        }

        private void FixedUpdate()
        {
            currentState?.FixedUpdateTick();
        }
    }
}
