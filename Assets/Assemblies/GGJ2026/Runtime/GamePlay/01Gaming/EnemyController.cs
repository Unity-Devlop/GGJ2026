using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(EntityPropertyShower))]
    public class EnemyController : MonoBehaviour, IEntityController
    {
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public EnemyData enemyData { get; private set; }
        private EntityPropertyShower _propertyShower;
        private DoTweenHitEffect _doTweenHitEffect;
        private EnemyIntentVisual _enemyIntent;
        public bool wantedOperation;


        private void Awake()
        {
            _propertyShower = GetComponent<EntityPropertyShower>();
            _doTweenHitEffect = GetComponentInChildren<DoTweenHitEffect>();
            _enemyIntent = GetComponentInChildren<EnemyIntentVisual>();
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

        public async UniTask StartThinking()
        {
            wantedOperation = false;
        }

        public async Task<IOperation> GetNextOperation()
        {
            throw new NotImplementedException();
        }

        public UniTask UseCard(CardData cardData)
        {
            throw new NotImplementedException();
        }

        public async UniTask TakeCard(CardData cardData)
        {
        }

        public async UniTask TakeDamage(int damageValue)
        {
            enemyData.propertyData.health.Value -= damageValue;
            // DOTween
            await _doTweenHitEffect.PlayHitEffect();
        }

        public UniTask GainShield(int value)
        {
            throw new NotImplementedException();
        }
    }
}