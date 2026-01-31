using System;
using System.Threading.Tasks;
using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(EntityPropertyShower))]
    public class EnemyController : MonoBehaviour, IEntityController
    {
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public EnemyData data { get; private set; }

        private EntityPropertyShower _propertyShower;
        private DoTweenHitEffect _doTweenHitEffect;
        private EnemyIntentVisual _enemyIntent;

        private int currentOperationIndex = 0;

        private void Awake()
        {
            _propertyShower = GetComponent<EntityPropertyShower>();
            _doTweenHitEffect = GetComponentInChildren<DoTweenHitEffect>();
            _enemyIntent = GetComponentInChildren<EnemyIntentVisual>();
        }


        public void Bind(EnemyData enemyData)
        {
            this.data = enemyData;
            _propertyShower.Bind(this.data.property);
            currentOperationIndex = 0;
            foreach (var cardData in data.candidateCards)
            {
                Debug.Log($"Enemy Candidate Card: {cardData.config.Id}");
            }
        }

        public bool IsDead()
        {
            return data.property.health.Value <= 0;
        }

        public void UnBind()
        {
            data = null;
            _propertyShower.UnBind();
        }

        public async UniTask StartThinking()
        {
        }

        public async UniTask<IOperation> GetNextOperation()
        {
            if (currentOperationIndex >= data.candidateCards.Count)
            {
                currentOperationIndex = 0;
            }

            var carData = data.candidateCards[currentOperationIndex];
            return new UseCardOperation(carData);
        }

        public bool TryGetMask(out MaskEnum id)
        {
            id = default;
            return false;
        }

        public async UniTask UseCard(CardData cardData)
        {
            currentOperationIndex++;
        }

        public async UniTask TakeCard(CardData cardData)
        {
        }

        public async UniTask TakeDamage(int damageValue)
        {
            data.property.health.Value -= damageValue;
            // DOTween
            await _doTweenHitEffect.PlayHitEffect();
        }

        public UniTask GainShield(int value)
        {
            throw new NotImplementedException();
        }
    }
}