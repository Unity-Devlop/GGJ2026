using System;
using System.Runtime.CompilerServices;
using Capabilities;
using cfg;
using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace GGJ2026
{
    public class Global : MonoSingleton<Global>
    {
        protected override bool DontDestroyOnLoad() => true;
        private SystemLocator _systemLocator = new SystemLocator();

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private GameFlow _gameFlow;

        private CameraSystem _cameraSystem;


        private AudioSystem _audioSystem;

        private AssetSystem _assetSystem;

        private GlobalRefHolder _refHolder;

        private LocalSaveSystem _localSaveSystem;

        private DataSystem _dataSystem;


        public static GameFlow gameFlow => Singleton._gameFlow;

        public static CameraSystem cameraSystem => Singleton._cameraSystem;

        public static AudioSystem audioSystem => Singleton._audioSystem;

        public static AssetSystem asset => Singleton._assetSystem;

        public static GlobalRefHolder refHolder => Singleton._refHolder;

        public static LocalSaveSystem localSave => Singleton._localSaveSystem;

        public static DataSystem dataSystem => Singleton._dataSystem;


        #region Event

        private static TypeEventSystem _event;

        public static TypeEventSystem Event
        {
            get
            {
                if (_event == null)
                {
                    _event = new TypeEventSystem();
                }

                return _event;
            }
        }

        #endregion

        #region Table

        private static Tables _tables;

        public static Tables tables
        {
            get
            {
                if (_tables == null)
                {
                    _tables = new Tables(TableLoader);
                }

                return _tables;
            }
        }

        private static JSONNode TableLoader(string name)
        {
            var asset = Addressables.LoadAssetAsync<TextAsset>($"Config/Luban/{name}.json").WaitForCompletion();
            return JSONNode.Parse(asset.text);
        }

        #endregion


        protected override void OnInit()
        {
            _event = new TypeEventSystem();

            _tables = new Tables(TableLoader);

            UIRoot.Singleton.UIDatabase.Loader = new AddressablesUILoader();

            _refHolder = GetComponentInChildren<GlobalRefHolder>();

            _systemLocator = new SystemLocator();
            _systemLocator.Register(GetComponentInChildren<GameFlow>());
            _systemLocator.Register(GetComponentInChildren<CameraSystem>());
            _systemLocator.Register(new AudioSystem());
            _systemLocator.Register(new AssetSystem());
            _systemLocator.Register(new LocalSaveSystem());
            _systemLocator.Register(new DataSystem());


            _cameraSystem = _systemLocator.Get<CameraSystem>();
            _gameFlow = _systemLocator.Get<GameFlow>();
            _audioSystem = _systemLocator.Get<AudioSystem>();
            _assetSystem = _systemLocator.Get<AssetSystem>();
            _localSaveSystem = _systemLocator.Get<LocalSaveSystem>();
            _dataSystem = _systemLocator.Get<DataSystem>();
            gameObject.name = nameof(Global);

            _gameFlow.Run();
        }


        protected override void OnDispose()
        {
            _systemLocator.Dispose();
            _tables = null;
            _event = null;
        }

        private void Update()
        {
            foreach (var system in _systemLocator.systems)
            {
                if (system is IOnUpdate updater)
                {
                    updater.OnUpdate(Time.deltaTime);
                }
            }
        }


    }
}