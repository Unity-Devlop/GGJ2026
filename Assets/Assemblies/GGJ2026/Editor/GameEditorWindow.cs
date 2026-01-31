using Framework.Editor;
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
                { "存档系统", new SaveSystemEditor() , EditorIcons.SettingsCog },
            };
            
            return odinMenuTree;
        }
    }
}