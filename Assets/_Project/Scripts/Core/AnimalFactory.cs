using UnityEngine;
using ZooWorld.Configs;
using ZooWorld.Core.Signals;
using ZooWorld.Gameplay;
using ZooWorld.Gameplay.Movement;
using Object = UnityEngine.Object;

namespace ZooWorld.Core
{
    public static class AnimalFactory
    {
        public static Animal CreateAnimal(AnimalConfigSO config, Vector3 position, IAnimalSignals signals)
        {
            var go = Object.Instantiate(config.Prefab, position, Quaternion.identity);
            var animal = go.GetComponent<Animal>();

            IMovementBehavior movementBehavior = null;

            switch (config.MovementType)
            {
                case MovementType.Jump:
                    movementBehavior = new JumpMovement(animal, config.JumpDistance);
                    movementBehavior.RandomlyRotateDirection();
                    break;
                case MovementType.Linear:
                    movementBehavior = new LinearMovement(animal, config.Speed);
                    movementBehavior.RandomlyRotateDirection();
                    break;
                case MovementType.StopAndGo:
                    movementBehavior = new StopAndGoMovement(animal, config.Speed);
                    movementBehavior.RandomlyRotateDirection();
                    break;
                default:
                    Debug.LogError("No Movement Type defined in AnimalFactory");
                    break;
            }


            animal.Initialize(config, movementBehavior, signals);


            return animal;
        }
    }
}