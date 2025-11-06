using UnityEngine;

namespace DefaultNamespace
{
    public class LinearMovement : IMovementBehavior
    {
        private readonly Animal _animal;
        private readonly float _speed;
        private readonly float _oneDirectionMovementDuration;

        private float _oneWayMovementTimer;

        public LinearMovement(Animal animal, float speed)
        {
            _animal = animal;
            _speed = speed;
            
            const float MIN_ONE_WAY_MOVING_DURATION = 1.5f;
            const float MAX_ONE_WAY_MOVING_DURATION = 2f;
            _oneDirectionMovementDuration = Random.Range(MIN_ONE_WAY_MOVING_DURATION, MAX_ONE_WAY_MOVING_DURATION);
        }

        public void Move()
        {
            _animal.transform.position += GetVelocityMagnitude()
                                          * _animal.GetMoveDirection();

            _oneWayMovementTimer += Time.deltaTime;
            if (_oneWayMovementTimer < _oneDirectionMovementDuration)
                return;

            var randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

            _animal.SetMoveDirection(randomDirection);
            _oneWayMovementTimer = 0f;
        }

        public void ReverseDirection()
        {
            //To avoid getting stuck in corners, we add a slight random sideways movement when reversing direction
            var reversed = _animal.GetMoveDirection() * -1;
            var sideways = new Vector3(-reversed.z, 0, reversed.x);
            var sideShift = Random.value < 0.5f ? sideways : -sideways;

            const float TURN_DELTA = 0.3f;
            
            _animal.SetMoveDirection((reversed + sideShift * TURN_DELTA).normalized); 
            _oneWayMovementTimer = 0f;
        }

        public bool CheckIfCanCollide() =>
            true;

        public float GetVelocityMagnitude() =>
            _speed * Time.deltaTime;
    }
}