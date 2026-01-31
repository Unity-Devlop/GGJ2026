using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfg;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(EntityPropertyShower))]
    public class PlayerController : MonoBehaviour, IEntityController
    {
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public PlayerData data { get; private set; }

        private EntityPropertyShower _propertyShower;

        private DoTweenHitEffect _doTweenHitEffect;

        [SerializeField] private TMP_Text maskText;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private Dictionary<CardEnum, int> thisRoundUseCardCount = new Dictionary<CardEnum, int>();


        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private CardEnum lastUsedCardThisRound;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private List<BuffInfo> _buffs = new List<BuffInfo>();

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private Dictionary<CardTypeEnum, int> mengpoData = new Dictionary<CardTypeEnum, int>();

        private void Awake()
        {
            _propertyShower = GetComponent<EntityPropertyShower>();
            _doTweenHitEffect = GetComponentInChildren<DoTweenHitEffect>();
        }

        public void Bind(PlayerData playerData)
        {
            this.data = playerData;
            _propertyShower.Bind(this.data.property);
        }

        public UniTask OnceKill()
        {
            data.property.health.Value = 0;
            return UniTask.CompletedTask;
        }

        public bool IsDead()
        {
            return data.property.health.Value <= 0;
        }

        public async UniTask GainHealth(int value)
        {
            data.property.health.Value += value;
            await UniTask.CompletedTask;
        }

        public async UniTask DrawCards(int count)
        {
            if (data.candidateCards.Count == 0)
            {
                Debug.Log("玩家可抽的牌组为空，无法抽牌");
                return;
            }

            await GamingMgr.Singleton.LocalPlayerDrawCards(count);
        }

        public bool TryGetLastUsedCardThisRound(out CardEnum cardEnum)
        {
            cardEnum = lastUsedCardThisRound;
            return lastUsedCardThisRound != CardEnum.None;
        }

        public UniTask AddBuff(BuffEnum buffEnum, object values)
        {
            _buffs.Add(new BuffInfo() { buffEnum = buffEnum, parameters = values });
            return UniTask.CompletedTask;
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

        public void UnBind()
        {
            data = null;
            _propertyShower.UnBind();
            _buffs.Clear();
            thisRoundUseCardCount.Clear();
            lastUsedCardThisRound = CardEnum.None;
        }


        public bool TryGetMask(out MaskEnum id)
        {
            id = data.currentMask;
            if (id == MaskEnum.本我) return false;
            return true;
        }


        public async UniTask UseCard(CardData cardData)
        {
            thisRoundUseCardCount.TryAdd(cardData.id, 0);
            thisRoundUseCardCount[cardData.id]++;
            lastUsedCardThisRound = cardData.id;
        }

        public async UniTask TurnStart()
        {
            lastUsedCardThisRound = CardEnum.None;
            thisRoundUseCardCount.Clear();
            await DrawCards(1);
            await UniTask.CompletedTask;
        }

        public async UniTask TurnEnd()
        {
            await BuffEffects.OnTurnEnd(this);
            lastUsedCardThisRound = CardEnum.None;
            RemoveBuff(BuffEnum.壮魂效果);

            CardEnum target = CardEnum.None;
            
            foreach (var mengpoConfig in Global.tables.MengpoTable.DataList)
            {
                // 找到第一个匹配的
                var dict = mengpoConfig.MengpoSoupEffect;
                // 看看是不是完全匹配
                bool isMatch = true;
                foreach (var kv in dict)
                {
                    mengpoData.TryAdd(kv.Key, 0);
                    if (mengpoData[kv.Key] < kv.Value)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    target = mengpoConfig.Id;
                    break;
                }
            }
            
            await CardEffects.ExecuteCardEffects(new CardData(target), this, GamingMgr.Singleton.GetEnemyEntity(this));
            
            mengpoData.Clear();
        }

        public int GetUseCardCount(CardEnum cardEnum)
        {
            thisRoundUseCardCount.TryAdd(cardEnum, 0);
            return thisRoundUseCardCount[cardEnum];
        }

        public async UniTask TakeCard(CardData cardData)
        {
        }

        public async UniTask TakeDamage(IEntityController sender, int damageValue, bool ignoreShield)
        {
            BuffEffects.ProcessTakeDamageBuffs(sender, this, ref damageValue);
            BuffEffects.ProcessTakeDamageIgnoreShieldBuffs(sender, this, ref ignoreShield);

            if (data.property.shield > 0 && !ignoreShield)
            {
                // 先扣护甲 扣完护甲如果还有伤害再扣血量
                int shieldDamage = Math.Min(data.property.shield, damageValue);
                int damageToHealth = damageValue - shieldDamage;
                data.property.shield -= shieldDamage;
                data.property.health.Value -= damageToHealth;
            }
            else
            {
                data.property.health.Value -= damageValue;
            }

            // DOTween
            await _doTweenHitEffect.PlayHitEffect();
        }

        public async UniTask GainShield(int value)
        {
            data.property.shield += value;
        }

        public async UniTask SwitchMask(MaskEnum id)
        {
            Global.Event.Invoke(new OnLocalPlayerWearMaskEvent(id));
            await Global.Event.Invoke<OnLocalPlayerWearMaskEvent, UniTask>(
                new OnLocalPlayerWearMaskEvent(id));
            Debug.Log("玩家切换面具: " + id);
            maskText.text = id.ToString();

            var enemy = GamingMgr.Singleton.GetEnemyEntity(this);
            if (data.currentMask == MaskEnum.阎王面具)
            {
                enemy.RemoveBuff(BuffEnum.死期);
            }

            data.currentMask = id;

            switch (id)
            {
                case MaskEnum.本我:
                    break;
                case MaskEnum.阎王面具:
                    await enemy.AddBuff(BuffEnum.死期, 5);
                    break;
                case MaskEnum.无常面具:
                    // TODO 默认黑 点自己切白
                    if (ContainsBuff(BuffEnum.黑无常))
                    {
                        RemoveBuff(BuffEnum.黑无常);
                        RemoveBuff(BuffEnum.白无常);
                        await AddBuff(BuffEnum.白无常, null);
                    }
                    else if (ContainsBuff(BuffEnum.白无常))
                    {
                        RemoveBuff(BuffEnum.黑无常);
                        RemoveBuff(BuffEnum.白无常);
                        await AddBuff(BuffEnum.黑无常, null);
                    }
                    else
                    {
                        RemoveBuff(BuffEnum.黑无常);
                        RemoveBuff(BuffEnum.白无常);
                        await AddBuff(BuffEnum.黑无常, null);
                    }

                    break;
                case MaskEnum.阎罗面具:
                    BuffEnum[] laws =
                    {
                        BuffEnum.监禁令,
                        BuffEnum.卸甲令,
                        BuffEnum.禁武令,
                    };

                    var targetLaw = laws.ToList().RandomTakeWithoutRemove();

                    enemy.RemoveBuff(BuffEnum.监禁令);
                    enemy.RemoveBuff(BuffEnum.卸甲令);
                    enemy.RemoveBuff(BuffEnum.禁武令);

                    await enemy.AddBuff(targetLaw, null);
                    break;
                case MaskEnum.孟婆面具:
                    break;
                case MaskEnum.二郎神面具:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }
        }

        private bool ContainsBuff(BuffEnum buffEnum)
        {
            foreach (var buff in _buffs)
            {
                if (buff.buffEnum == buffEnum)
                {
                    return true;
                }
            }

            return false;
        }

        public UniTask ReduceBuff(BuffEnum id, object parmaters)
        {
            foreach (var buff in _buffs)
            {
                if (buff.buffEnum == id)
                {
                    return BuffEffects.ReduceBuff(this, buff.buffEnum, parmaters);
                }
            }

            return UniTask.CompletedTask;
        }

        public void AddMengpoData(CardTypeEnum type, int value)
        {
            mengpoData.TryAdd(type, 0);
            mengpoData[type] += value;
        }
    }
}