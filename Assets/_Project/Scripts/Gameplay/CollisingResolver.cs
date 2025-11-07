using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class CollisingResolver
    {
        private class CollisionPair
        {
            private readonly AnimalType _animalTypeA;
            private readonly AnimalType _animalTypeB;

            public CollisionPair(AnimalType a, AnimalType b)
            {
                _animalTypeA = a;
                _animalTypeB = b;
            }

            public override bool Equals(object obj)
            {
                if (obj is not CollisionPair other)
                    return false;

                return _animalTypeA == other._animalTypeA && _animalTypeB == other._animalTypeB;
            }

            public override int GetHashCode() =>
                ((int)_animalTypeA * 397) ^ (int)_animalTypeB;
        }

        private static Dictionary<CollisionPair, ICollisionRule> _collisionRules;

        public static void Initialize()
        {
            _collisionRules = new Dictionary<CollisionPair, ICollisionRule>
            {
                { new CollisionPair(AnimalType.Prey, AnimalType.Prey), new PreyPreyRule() },
                { new CollisionPair(AnimalType.Predator, AnimalType.Prey), new PredatorPreyRule() },
                { new CollisionPair(AnimalType.Prey, AnimalType.Predator), new PredatorPreyRule() },
                { new CollisionPair(AnimalType.Predator, AnimalType.Predator), new PredatorPredatorRule() },
            };
        }

        public static void Resolve(Animal a, Animal b)
        {
            var key = new CollisionPair(a.GetConfig().AnimalType, b.GetConfig().AnimalType);

            if (!_collisionRules.TryGetValue(key, out var rule))
            {
                Debug.LogError("No collision rule found for the given animal types.");
                return;
            }

            rule.Resolve(a, b);
        }
    }
}