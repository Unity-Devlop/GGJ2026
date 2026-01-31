using System;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(EntityPropertyShower))]
    public class EnemyController : MonoBehaviour
    {
        public EnemyData enemyData { get; private set; }
        private EntityPropertyShower _propertyShower;

        private void Awake()
        {
            _propertyShower = GetComponent<EntityPropertyShower>();
        }

        public void Bind(EnemyData enemyData)
        {
            this.enemyData = enemyData;
            _propertyShower.Bind(this.enemyData.propertyData);
        }

        public bool IsDead()
        {
            return enemyData.propertyData.health.Value <= 0;
        }

        public void UnBind()
        {
            enemyData = null;
            _propertyShower.UnBind();
        }
    }
}