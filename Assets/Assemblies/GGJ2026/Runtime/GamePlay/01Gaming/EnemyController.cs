using System;
using System.Collections.Generic;
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

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private CardEnum lastUsedCardThisRound;

        private EntityPropertyShower _propertyShower;
        private DoTweenHitEffect _doTweenHitEffect;
        private EnemyIntentVisual _enemyIntent;

        private int currentOperationIndex = 0;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private Dictionary<CardEnum, int> useCardCountThisTurn = new Dictionary<CardEnum, int>();

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private List<BuffInfo> _buffs = new List<BuffInfo>();

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

        public UniTask GainHealth(int value)
        {
            data.property.health.Value += value;
            return UniTask.CompletedTask;
        }

        public UniTask DrawCards(int count)
        {
            return UniTask.CompletedTask;
        }


        public bool TryGetLastUsedCardThisRound(out CardEnum cardEnum)
        {
            cardEnum = lastUsedCardThisRound;
            return lastUsedCardThisRound != default;
        }


        public UniTask AddBuff(BuffEnum 碎魂效果, object values)
        {
            _buffs.Add(new BuffInfo() { buffEnum = 碎魂效果, parameters = values });
            return UniTask.CompletedTask;
        }

        public void GetBuffs(out List<BuffInfo> buffInfos)
        {
            buffInfos = _buffs;
        }

        public void RemoveBuff(BuffInfo buff)
        {
            _buffs.Remove(buff);
        }

        public async UniTask OnApplyDamageTo(IEntityController tar, int value)
        {
            await BuffEffects.ProcessWhenApplyDamageTo(this, tar, value);
        }

        public UniTask SwitchMask(MaskEnum id)
        {
            return UniTask.CompletedTask;
        }

        public async UniTask ReduceBuff(BuffEnum id, object parmaters)
        {
            await BuffEffects.ReduceBuff(this, id, parmaters);
        }

        public void UnBind()
        {
            data = null;
            _propertyShower.UnBind();
            _buffs.Clear();
            useCardCountThisTurn.Clear();
            lastUsedCardThisRound = CardEnum.None;
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


        public UniTask TurnStart()
        {
            useCardCountThisTurn.Clear();
            lastUsedCardThisRound = CardEnum.None;
            return UniTask.CompletedTask;
        }

        public async UniTask TurnEnd()
        {
            lastUsedCardThisRound = CardEnum.None;
            await BuffEffects.OnTurnEnd(this);

            // 拿到下一次会出的牌
            if (data.candidateCards.Count > 0)
            {
                var nextCard = data.candidateCards[currentOperationIndex % data.candidateCards.Count];
                _enemyIntent.SetIntent(nextCard.config.Intent);
            }
        }

        public int GetUseCardCount(CardEnum cardEnum)
        {
            return useCardCountThisTurn.GetValueOrDefault(cardEnum, 0);
        }

        public bool TryGetMask(out MaskEnum id)
        {
            id = default;
            return false;
        }

        public async UniTask UseCard(CardData cardData)
        {
            currentOperationIndex++;
            useCardCountThisTurn.TryAdd(cardData.id, 0);
            useCardCountThisTurn[cardData.id]++;
        }

        public async UniTask TakeCard(CardData cardData)
        {
        }

        public async UniTask TakeDamage(int damageValue)
        {
            BuffEffects.ProcessTakeDamageBuffs(this, ref damageValue);
            data.property.health.Value -= damageValue;
            // DOTween
            await _doTweenHitEffect.PlayHitEffect();
        }

        public UniTask OnceKill()
        {
            data.property.health.Value = 0;
            return UniTask.CompletedTask;
        }

        public UniTask GainShield(int value)
        {
            throw new NotImplementedException();
        }
    }
}