using UnityEngine;

namespace DefaultNamespace
{
    public class BoundaryMonitor : MonoBehaviour
    {
        private Camera _mainCamera;
        private bool _needsRedirect;

        //Could implement some camera provider, but at this scope works perfectly fine
        private void Awake() =>
            _mainCamera = Camera.main;

        private void Update()
        {
            var viewPortPosition = _mainCamera.WorldToViewportPoint(transform.position);

            if (viewPortPosition.x is < 0f or > 1f ||
                viewPortPosition.y is < 0f or > 1f)
                _needsRedirect = true;
            else
                _needsRedirect = false;
        }
        
        public bool GetNeedsRedirect() => 
            _needsRedirect;
        
        public Vector3 GetCenterDirection() =>
            (Vector3.zero - transform.position).normalized;
    }
}