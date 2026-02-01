using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cfg;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(EntityPropertyShower))]
    public class EnemyController : MonoBehaviour, IEntityController
    {
        [Sirenix.OdinInspector.ShowInInspector]
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


        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private Dictionary<CardTypeEnum, int> mengpoData = new Dictionary<CardTypeEnum, int>();

        private void Awake()
        {
            _propertyShower = GetComponent<EntityPropertyShower>();
            _enemyIntent = GetComponentInChildren<EnemyIntentVisual>();
        }


        public void Bind(EnemyData enemyData)
        {
            var cfg = Global.tables.GhostTable.Get(enemyData.id);
            Addressables.InstantiateAsync(cfg.PrefabPath, transform).WaitForCompletion();


            _doTweenHitEffect = GetComponentInChildren<DoTweenHitEffect>();
            this.data = enemyData;
            _propertyShower.Bind(this.data.property);
            currentOperationIndex = 0;
            foreach (var cardData in data.candidateCards)
            {
                Debug.Log($"Enemy Candidate Card: {cardData.config.Id}");
            }
        }

        public void UnBind()
        {
            Addressables.ReleaseInstance(_doTweenHitEffect.gameObject);

            data = null;
            _propertyShower.UnBind();
            _buffs.Clear();
            useCardCountThisTurn.Clear();
            lastUsedCardThisRound = CardEnum.None;
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


        public async UniTask AddBuff(BuffEnum buffEnum, object values)
        {
            await BuffEffects.OnBuffAdded(this, buffEnum, values);
            _buffs.Add(new BuffInfo() { buffEnum = buffEnum, parameters = values });
        }

        public void GetBuffs(out List<BuffInfo> buffInfos)
        {
            buffInfos = _buffs;
        }

        public void RemoveBuff(BuffEnum buff)
        {
            _buffs.RemoveAll(b => b.buffEnum == buff);
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


        public void AddMengpoData(CardTypeEnum type, int value)
        {
            mengpoData.TryAdd(type, 0);
            mengpoData[type] += value;
        }

        public UniTask ClearShield()
        {
            data.property.shield = 0;
            return UniTask.CompletedTask;
        }

        public string GetName()
        {
            if (data == null)
            {
                return "这是敌人";
            }

            var cfg = Global.tables.GhostTable.Get(data.id);
            return cfg.Id.ToString();
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
                _enemyIntent.SetIntent(nextCard.config.Type);
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

        public async UniTask TakeDamage(IEntityController sender, int damageValue, bool ignoreShield)
        {
            BuffEffects.ProcessTakeDamageBuffs(sender, this, ref damageValue);
            BuffEffects.ProcessTakeDamageIgnoreShieldBuffs(sender, this, ref ignoreShield);

            RuntimeManager.PlayOneShot(Global.refHolder.attack);
            RuntimeManager.PlayOneShot(Global.refHolder.受击);

            if (data.property.shield > 0 && !ignoreShield)
            {
                // 先扣护甲 扣完护甲如果还有伤害再扣血量
                int shieldDamage = Math.Min(data.property.shield, damageValue);
                int damageToHealth = damageValue - shieldDamage;
                data.property.shield -= shieldDamage;

                RuntimeManager.PlayOneShot(Global.refHolder.defence);
                GamingMgr.Singleton.OnEntityTakeDamage(transform.position, damageToHealth);
                data.property.health.Value -= damageToHealth;
            }
            else
            {
                GamingMgr.Singleton.OnEntityTakeDamage(transform.position, damageValue);
                data.property.health.Value -= damageValue;
            }

            if (data.property.health.Value <= 0)
            {
                RuntimeManager.PlayOneShot(Global.refHolder.dead);
            }

            // DOTween
            await _doTweenHitEffect.PlayHitEffect();
        }

        public UniTask OnceKill()
        {
            GamingMgr.Singleton.OnEntityTakeDamage(transform.position, 9999);
            data.property.health.Value = 0;
            RuntimeManager.PlayOneShot(Global.refHolder.dead);

            return UniTask.CompletedTask;
        }

        public UniTask GainShield(int value)
        {
            BuffEffects.ProcessGainShieldBuffs(this, this, ref value);
            data.property.shield += value;
            return UniTask.CompletedTask;
        }
    }
}