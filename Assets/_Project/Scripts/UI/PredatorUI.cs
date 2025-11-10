using _Project.Scripts.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class PredatorUI : MonoBehaviour
    {
        [SerializeField] private GameObject _tastyLabelPrefab;
        
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
            Animal.OnEat += Animal_OnEat;
        }

        private void OnDestroy() =>
            Animal.OnEat -= Animal_OnEat;

        private void Animal_OnEat(object sender, Animal.AnimalEatEventArgs e)
        {
            var labelObject = Instantiate(_tastyLabelPrefab, transform);

            labelObject.SetActive(true);

            if (!labelObject.TryGetComponent<TastyLabel>(out var tastyLabelComponent)) 
                return;
            
            tastyLabelComponent.Initialize(e.Attacker.transform, _camera);
        }
    }
}