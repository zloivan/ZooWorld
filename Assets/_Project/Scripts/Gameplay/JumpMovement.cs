using UnityEngine;

namespace DefaultNamespace
{
    public class JumpMovement : BaseAnimalMovement
    {
        private readonly float _jumpDistance;

        private const float JUMP_DURATION = 0.5f;
        private const float JUMP_COOLDOWN = 2f;


        private float _jumpTimer;
        private bool _isJumping;
        private float _speed;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private float _cooldownTimer;

        public JumpMovement(Animal animal, float jumpDistance)
        {
            Animal = animal;
            CollisionDetector = Animal.GetCollisionDetector();
            _jumpDistance = jumpDistance;
        }

        public override void Move()
        {
            if (_isJumping)
            {
                PerformJump();
            }
            else
            {
                _cooldownTimer += Time.deltaTime;

                if (_cooldownTimer >= JUMP_COOLDOWN)
                {
                    TryStartJump();
                }
            }
        }

        private void PerformJump()
        {
            _jumpTimer += Time.deltaTime;
            var progress = _jumpTimer / JUMP_DURATION;

            var nextPosition = Vector3.Lerp(_startPosition, _targetPosition, progress);
            var direction = (nextPosition - Animal.transform.position).normalized;
            var distance = Vector3.Distance(Animal.transform.position, nextPosition);

            if (!CollisionDetector.IsPathClear(direction, distance))
            {
                _isJumping = false;
                _speed = 0f;
                _cooldownTimer = 0f;

                if (CollisionDetector.TryFindFreeDirection(out var freeDirection, _jumpDistance))
                {
                    Animal.SetMoveDirection(freeDirection);
                }
                
                return;
            }

            Animal.transform.position = nextPosition;

            if (progress < 1f)
                return;
            
            _isJumping = false;
            _speed = 0f;
            _cooldownTimer = 0f;
        }

        private bool TryStartJump()
        {
            if (!CollisionDetector.IsPathClear(Animal.GetMoveDirection(), _jumpDistance))
            {
                if (CollisionDetector.TryFindFreeDirection(out var freeDirection, _jumpDistance))
                {
                    Animal.SetMoveDirection(freeDirection);
                }
                else
                {
                    _cooldownTimer = 0f;
                    return false;
                }
            }

            _isJumping = true;
            _jumpTimer = 0f;
            _startPosition = Animal.transform.position;
            RandomlyRotateDirection();
            _targetPosition = _startPosition + Animal.GetMoveDirection() * _jumpDistance;
            _speed = _jumpDistance / JUMP_DURATION;
            
            return true;
        }

        public override float GetVelocityMagnitude() =>
            _speed * Time.deltaTime;

        public override void OnInterrupted()
        {
            _isJumping = false;
            _speed = 0f;
            _cooldownTimer = 0f;
            _jumpTimer = 0f;
        }
    }
}