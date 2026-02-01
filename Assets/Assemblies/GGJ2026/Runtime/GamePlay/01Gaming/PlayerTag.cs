using System;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    public class PlayerTag : MonoBehaviour
    {
        public IEntityController entityController;
        private void Awake()
        {
            entityController = GetComponent<IEntityController>();
        }
    }
}