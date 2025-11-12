using System;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class TastyLabel : MonoBehaviour
    {
        [SerializeField] private float _lifetime;
        [SerializeField] private Vector3 _worldOffset = new(0f, 0f, -1.8f);
        [SerializeField] private CanvasGroup _canvasGroup;

        private Transform _targetTransform;
        private Camera _camera;
        private float _timer;
        private RectTransform _rectTransform;
        private Sequence _animationSequence;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();


            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
        }

        private void OnDestroy()
        {
            if (_animationSequence != null && _animationSequence.IsActive())
            {
                _animationSequence.Kill();
            }
        }

        public void Initialize(Transform target, Camera cam)
        {
            _targetTransform = target;
            _camera = cam;

            UpdatePosition();

            _animationSequence = DOTween.Sequence();
            const float ANIMATION_DURATION = 0.2f;

            _animationSequence.Append(_canvasGroup.DOFade(1f, ANIMATION_DURATION).From(0f));
            _animationSequence.Join(_rectTransform.DOScale(Vector3.one, ANIMATION_DURATION)
                .From(Vector3.one * 0.5f)
                .SetEase(Ease.OutBack));
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _lifetime)
            {
                if (_animationSequence != null && _animationSequence.IsActive())
                {
                    _animationSequence.Kill();
                }

                _canvasGroup.DOFade(0f, 0.3f).OnComplete(() => Destroy(gameObject));
                enabled = false;
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