using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UICardDesc : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI descText;
        private CharBounceEffect _charBounceEffect;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _charBounceEffect = descText.gameObject.GetComponent<CharBounceEffect>();

            Global.Event.Listen<OnUICardVisualPointerEnter>(OnUICardVisualPointerEnter);
            Global.Event.Listen<OnUICardVisualPointerExit>(OnUICardVisualPointerExit);


            gameObject.SetActive(false);
        }


        private void OnUICardVisualPointerExit(in OnUICardVisualPointerExit args)
        {
            descText.text = string.Empty;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            _bounceEffectCts?.Cancel();
        }

        private CancellationTokenSource _bounceEffectCts;

        private void OnUICardVisualPointerEnter(in OnUICardVisualPointerEnter args)
        {
            string desc;
            var atk = GamingMgr.Singleton.GetLocalPlayer();
            var cardData = args.data;
            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                var newCardConfig = Global.tables.CardTable.Get(newCardEffectId);
                desc = newCardConfig.Desc;
                for (int i = 0; i < newCardConfig.Value.Length; i++)
                {
                    desc = desc.Replace("{" + i + "}", newCardConfig.Value[i].ToString());
                }
            }
            else
            {
                desc = args.data.config.Desc;
                for (int i = 0; i < args.data.config.Value.Length; i++)
                {
                    desc = desc.Replace("{" + i + "}", args.data.config.Value[i].ToString());
                }
            }


            descText.text = desc;

            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _bounceEffectCts?.Cancel();
            _bounceEffectCts = new CancellationTokenSource();
            _charBounceEffect.PlayWaveEffect(_bounceEffectCts.Token).Forget();
        }

        private void OnDestroy()
        {
            Global.Event.UnListen<OnUICardVisualPointerEnter>(OnUICardVisualPointerEnter);
            Global.Event.UnListen<OnUICardVisualPointerExit>(OnUICardVisualPointerExit);
        }
    }
}