using System;
using UnityEngine;


namespace Scripts.Utilities
{
    
    public class Timer : MonoBehaviour
    {

        [SerializeField] private float waitTime = 1.0f;
        [SerializeField] private bool oneShot = false;
        [SerializeField] private bool autoStart = true;

        public Action OnTimeOut;
        
        private float _currentTime = 0.0f;
        private bool _paused = true;

        void Start()
        {
            if (autoStart)
                StartTimer();
        }
        
        void Update()
        {
            if (_currentTime > waitTime){
                OnTimeOut.Invoke();
                _currentTime = 0.0f;
                if (oneShot)
                    StopTimer();
            }
            
            if (_paused)
                return;

            _currentTime += Time.deltaTime;
        }

        public void StartTimer(float time = 0.0f)
        {
            _currentTime = time;
            UnpauseTimer();
        }

        public void StopTimer()
        {
            _currentTime = 0.0f;
            PauseTimer();
        }
        
        public void PauseTimer()
        {
            _paused = true;
        }
        
        public void UnpauseTimer()
        {
            _paused = false;
        }

    }
   
}