using System;
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
        Liear
    }


    public class Animal : MonoBehaviour
    {
        private AnimalType _animalType;
        private AnimalConfigSO _config;
        [SerializeField] private Vector3 _currentDirection;

        private IMovementBehavior _movementBehavior;


        public void Initialize(AnimalConfigSO animalConfigSO, IMovementBehavior movement)
        {
            _config = animalConfigSO;
            _animalType = animalConfigSO.AnimalType;
            _movementBehavior = movement;
            
        }
        private void Update()
        {
            _movementBehavior?.Move();
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public void SetMoveDirection(Vector3 getMoveDirection) =>
            _currentDirection = getMoveDirection;

        public Vector3 GetMoveDirection() =>
            _currentDirection;
    }
}