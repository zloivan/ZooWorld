using UnityEngine;
using ZooWorld.Gameplay.Collisions;

namespace ZooWorld.Gameplay.Movement
{
    public abstract class BaseAnimalMovement : IMovementBehavior
    {
        protected Animal Animal;
        protected CollisionDetector CollisionDetector;

        public abstract void Move();

        public abstract float GetVelocityMagnitude();

        public virtual void RandomlyRotateDirection()
        {
            Animal.SetMoveDirection(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized);
        }

        public virtual void OnInterrupted()
        {
            //Default implementation does nothing
        }

        public virtual void RedirectToCenter()
        {
            var directionToCenter = (Vector3.zero - Animal.transform.position).normalized;
            Animal.SetMoveDirection(directionToCenter);
        }
    }
}