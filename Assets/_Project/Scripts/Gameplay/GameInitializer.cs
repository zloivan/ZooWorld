using System;
using UnityEngine;

namespace DefaultNamespace
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