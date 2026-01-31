using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(EntityPropertyShower))]
    public class PlayerController : MonoBehaviour,IEntityController
    {
        public PlayerData playerData { get; private set; }
        private EntityPropertyShower _propertyShower;
        private void Awake()
        {
            _propertyShower = GetComponent<EntityPropertyShower>();
        }
        public void Bind(PlayerData playerData)
        {
            this.playerData = playerData;
            _propertyShower.Bind(this.playerData.property);
        }

        public bool IsDead()
        {
            return playerData.property.health.Value <= 0;
        }

        public void UnBind()
        {
            playerData = null;
            _propertyShower.UnBind();
        }

        public async UniTask UseCard(CardData cardData)
        {
            
        }

        UniTask IEntityController.GainShield(int value)
        {
            throw new System.NotImplementedException();
        }

        public UniTask TakeCard(CardData cardData)
        {
            throw new System.NotImplementedException();
        }

        public UniTask TakeDamage(int value)
        {
            throw new System.NotImplementedException();
        }

        public async Task GainShield(int value)
        {
            
        }
    }
}