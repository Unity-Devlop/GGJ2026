using System.Collections.Generic;
using System.Threading.Tasks;
using cfg;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

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

        public UniTask GainHealth(int value)
        {
            data.property.health.Value += value;
            return UniTask.CompletedTask;
        }

        public async UniTask DrawCards(int count)
        {
            await GamingMgr.Singleton.LocalPlayerDrawCards(count);
        }

        public bool TryGetLastUsedCardThisRound(out CardEnum cardEnum)
        {
            cardEnum = lastUsedCardThisRound;
            return lastUsedCardThisRound != CardEnum.None;
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
            await BuffEffects.ProcessWhenApplyDamageTo(this,tar, value);
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
        }

        public UniTask TurnStart()
        {
            lastUsedCardThisRound = CardEnum.None;
            thisRoundUseCardCount.Clear();
            return UniTask.CompletedTask;
        }

        public UniTask TurnEnd()
        {
            lastUsedCardThisRound = CardEnum.None;
            return UniTask.CompletedTask;
        }

        public int GetUseCardCount(CardEnum cardEnum)
        {
            thisRoundUseCardCount.TryAdd(cardEnum, 0);
            return thisRoundUseCardCount[cardEnum];
        }

        public async UniTask TakeCard(CardData cardData)
        {
        }

        public async UniTask TakeDamage(int value)
        {
            data.property.health.Value -= value;
            // DOTween
            await _doTweenHitEffect.PlayHitEffect();
        }

        public async UniTask GainShield(int value)
        {
        }

        public async UniTask SwitchMask(MaskEnum id)
        {
            maskText.text = id.ToString();
            data.currentMask = id;
        }
    }
}