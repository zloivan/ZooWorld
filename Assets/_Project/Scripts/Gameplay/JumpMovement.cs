using UnityEngine;

namespace DefaultNamespace
{
    public class JumpMovement : BaseAnimalMovement
    {
        private readonly Animal _animal;
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
            _animal = animal;
            _jumpDistance = jumpDistance;
        }


        public override void Move()
        {
            if (_isJumping)
            {
                _jumpTimer += Time.deltaTime;
                var progress = _jumpTimer / JUMP_DURATION;

                _animal.transform.position =
                    Vector3.Lerp(_startPosition, _targetPosition, progress);


                if (progress >= 1f)
                {
                    _isJumping = false;
                    _speed = 0f;
                    _cooldownTimer = 0f;
                }
            }
            else
            {
                _cooldownTimer += Time.deltaTime;

                Debug.Log(_animal.name + _cooldownTimer);
                if (_cooldownTimer >= JUMP_COOLDOWN)
                {
                    StartJump();
                }
            }
        }

        private void StartJump()
        {
            _isJumping = true;
            _jumpTimer = 0f;
            _startPosition = _animal.transform.position;
            _targetPosition = _startPosition + _animal.GetMoveDirection() * _jumpDistance;
            _speed = _jumpDistance / JUMP_DURATION;
        }

        public override bool CheckIfCanCollide() =>
            _isJumping;

        public override float GetVelocityMagnitude() =>
            _speed * Time.deltaTime;
    }
}