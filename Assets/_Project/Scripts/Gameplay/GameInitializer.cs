using UnityEngine;
using ZooWorld.Gameplay.Collisions;

namespace ZooWorld.Gameplay
{
    public class GameInitializer : MonoBehaviour
    {
        //TODO: move to initial scene or use DI framework
        private void Awake()
        {
            CollisingResolver.Initialize();
        }
    }
}