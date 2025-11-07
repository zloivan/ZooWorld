using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PredatorUI : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
            
            Animal.OnEat += HandleAnimalEat;
        }

        private void HandleAnimalEat(object sender, Animal.AnimalEatEventArgs args)
        {
            Debug.Log($@"PredatorUI: {args.Attacker.name} said ""Yammi""");
        }
    }
}