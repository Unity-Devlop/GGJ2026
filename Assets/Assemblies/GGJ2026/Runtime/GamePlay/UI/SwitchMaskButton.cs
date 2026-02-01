using System;
using cfg;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(Button))]
    public class SwitchMaskButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

            if (id == MaskEnum.本我)
            {
                return;
            }

            GamingMgr.Singleton.PushPlayerOperation(new SwitchMaskOperation(id, true));
        }

        public void Bind(MaskEnum id)
        {
            this.id = id;
            nameText.text = id.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Global.Event.Invoke<OnMaskButtonPointerEnter>(new OnMaskButtonPointerEnter(id));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Global.Event.Invoke<OnMaskButtonPointerExit>(new OnMaskButtonPointerExit(id));
        }
    }
}