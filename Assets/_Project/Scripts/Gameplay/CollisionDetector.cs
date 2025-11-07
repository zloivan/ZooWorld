using UnityEngine;

namespace DefaultNamespace
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactCollisions;
        [SerializeField] private LayerMask _obstacleCollisions;

        private Animal _animal;
        private float _detectionRadius;

        public void Initialize(Animal animal, float collisionSize)
        {
            _animal = animal;
            _detectionRadius = collisionSize;
        }

        public void FixedUpdate() =>
            ProcessInteractCollisions();

        public bool IsPathClear(Vector3 direction, float distance) =>
            !Physics.BoxCast(transform.position,
                Vector3.one * _detectionRadius,
                direction, out _,
                Quaternion.identity,
                distance,
                _obstacleCollisions);

        public bool TryFindFreeDirection(out Vector3 freeDirection, float checkDistance)
        {
            Vector3[] directions =
            {
                Vector3.forward,
                Vector3.right,
                Vector3.back,
                Vector3.left,
            };

            foreach (var dir in directions)
            {
                if (!IsPathClear(dir, checkDistance))
                    continue;

                freeDirection = dir;
                return true;
            }

            freeDirection = Vector3.zero;
            return false;
        }

        private void ProcessInteractCollisions()
        {
            //if happens before initialization
            if (_animal == null)
                return;

            _animal.SetIfProcessingThisFrame(false);


            //TODO: MAGIC NUMBER
            if (!Physics.BoxCast(transform.position, Vector3.one * _detectionRadius, _animal.GetMoveDirection(),
                    out var hitInfo,
                    Quaternion.identity, _animal.GetCurrentVelocity() * 1.2f, _interactCollisions))
                return;

            var otherAnimal = hitInfo.collider.GetComponent<Animal>();

            //SHOULD NEVER HAPPEN
            Debug.Assert(otherAnimal != null, "No animal component found on collider");

            if (_animal.GetIfProcessingThisFrame() || otherAnimal.GetIfProcessingThisFrame())
                return;

            _animal.SetIfProcessingThisFrame(true);
            otherAnimal.SetIfProcessingThisFrame(true);


            CollisingResolver.Resolve(_animal, otherAnimal);
        }
    }
}