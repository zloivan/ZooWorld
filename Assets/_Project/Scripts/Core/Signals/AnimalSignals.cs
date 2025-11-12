using UnityEngine;
using UnityEngine.Events;
using ZooWorld.Gameplay;

namespace ZooWorld.Core.Signals
{
    public class AnimalSignals : MonoBehaviour, IAnimalSignals
    {
        public UnityEvent<Animal> OnAnimalDied { get; } = new();

        public UnityEvent<AnimalEatEventArgs> OnAnimalEaten { get; } = new();

        private void OnDestroy()
        {
            OnAnimalDied.RemoveAllListeners();
            OnAnimalEaten.RemoveAllListeners();
        }
    }
}