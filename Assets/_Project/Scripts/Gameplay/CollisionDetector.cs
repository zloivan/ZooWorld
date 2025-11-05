using System;
using _Project.Scripts.Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class CollisionDetector : MonoBehaviour
    {
        private Animal _animal;
        private IMovementBehavior _movementBehavior;
        private float _collisionRadius;

        public void Initialize(Animal animal, IMovementBehavior movement, float radius)
        {
            _animal = animal;
            _movementBehavior = movement;
            _collisionRadius = radius;
        }

        public void FixedUpdate()
        {
            if (!_movementBehavior.CheckIfCanCollide())
                return;

            //TODO: Probably will need optimization
            var colliders = Physics.OverlapSphere(transform.position, _collisionRadius);

            foreach (var col in colliders)
            {
                //
                // if (col.gameObject == gameObject) continue;
                // {
                //     
                // }

                var otherAnimal = col.GetComponent<Animal>();
                
                if (otherAnimal != null)
                {
                    CollisionManager.ProcessCollision(_animal, otherAnimal);
                }
            }
        }
    }
}