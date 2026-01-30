using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using UnityEngine.Localization.Tables;

namespace Framework
{
    [ExecuteAlways]
    [RequireComponent(typeof(LocalizeStringEvent))]
    internal class LocalizationText : TextMeshProUGUI
    {
#if UNITY_EDITOR
        private static Action<UnityEventString, int, object, MethodInfo> RegisterPersistentListener;
        private static Action<UnityEventString> AddPersistentListenerAction;
        private static bool _initialized = false;
        private static MethodInfo _setTextPropertyMethodInfo;

        [SerializeField] private bool localized = false;

        private static void InitializeReflectionMethod()
        {
            if (_initialized) return;
            Type type = typeof(UnityEventBase);


            MethodInfo addPersistentListenerMethodInfo =
                type.GetMethod("AddPersistentListener", BindingFlags.Instance | BindingFlags.NonPublic);
            AddPersistentListenerAction = (UnityEventString eventString) =>
            {
                addPersistentListenerMethodInfo.Invoke(eventString, null);
            };


            MethodInfo registerPersistent =
                type.GetMethod("RegisterPersistentListener", BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new Type[] { typeof(int), typeof(object), typeof(MethodInfo) }, null);

            RegisterPersistentListener =
                (UnityEventString eventString, int index, object obj, MethodInfo setTextPropertyMethodInfo) =>
                {
                    registerPersistent.Invoke(eventString, new object[] { index, obj, setTextPropertyMethodInfo });
                };

            _setTextPropertyMethodInfo = typeof(TMP_Text).GetProperty("text").GetSetMethod();

            _initialized = true;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (localized) return;
            InitializeReflectionMethod();
            var localizeStringEvent = GetComponent<LocalizeStringEvent>();
            if (localizeStringEvent == null)
                localizeStringEvent = gameObject.AddComponent<LocalizeStringEvent>();
            // 通过反射注册进去
            AddPersistentListenerAction.Invoke(localizeStringEvent.OnUpdateString);
            RegisterPersistentListener.Invoke(localizeStringEvent.OnUpdateString, 0, this, _setTextPropertyMethodInfo);
            localized = true;
        }

        private static LocalizationText Create()
        {
            var go = new GameObject("LocalizationText");
            var localizationText = go.AddComponent<LocalizationText>();
            localizationText.text = "Localized Text Here";
            localizationText.fontSize = 36;
            localizationText.alignment = TextAlignmentOptions.Center;
            return localizationText;
        }

        [UnityEditor.MenuItem("GameObject/UI/Localization Text", false, 10)]
        private static void CreateLocalizationText()
        {
            // 获取当前选中的对象
            var parent = UnityEditor.Selection.activeGameObject;
            // 创建LocalizationText对象
            if (parent == null)
            {
                // 找到第一个根节点
                return;
            }

            var go = Create();
            go.transform.SetParent(parent.transform, false);
        }

#endif
    }
}