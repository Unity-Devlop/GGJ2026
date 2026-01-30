using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityToolkit;

namespace GGJ2026
{
    public class LocalSaveSystem : ISystem, IOnInit
    {
        private ModelCenter _modelCenter;

        public static string dataPath
        {
            get
            {
#if UNITY_EDITOR
                return $"{Application.dataPath}/Editor Default Resources/GameData";
#endif
                return $"{Application.dataPath}/GameData";
            }
        }

        public void OnInit()
        {
            _modelCenter = new ModelCenter();
        }

        public void Dispose()
        {
        }

        public void Add<T>(T data) where T : IModel
        {
            _modelCenter.Register(data);
        }

        public void Get<T>(out T data) where T : IModel
        {
            data = _modelCenter.Get<T>();
        }


        private static void MakeSurePathExist()
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
        }

        public static bool Read<T>(string key, out T data)
        {
            MakeSurePathExist();
            data = default;
            string path = $"{dataPath}/{key}.json";

            GlobalLogger.LogInfo($"读取数据key[{key}],类型[{typeof(T)}],路径[{path}]");
            if (!File.Exists(path))
            {
                return false;
            }

            var content = File.ReadAllText(path);
            data = JsonConvert.DeserializeObject<T>(content);
            return true;
        }

        public static void Write<T>(string key, T data)
        {
            MakeSurePathExist();
            string path = $"{dataPath}/{key}.json";
            GlobalLogger.LogInfo($"写入数据key[{key}],类型[{typeof(T)}],路径[{path}]");
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }
    }
}