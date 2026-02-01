using UnityEngine;

namespace GGJ2026.GamePlay
{
    public class EnemyTag : MonoBehaviour
    {
        public IEntityController entityController;

        private void Awake()
        {
            entityController = GetComponentInParent<IEntityController>();
        }
    }
}