using System;
using System.Runtime.CompilerServices;
using DefaultNamespace.Configs;
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


    [RequireComponent(typeof(CollisionDetector), typeof(BoundaryMonitor))]
    public class Animal : MonoBehaviour
    {
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
        }

        public void Initialize(AnimalConfigSO animalConfigSO, IMovementBehavior movement)
        {
            _config = animalConfigSO;
            _animalType = animalConfigSO.AnimalType;
            _movementBehavior = movement;

            _collisionDetector.Initialize(this, _config.AnimalRadius);
            _boundaryMonitor.Initialize(this, _movementBehavior);
        }

        private void Update() =>
            _movementBehavior?.Move();

        public void Die() =>
            Destroy(gameObject);

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

        public float GetCurrentVelocity() =>
            _movementBehavior.GetVelocityMagnitude();

        public CollisionDetector GetCollisionDetector() =>
            _collisionDetector;

        public IMovementBehavior GetMovementBehavior() =>
            _movementBehavior;
    }
}