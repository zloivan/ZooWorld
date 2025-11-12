using System;
using System.Runtime.CompilerServices;
using DefaultNamespace.Configs;
using DG.Tweening;
using UnityEngine;

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
        Linear
    }


    [RequireComponent(typeof(CollisionDetector), typeof(BoundaryMonitor))][SelectionBase]
    public class Animal : MonoBehaviour
    {
        public class AnimalEatEventArgs : EventArgs
        {
            public Animal Attacker { get; }
            public Animal Victim { get; }

            public AnimalEatEventArgs(Animal attacker, Animal victim)
            {
                Attacker = attacker;
                Victim = victim;
            }
        }

        public string Log;
        public static event EventHandler<AnimalEatEventArgs> OnEat;
        public static event EventHandler OnDied;

        private AnimalType _animalType;
        private AnimalConfigSO _config;
        private Vector3 _currentDirection;
        private bool _isProcessingThisFrame;

        private IMovementBehavior _movementBehavior;
        private CollisionDetector _collisionDetector;
        private BoundaryMonitor _boundaryMonitor;

        private void Awake()
        {
            _collisionDetector = gameObject.GetComponent<CollisionDetector>();
            _boundaryMonitor = gameObject.GetComponent<BoundaryMonitor>();
            
            const float ANIMATION_APPEAR_DURATION = .7f;

            transform.DOScale(Vector3.one, ANIMATION_APPEAR_DURATION).From(Vector3.zero).SetEase(Ease.OutBounce);
        }

        private void OnDestroy() =>
            transform.DOKill();

        public void Initialize(AnimalConfigSO animalConfigSO, IMovementBehavior movement)
        {
            _config = animalConfigSO;
            _animalType = animalConfigSO.AnimalType;
            _movementBehavior = movement;
            transform.rotation = Quaternion.LookRotation(_currentDirection);
            _collisionDetector.Initialize(this, _config.AnimalRadius);
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
            Destroy(gameObject);
            OnDied?.Invoke(this, EventArgs.Empty);
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
            OnEat?.Invoke(this, new AnimalEatEventArgs(this, victim));
        
        public BoundaryMonitor GetBoundaryMonitor() =>
            _boundaryMonitor;
        
        //DEBUG

        [ContextMenu("Test Text")]
        public void TestText()
        {
            OnEat?.Invoke(this, new AnimalEatEventArgs(this, null));
        }
    }
}