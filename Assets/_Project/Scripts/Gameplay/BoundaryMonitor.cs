using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BoundaryMonitor : MonoBehaviour
    {
        private Animal _animal;
        private IMovementBehavior _movementBehavior;
        private Camera _mainCamera;

        [SerializeField] private string _Log;
        

        //Could implement some camera provider, but at this scope works perfectly fine
        private void Awake() =>
            _mainCamera = Camera.main;

        public void Initialize(Animal animanl, IMovementBehavior movementBehavior)
        {
            _animal = animanl;
            _movementBehavior = movementBehavior;
        }

        private void Update()
        {
            if (_movementBehavior == null)
                return;
            
            var viewPortPosition = _mainCamera.WorldToViewportPoint(transform.position);

            if (viewPortPosition.x is < 0f or > 1f ||
                viewPortPosition.y is < 0f or > 1f)
            {
                _Log = "REVERSING";
                _movementBehavior.RedirectToCenter();
            }
            else
            {
                _Log = "IDLE";
            }
        }
    }
}