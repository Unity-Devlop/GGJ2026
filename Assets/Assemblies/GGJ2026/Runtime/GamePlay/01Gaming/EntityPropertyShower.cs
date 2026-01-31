using System;
using TMPro;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    public class EntityPropertyShower : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text shieldText;


        private EntityPropertyData _propertyData;
        public void Bind(EntityPropertyData propertyData)
        {
            _propertyData = propertyData;
        }

        private void Update()
        {
            if (_propertyData == null) return;
            healthText.text = _propertyData.health.Value.ToString();
            shieldText.text = _propertyData.shield.ToString();
        }

        public void UnBind()
        {
            _propertyData = null;
        }
    }
}