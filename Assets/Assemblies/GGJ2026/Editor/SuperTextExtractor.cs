namespace GGJ2026.Editor
{
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GGJ2026.Editor
{
    [Serializable]
    public class SuperTextExtractor
    {
        private const string StringRegex = @"""((?:[^""\\]|\\.)*)""";
        private static HashSet<string> _results = new HashSet<string>();

        [Sirenix.OdinInspector.Button]
        public void ExtractAll()
        {
            _results.Clear();

            // 1. 处理代码和JSON (正则扫描)
            ExtractFromFiles(".cs");
            ExtractFromFiles(".json");

            // 2. 处理 Prefabs (组件扫描)
            ExtractFromPrefabs();

            // 3. 处理 Scenes (组件扫描)
            ExtractFromScenes();

            // 保存结果
            SaveToFile();
        }

        private static void ExtractFromFiles(string extension)
        {
            string[] files = Directory.GetFiles(Application.dataPath, $"*{extension}", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string content = File.ReadAllText(file);
                var matches = Regex.Matches(content, StringRegex);
                foreach (Match m in matches)
                {
                    AddResult(m.Groups[1].Value);
                }
            }
        }

        private static void ExtractFromPrefabs()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null) continue;

                ExtractFromGameObject(prefab);
            }
        }

        private static void ExtractFromScenes()
        {
            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
            string currentScene = EditorSceneManager.GetActiveScene().path;

            foreach (var guid in sceneGuids)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guid);
                // 注意：由于加载场景比较慢，如果你场景非常多，可以考虑只扫描当前打开的场景
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                
                foreach (var root in scene.GetRootGameObjects())
                {
                    ExtractFromGameObject(root);
                }
                
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        private static void ExtractFromGameObject(GameObject go)
        {
            // 提取原生 Text
            var legacyTexts = go.GetComponentsInChildren<Text>(true);
            foreach (var t in legacyTexts) AddResult(t.text);

            // 提取 TextMeshPro
            var tmpTexts = go.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var t in tmpTexts) AddResult(t.text);
            
            var tmp3D = go.GetComponentsInChildren<TextMeshPro>(true);
            foreach (var t in tmp3D) AddResult(t.text);
        }

        private static void AddResult(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            // 过滤掉一些明显的占位符，如 "New Text"
            if (text == "New Text") return;
            _results.Add(text.Trim());
        }

        private static void SaveToFile()
        {
            string outputPath = Path.Combine(Application.dataPath, "FullProjectStrings.txt");
            List<string> list = new List<string>(_results);
            list.Sort();
            File.WriteAllLines(outputPath, list);
            AssetDatabase.Refresh();
            Debug.Log($"<color=cyan>全量提取完成！总计唯一文本: {_results.Count} 条。结果保存至: {outputPath}</color>");
        }
    }
}
}