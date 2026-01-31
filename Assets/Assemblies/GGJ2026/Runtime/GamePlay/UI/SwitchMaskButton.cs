using System;
using cfg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(Button))]
    public class SwitchMaskButton : MonoBehaviour
    {
        [Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.ShowInInspector]
        private MaskEnum id;

        private Button button;

        [field: SerializeField] public TextMeshProUGUI nameText;

        private void Awake()
        {
            button = this.GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var player = GamingMgr.Singleton.GetLocalPlayer();

            if (player.TryGetMask(out var mask) && mask == id)
            {
                if (id == MaskEnum.无常面具)
                {
                    GamingMgr.Singleton.PushPlayerOperation(new SwitchMaskOperation(id, true));
                }

                Debug.LogWarning("Already in this mask: " + id);
                return;
            }

            GamingMgr.Singleton.PushPlayerOperation(new SwitchMaskOperation(id, true));
        }

        public void Bind(MaskEnum id)
        {
            this.id = id;
            nameText.text = id.ToString();
        }
    }
}