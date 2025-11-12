using System;
using UnityEngine.Events;
using ZooWorld.Gameplay;

namespace ZooWorld.Core.Signals
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