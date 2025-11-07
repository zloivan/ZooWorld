using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BaseAnimalMovement : IMovementBehavior
    {
        protected Animal Animal;
        protected CollisionDetector CollisionDetector;

        public abstract void Move();

        public abstract float GetVelocityMagnitude();

        public virtual void ReverseDirection()
        {
            //To avoid getting stuck in corners, we add a slight random sideways movement when reversing direction
            var reversed = Animal.GetMoveDirection() * -1;
            var sideways = new Vector3(-reversed.z, 0, reversed.x);
            var sideShift = Random.value < 0.5f ? sideways : -sideways;

            const float TURN_DELTA = 0.3f;

            Animal.SetMoveDirection((reversed + sideShift * TURN_DELTA).normalized);
        }

        public virtual void RandomlyRotateDirection()
        {
            Animal.SetMoveDirection(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized);
        }

        public virtual void OnInterrupted()
        {
            //Default implementation does nothing
        }
    }
}