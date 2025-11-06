using System;
using _Project.Scripts.Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionLayers;

        private Animal _animal;
        private IMovementBehavior _movementBehavior;

        public void Initialize(Animal animal, IMovementBehavior movement)
        {
            _animal = animal;
            _movementBehavior = movement;
        }

        public void FixedUpdate()
        {
            //if happens before initialization
            if (_animal == null)
                return;

            _animal.SetIfProcessingThisFrame(false);

            if (!_movementBehavior.CheckIfCanCollide())
                return;

            if (!Physics.BoxCast(transform.position, new Vector3(0.5f, .5f, 0.5f), _animal.GetMoveDirection(),
                    out var hitInfo,
                    Quaternion.identity, .2f, _collisionLayers))
            {
                //Debug.Log("GOT HIT");
                return;
            }
            
            var otherAnimal = hitInfo.collider.GetComponent<Animal>();

            //SHOULD NEVER HAPPEN
            Debug.Assert(otherAnimal != null, "No animal component found on collider");

            if (_animal.GetIfProcessingThisFrame() || otherAnimal.GetIfProcessingThisFrame())
                return;

            _animal.SetIfProcessingThisFrame(true);
            otherAnimal.SetIfProcessingThisFrame(true);

            //BOTH PREY
            if (_animal.IsPrey() && otherAnimal.IsPrey())
            {
                //Both are prey - toss both from each other
                var direction = (otherAnimal.transform.position - _animal.transform.position).normalized;
                const float bounceBackDistance = 1f;

                _animal.transform.position -= direction * bounceBackDistance;
                otherAnimal.transform.position += direction * bounceBackDistance;

                _animal.SetMoveDirection(-direction);
                otherAnimal.SetMoveDirection(direction);
            }
            else
            {
                //ONE PREDATOR, ONE PREY
                if (_animal.IsPrey() && !otherAnimal.IsPrey()
                    || !_animal.IsPrey() && otherAnimal.IsPrey())
                {
                    //Prey dies
                    if (_animal.IsPrey())
                        _animal.Die();
                    else
                        otherAnimal.Die();

                    //TODO: Trigger prey death in UI
                }
                else
                {
                    //BOTH PREDATORS
                    if (!_animal.IsPrey() && !otherAnimal.IsPrey())
                    {
                        //Random predator dies

                        if (UnityEngine.Random.value < 0.5f)
                            _animal.Die();
                        else
                            otherAnimal.Die();
                    }
                }
            }
        }
    }
}