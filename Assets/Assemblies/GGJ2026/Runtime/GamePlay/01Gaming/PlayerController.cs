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

        public bool IsDead()
        {
            return data.property.health.Value <= 0;
        }

        public void UnBind()
        {
            data = null;
            _propertyShower.UnBind();
        }

        public async UniTask UseCard(CardData cardData)
        {
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

        public async Task SwitchMask(MaskEnum id)
        {
            maskText.text = id.ToString();
        }
    }
}