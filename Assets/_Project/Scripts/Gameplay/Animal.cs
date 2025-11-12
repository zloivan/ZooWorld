using System;
using System.Runtime.CompilerServices;
using _Project.Scripts.Core.Signals;
using DefaultNamespace.Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace DefaultNamespace
{
    public enum AnimalType
    {
        Prey,
        Predator
    }

    public enum MovementType
    {
        Jump,
        Linear,
        StopAndGo
    }


    [RequireComponent(typeof(CollisionDetector), typeof(BoundaryMonitor))] [SelectionBase]
    public class Animal : MonoBehaviour
    {
        private AnimalType _animalType;
        private AnimalConfigSO _config;
        private Vector3 _currentDirection;
        private bool _isProcessingThisFrame;

        private IMovementBehavior _movementBehavior;
        private CollisionDetector _collisionDetector;
        private BoundaryMonitor _boundaryMonitor;
        private ObjectPool<Animal> _sourcePool;
        private IAnimalSignals _signals;

        private void Awake()
        {
            _collisionDetector = gameObject.GetComponent<CollisionDetector>();
            _boundaryMonitor = gameObject.GetComponent<BoundaryMonitor>();
        }

        private void OnEnable()
        {
            const float ANIMATION_APPEAR_DURATION = .7f;
            transform.DOScale(Vector3.one, ANIMATION_APPEAR_DURATION).From(Vector3.zero).SetEase(Ease.OutBounce);
        }

        private void OnDisable() =>
            transform.DOKill();

        public void Initialize(AnimalConfigSO animalConfigSO, IMovementBehavior movement, IAnimalSignals signals)
        {
            _config = animalConfigSO;
            _animalType = animalConfigSO.AnimalType;
            _movementBehavior = movement;
            transform.rotation = Quaternion.LookRotation(_currentDirection);
            _collisionDetector.Initialize(this, _config.AnimalRadius);
            _signals = signals;
        }

        private void Update()
        {
            _movementBehavior?.Move();
            HandleRotation();
        }

        private void HandleRotation()
        {
            var targetRotation = Quaternion.LookRotation(_currentDirection);

            const float ROTATION_SPEED = 5f;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ROTATION_SPEED);
        }

        public void Die()
        {
            if (_sourcePool != null)
            {
                _sourcePool.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }

            _signals.OnAnimalDied?.Invoke(this);
        }

        public void SetMoveDirection(Vector3 getMoveDirection) =>
            _currentDirection = getMoveDirection;

        public Vector3 GetMoveDirection() =>
            _currentDirection;

        public AnimalConfigSO GetConfig() =>
            _config;

        public bool GetIfProcessingThisFrame() =>
            _isProcessingThisFrame;

        public void SetIfProcessingThisFrame(bool isProcessing) =>
            _isProcessingThisFrame = isProcessing;

        public bool IsPrey() =>
            _animalType == AnimalType.Prey;

        public float GetCurrentVelocityMagnitude() =>
            _movementBehavior.GetVelocityMagnitude();

        public CollisionDetector GetCollisionDetector() =>
            _collisionDetector;

        public IMovementBehavior GetMovementBehavior() =>
            _movementBehavior;

        public void NotifyAnimalEat(Animal victim) =>
            _signals?.OnAnimalEaten?.Invoke(new AnimalEatEventArgs(this, victim));

        public BoundaryMonitor GetBoundaryMonitor() =>
            _boundaryMonitor;

        public void SetSourcePool(ObjectPool<Animal> sourcePool) =>
            _sourcePool = sourcePool;
    }
}