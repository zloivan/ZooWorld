using DefaultNamespace;
using DefaultNamespace.Configs;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core
{
    public class AnimalFactory
    {
        public static Animal CreateAnimal(AnimalConfigSO config, Vector3 position)
        {
            var go = Object.Instantiate(config.Prefab, position, Quaternion.identity);
            var animal = go.GetComponent<Animal>();

            IMovementBehavior movementBehavior = null;

            switch (config.MovementType)
            {
                case MovementType.Jump:
                    //TODO: Implement Jump Movement
                    break;
                case MovementType.Liear:
                    movementBehavior = new LinearMovement(animal, config.Speed);

                    break;
                default:
                    Debug.LogError("No Movement Type defined in AnimalFactory");
                    break;
            }


            var randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            animal.SetMoveDirection(randomDirection);
            animal.Initialize(config, movementBehavior);

            return animal;
        }
    }
}