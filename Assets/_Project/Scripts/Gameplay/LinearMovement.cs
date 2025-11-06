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
            _speed = speed;

            const float MIN_ONE_WAY_MOVING_DURATION = 1.5f;
            const float MAX_ONE_WAY_MOVING_DURATION = 2f;
            _oneDirectionMovementDuration = Random.Range(MIN_ONE_WAY_MOVING_DURATION, MAX_ONE_WAY_MOVING_DURATION);
        }

        public override void Move()
        {
            Animal.transform.position += GetVelocityMagnitude()
                                          * Animal.GetMoveDirection();

            _oneWayMovementTimer += Time.deltaTime;
            if (_oneWayMovementTimer < _oneDirectionMovementDuration)
                return;

            ReverseDirection();
            _oneWayMovementTimer = 0f;
        }

        public override bool CheckIfCanCollide() =>
            true;

        public override float GetVelocityMagnitude() =>
            _speed * Time.deltaTime;
    }
}