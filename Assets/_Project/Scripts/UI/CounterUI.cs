using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class CounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _predatorsCounterLabel;
        [SerializeField] private TextMeshProUGUI _preyCounterLabel;

        private int _preyCounter;
        private int _predatorCounter;
        
        private void Awake()
        {
            UpdateUI();
            Animal.OnDied += HandleAnimalOnDied;
        }

        private void HandleAnimalOnDied(object sender, EventArgs e)
        {
           var isPrey = ((Animal)sender).IsPrey();

           if (isPrey)
           {
               _preyCounter++;
           }
           else
           {
               _predatorCounter++;
           }

           UpdateUI();
        }

        private void UpdateUI()
        {
            _predatorsCounterLabel.text = "Predators eaten: " + _predatorCounter;
            _preyCounterLabel.text = "Prey eaten: " + _preyCounter;
        }
    }
}