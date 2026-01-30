using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolkit;

namespace Jump
{
    public class CameraSystem : MonoBehaviour, ISystem, IOnInit
    {
        [field: SerializeField] public Camera mainCamera { get; private set; }
        public CinemachineBrain cinemachineBrain { get; private set; }
        public Camera uiCamera { get; private set; }

        private CinemachineBlendDefinition defaultBlendDefinition;

        private CinemachineBlenderSettings defaultCustomBlends;

        private CinemachineBlendDefinition cutBlendDefinition =
            new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0);

        /// <summary>
        /// 主相机是否正在混合中
        /// </summary>
        public bool isMainCameraBlending => cinemachineBrain.IsBlending;

        public void OnInit()
        {
            cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();

            defaultBlendDefinition = cinemachineBrain.DefaultBlend;
            defaultCustomBlends = cinemachineBrain.CustomBlends;

            uiCamera = UIRoot.Singleton.UICamera;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            var cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            foreach (var cam in cameras)
            {
                if (cam.CompareTag("MainCamera") && cam != mainCamera)
                {
                    GlobalLogger.LogInfo($"CameraSystem 新场景中有其他的 MainCamera 删除它 {cam.gameObject}");
                    Destroy(cam.gameObject);
                }
            }
        }


        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        public void ForceCutBlend()
        {
            cinemachineBrain.DefaultBlend = cutBlendDefinition;
            cinemachineBrain.CustomBlends = null;
        }

        public void RecoverBlend()
        {
            cinemachineBrain.DefaultBlend = defaultBlendDefinition;
            cinemachineBrain.CustomBlends = defaultCustomBlends;
        }
    }
}