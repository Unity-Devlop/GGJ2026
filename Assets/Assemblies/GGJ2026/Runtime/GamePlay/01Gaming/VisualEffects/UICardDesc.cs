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
            descText.text = args.data.config.Desc;
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