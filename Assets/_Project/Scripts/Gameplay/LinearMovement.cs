using UnityEngine;

namespace DefaultNamespace
{
    public class LinearMovement : BaseAnimalMovement
    {
        private readonly float _oneDirectionMovementDuration;
        private float _oneWayMovementTimer;
        private readonly float _speed;

        public LinearMovement(Animal animal, float speed)
        {
            Animal = animal;
            CollisionDetector = Animal.GetCollisionDetector();
            _speed = speed;

            const float MIN_ONE_WAY_MOVING_DURATION = 1.5f;
            const float MAX_ONE_WAY_MOVING_DURATION = 2f;

            _oneDirectionMovementDuration = Random.Range(MIN_ONE_WAY_MOVING_DURATION, MAX_ONE_WAY_MOVING_DURATION);
        }

        public override void Move()
        {
            var velMagnitude = GetVelocityMagnitude();

            //No path
            if (!CollisionDetector.IsPathClear(Animal.GetMoveDirection(),
                    //TODO: MAGIC NUMBER
                    velMagnitude * 2f))
            {
                //TODO: MAGIC NUMBER
                if (CollisionDetector.TryFindFreeDirection(out var freeDirection, velMagnitude *
                        2f))

                {
                    Animal.SetMoveDirection(freeDirection);
                }
                else
                {
                    RandomlyRotateDirection();
                }
            }

            Animal.transform.position += velMagnitude * Animal.GetMoveDirection();
            
            _oneWayMovementTimer += Time.deltaTime;
            if (_oneWayMovementTimer < _oneDirectionMovementDuration)
            {
                return;
            }
            
            RandomlyRotateDirection();
            _oneWayMovementTimer = 0f;
        }

        public override float GetVelocityMagnitude() =>
            _speed * Time.deltaTime;
    }
}