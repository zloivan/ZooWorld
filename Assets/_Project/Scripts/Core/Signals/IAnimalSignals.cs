using System;
using DefaultNamespace;
using UnityEngine.Events;

namespace _Project.Scripts.Core.Signals
{
    public class AnimalEatEventArgs : EventArgs
    {
        public Animal Attacker { get; }
        public Animal Victim { get; }

        public AnimalEatEventArgs(Animal attacker, Animal victim)
        {
            Attacker = attacker;
            Victim = victim;
        }
    }
    
    public interface IAnimalSignals
    {
        UnityEvent<Animal> OnAnimalDied { get; }
        UnityEvent<AnimalEatEventArgs> OnAnimalEaten{ get; }
    }
}