using UnityEngine;
using ZooWorld.Core.Signals;

namespace ZooWorld.UI
{
    public class PredatorUI : MonoBehaviour
    {
        [SerializeField] private GameObject _tastyLabelPrefab;
        [SerializeField] private AnimalSignals _animalSignals;
        
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
            _animalSignals.OnAnimalEaten.AddListener(Animal_OnEaten);
        }

        private void OnDestroy() =>
            _animalSignals.OnAnimalEaten.RemoveListener(Animal_OnEaten);

        private void Animal_OnEaten(AnimalEatEventArgs e)
        {
            var labelObject = Instantiate(_tastyLabelPrefab, transform);

            labelObject.SetActive(true);

            if (!labelObject.TryGetComponent<TastyLabel>(out var tastyLabelComponent)) 
                return;
            
            tastyLabelComponent.Initialize(e.Attacker.transform, _camera);
        }
    }
}