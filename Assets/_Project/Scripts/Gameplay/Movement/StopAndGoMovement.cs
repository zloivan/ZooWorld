using UnityEngine;

namespace DefaultNamespace
{
    public class StopAndGoMovement : BaseAnimalMovement
    {
        private readonly float _speed;
        private float _stateTimer;
        private bool _isMoving;


        private const float MOVE_DURATION = 1.0f;
        private const float STOP_DURATION = 1.5f;

        public StopAndGoMovement(Animal animal, float speed)
        {
            Animal = animal;
            CollisionDetector = Animal.GetCollisionDetector();
            
            _speed = speed;
            _stateTimer = 0f;
            _isMoving = true;
        }

        public override void Move()
        {
            _stateTimer += Time.deltaTime;

            if (_isMoving && _stateTimer >= MOVE_DURATION)
            {
                _isMoving = false;
                _stateTimer = 0f;
                return;
            }
            else if (!_isMoving && _stateTimer >= STOP_DURATION)
            {
                _isMoving = true;
                _stateTimer = 0f;
                RandomlyRotateDirection();
            }

            if (!_isMoving)
            {
                return;
            }

            var velMagnitude = GetVelocityMagnitude();
            if (Animal.GetBoundaryMonitor().GetNeedsRedirect())
            {
                Animal.SetMoveDirection(Animal.GetBoundaryMonitor().GetCenterDirection());
            }
            else
            {
                if (!CollisionDetector.IsPathClear(Animal.GetMoveDirection(), velMagnitude * 2f))
                {
                    if (CollisionDetector.TryFindFreeDirection(out var freeDirection, velMagnitude * 2f))
                    {
                        Animal.SetMoveDirection(freeDirection);
                    }
                    else
                    {
                        RandomlyRotateDirection();
                    }
                }
            }

            Animal.transform.position += velMagnitude * Animal.GetMoveDirection();
        }

        public override float GetVelocityMagnitude() =>
            _isMoving ? _speed * Time.deltaTime : 0f;

        public override void OnInterrupted()
        {
            _isMoving = true;
            _stateTimer = 0f;
        }
    }
}