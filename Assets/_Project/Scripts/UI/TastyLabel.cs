using System;
using System.Timers;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class TastyLabel : MonoBehaviour
    {
        [SerializeField] private float _lifetime;
        [SerializeField] private Vector3 _worldOffset;
        
        private Transform _targetTransform;
        private Camera _camera;
        private float _timer;
        private RectTransform _rectTransform;

        private void Awake() =>
            _rectTransform = GetComponent<RectTransform>();

        public void Initialize(Transform target, Camera cam)
        {
            _targetTransform = target;
            _camera = cam;
            UpdatePosition();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _lifetime)
            {
                Destroy(gameObject);
                return;
            }

            if (_targetTransform != null)
            {
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            var worldPosition = _targetTransform.position + _worldOffset;
            _rectTransform.position = _camera.WorldToScreenPoint(worldPosition);
        }
    }
}