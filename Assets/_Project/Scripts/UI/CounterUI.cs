using System;
using DefaultNamespace;
using DG.Tweening;
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

            const float ANIMATION_PUNCH_STRENGTH = 0.2f;
            const float ANIMATION_DURATION = 0.3f;
            const int ANIMATION_VIBRATO = 5;
            const float ANIMATION_ELASTICITY = 0.5f;

            if (isPrey)
            {
                _preyCounter++;

                _preyCounterLabel.transform.DOComplete();
                _preyCounterLabel.transform.DOPunchScale(Vector3.one * ANIMATION_PUNCH_STRENGTH,
                    ANIMATION_DURATION, ANIMATION_VIBRATO, ANIMATION_ELASTICITY);
            }
            else
            {
                _predatorCounter++;
                _predatorsCounterLabel.transform.DOComplete();
                _predatorsCounterLabel.transform.DOPunchScale(Vector3.one * ANIMATION_PUNCH_STRENGTH,
                    ANIMATION_DURATION, ANIMATION_VIBRATO, ANIMATION_ELASTICITY);
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