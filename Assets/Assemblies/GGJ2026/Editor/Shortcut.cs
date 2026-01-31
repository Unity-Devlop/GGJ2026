using UnityEngine.SceneManagement;

#if UNITY_EDITOR
namespace GGJ2026.Editor
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    public class EditorSceneShortcut
    {
        // 定义快捷键：
        // _q = Q
        // %q = Ctrl + Q
        // #q = Shift + Q
        // &q = Alt + Q

        // 强烈建议不要单用 Q，因为 Q 是 Unity 的"抓手工具"快捷键，会冲突。
        // 这里我帮你设为 Alt + Q
        [MenuItem("Tools/Quick Open Scene &q")]
        public static void QuickOpen()
        {
            // 这里填你场景的具体路径，必须带 .unity 后缀
            string scenePath = "Assets/Assemblies/GGJ2026/Resource/Scenes/GameEntry.unity";

            // 询问是否保存当前场景，防止丢失进度
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                Debug.Log("已快速打开场景: " + scenePath);
                SceneManager.SetActiveScene(scene);

                EditorApplication.isPlaying = true;
            }
        }
    }
}
#endif