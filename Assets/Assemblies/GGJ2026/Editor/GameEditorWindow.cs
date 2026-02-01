using Framework.Editor;
using GGJ2026.Editor.GGJ2026.Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GGJ2026.Editor
{
    public class GameEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/EditorWindow")]
        private static void ShowWindow()
        {
            var window = GetWindow<GameEditorWindow>();
            window.titleContent = new GUIContent("GameEditorWindow");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree odinMenuTree = new OdinMenuTree()
            {
                { "卡牌效果生成器", new CardEffectGeneratorEditor(), EditorIcons.SettingsCog },
                { "存档系统", new SaveSystemEditor(), EditorIcons.SettingsCog },
                { "文字收集", new SuperTextExtractor(), EditorIcons.SettingsCog },
                { "FMOD事件生成器", new FMODEditor(), EditorIcons.SettingsCog },
            };

            return odinMenuTree;
        }
    }
}