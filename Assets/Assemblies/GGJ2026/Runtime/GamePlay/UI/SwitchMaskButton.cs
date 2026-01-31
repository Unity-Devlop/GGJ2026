using cfg;
using TMPro;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    public class SwitchMaskButton : MonoBehaviour
    {
        [Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.ShowInInspector]
        private MaskEnum id;

        [field: SerializeField] public TextMeshProUGUI nameText;

        public void Bind(MaskEnum id)
        {
            this.id = id;
            nameText.text = id.ToString();
        }
    }
}