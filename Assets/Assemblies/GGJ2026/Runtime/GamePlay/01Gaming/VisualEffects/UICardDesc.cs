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
            Global.Event.Listen<OnMaskButtonPointerEnter>(OnMaskButtonPointerEnter);
            Global.Event.Listen<OnMaskButtonPointerExit>(OnMaskButtonPointerExit);
            Global.Event.Listen<OnPointerEnterHealthTriggerEvent>(OnPointerEnterHealthTriggerEvent);
            Global.Event.Listen<OnPointerExitHealthTriggerEvent>(OnPointerExitHealthTriggerEvent);
            Global.Event.Listen<OnPointerEnterShieldTriggerEvent>(OnPointerEnterShieldTriggerEvent);
            Global.Event.Listen<OnPointerExitShieldTriggerEvent>(OnPointerExitShieldTriggerEvent);
            Global.Event.Listen<OnPointerEnterBuffTriggerEvent>(OnPointerEnterBuffTriggerEvent);
            Global.Event.Listen<OnPointerExitBuffTriggerEvent>(OnPointerExitBuffTriggerEvent);
            Global.Event.Listen<OnPointerEnterPlayerTagEvent>(OnPointerEnterPlayerTagEvent);
            Global.Event.Listen<OnPointerExitPlayerTagEvent>(OnPointerExitPlayerTagEvent);
            Global.Event.Listen<OnPointerEnterEnemyTagEvent>(OnPointerEnterEnemyTagEvent);
            Global.Event.Listen<OnPointerExitEnemyTagEvent>(OnPointerExitEnemyTagEvent);
            Global.Event.Listen<OnEnemyIntentPointerEnterEvent>(OnPointerEnterEnemyIntentEvent);
            Global.Event.Listen<OnEnemyIntentPointerExitEvent>(OnPointerExitEnemyIntentEvent);

            gameObject.SetActive(false);
        }


        private void OnDestroy()
        {
            Global.Event.UnListen<OnUICardVisualPointerEnter>(OnUICardVisualPointerEnter);
            Global.Event.UnListen<OnUICardVisualPointerExit>(OnUICardVisualPointerExit);
            Global.Event.UnListen<OnMaskButtonPointerEnter>(OnMaskButtonPointerEnter);
            Global.Event.UnListen<OnMaskButtonPointerExit>(OnMaskButtonPointerExit);
            Global.Event.UnListen<OnPointerEnterHealthTriggerEvent>(OnPointerEnterHealthTriggerEvent);
            Global.Event.UnListen<OnPointerExitHealthTriggerEvent>(OnPointerExitHealthTriggerEvent);
            Global.Event.UnListen<OnPointerEnterShieldTriggerEvent>(OnPointerEnterShieldTriggerEvent);
            Global.Event.UnListen<OnPointerExitShieldTriggerEvent>(OnPointerExitShieldTriggerEvent);
            Global.Event.UnListen<OnPointerEnterBuffTriggerEvent>(OnPointerEnterBuffTriggerEvent);
            Global.Event.UnListen<OnPointerExitBuffTriggerEvent>(OnPointerExitBuffTriggerEvent);
            Global.Event.UnListen<OnPointerEnterPlayerTagEvent>(OnPointerEnterPlayerTagEvent);
            Global.Event.UnListen<OnPointerExitPlayerTagEvent>(OnPointerExitPlayerTagEvent);
            Global.Event.UnListen<OnPointerEnterEnemyTagEvent>(OnPointerEnterEnemyTagEvent);
            Global.Event.UnListen<OnPointerExitEnemyTagEvent>(OnPointerExitEnemyTagEvent);
            Global.Event.UnListen<OnEnemyIntentPointerEnterEvent>(OnPointerEnterEnemyIntentEvent);
            Global.Event.UnListen<OnEnemyIntentPointerExitEvent>(OnPointerExitEnemyIntentEvent);
        }

        private void OnPointerExitEnemyIntentEvent(in OnEnemyIntentPointerExitEvent args)
        {
            descText.text = string.Empty;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            _bounceEffectCts?.Cancel();
        }

        private void OnPointerEnterEnemyIntentEvent(in OnEnemyIntentPointerEnterEvent args)
        {
            descText.text = $"意图:{args.intent.ToString()}";

            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _bounceEffectCts?.Cancel();
            _bounceEffectCts = new CancellationTokenSource();
            _charBounceEffect.PlayWaveEffect(_bounceEffectCts.Token).Forget();
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


        private void OnMaskButtonPointerExit(in OnMaskButtonPointerExit args)
        {
            descText.text = string.Empty;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            _bounceEffectCts?.Cancel();
        }

        private void OnMaskButtonPointerEnter(in OnMaskButtonPointerEnter args)
        {
            descText.text = Global.tables.MaskTable.Get(args.maskID).Desc;

            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _bounceEffectCts?.Cancel();
            _bounceEffectCts = new CancellationTokenSource();
            _charBounceEffect.PlayWaveEffect(_bounceEffectCts.Token).Forget();
        }

        private void OnPointerExitBuffTriggerEvent(in OnPointerExitBuffTriggerEvent args)
        {
            descText.text = string.Empty;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            _bounceEffectCts?.Cancel();
        }

        private void OnPointerEnterBuffTriggerEvent(in OnPointerEnterBuffTriggerEvent args)
        {
            descText.text = Global.tables.BuffTable.Get(args.buffID).Desc;

            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _bounceEffectCts?.Cancel();
            _bounceEffectCts = new CancellationTokenSource();
            _charBounceEffect.PlayWaveEffect(_bounceEffectCts.Token).Forget();
        }

        private void OnPointerExitShieldTriggerEvent(in OnPointerExitShieldTriggerEvent args)
        {
            descText.text = string.Empty;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            _bounceEffectCts?.Cancel();
        }

        private void OnPointerEnterShieldTriggerEvent(in OnPointerEnterShieldTriggerEvent args)
        {
            descText.text = "护盾，可以抵挡伤害。";

            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _bounceEffectCts?.Cancel();
            _bounceEffectCts = new CancellationTokenSource();
            _charBounceEffect.PlayWaveEffect(_bounceEffectCts.Token).Forget();
        }

        private void OnPointerExitHealthTriggerEvent(in OnPointerExitHealthTriggerEvent args)
        {
            descText.text = string.Empty;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            _bounceEffectCts?.Cancel();
        }

        private void OnPointerEnterHealthTriggerEvent(in OnPointerEnterHealthTriggerEvent args)
        {
            descText.text = "生命值 归零后失败";

            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _bounceEffectCts?.Cancel();
            _bounceEffectCts = new CancellationTokenSource();
            _charBounceEffect.PlayWaveEffect(_bounceEffectCts.Token).Forget();
        }


        private void OnPointerExitEnemyTagEvent(in OnPointerExitEnemyTagEvent args)
        {
            descText.text = string.Empty;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            _bounceEffectCts?.Cancel();
        }

        private void OnPointerEnterEnemyTagEvent(in OnPointerEnterEnemyTagEvent args)
        {
            args.entityController.GetBuffs(out var buffs);
            string desc = $"这是{args.entityController.GetName()}\n";
            foreach (var buff in buffs)
            {
                var buffConfig = Global.tables.BuffTable.Get(buff.buffEnum);
                string parameters = buff.parameters != null ? buff.parameters.ToString() : "";
                desc += $"{buffConfig.Id}-{parameters}: {buffConfig.Desc}\n";
            }

            descText.text = desc;
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _bounceEffectCts?.Cancel();
            _bounceEffectCts = new CancellationTokenSource();
            _charBounceEffect.PlayWaveEffect(_bounceEffectCts.Token).Forget();
        }

        private void OnPointerExitPlayerTagEvent(in OnPointerExitPlayerTagEvent args)
        {
            descText.text = string.Empty;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            _bounceEffectCts?.Cancel();
        }

        private void OnPointerEnterPlayerTagEvent(in OnPointerEnterPlayerTagEvent args)
        {
            args.entityController.GetBuffs(out var buffs);
            string desc = $"这是{args.entityController.GetName()}\n";
            foreach (var buff in buffs)
            {
                var buffConfig = Global.tables.BuffTable.Get(buff.buffEnum);
                string parameters = buff.parameters != null ? buff.parameters.ToString() : "";
                desc += $"{buffConfig.Id}-{parameters}: {buffConfig.Desc}\n";
            }

            descText.text = desc;
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _bounceEffectCts?.Cancel();
            _bounceEffectCts = new CancellationTokenSource();
            _charBounceEffect.PlayWaveEffect(_bounceEffectCts.Token).Forget();
        }
    }
}