using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(fileName = "AnimalConfigSO", menuName = "Configs/AnimalConfigSO", order = 0)]
    public class AnimalConfigSO : ScriptableObject
    {
        public AnimalType AnimalType;
        public MovementType MovementType;
        public float Speed;
        public float JumpDistance;
        public GameObject Prefab;
    }
}