using UnityEngine;

namespace DefaultNamespace
{
    public class LinearMovement : IMovementBehavior
    {
        private readonly Animal _animal;
        private readonly float _speed;
        private const float ONE_DIRECTION_MOVEMENT_DURATION = 2f;

        private float _oneWayMovementTimer;

        public LinearMovement(Animal animal, float speed)
        {
            _animal = animal;
            _speed = speed;
        }

        public void Move()
        {
            _animal.transform.position += _speed
                                          * Time.deltaTime
                                          * _animal.GetMoveDirection();

            _oneWayMovementTimer += Time.deltaTime;
            if (_oneWayMovementTimer < ONE_DIRECTION_MOVEMENT_DURATION)
                return;

            var randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _animal.SetMoveDirection(randomDirection);
            _oneWayMovementTimer = 0f;
        }

        public void ReverseDirection() =>
            _animal.SetMoveDirection(_animal.GetMoveDirection() * -1);

        public bool CheckIfCanCollide() =>
            true;
    }
}