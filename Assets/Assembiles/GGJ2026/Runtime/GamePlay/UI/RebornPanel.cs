using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Jump.GamePlay
{
    public class RebornPanel : UIPanel
    {
        private static readonly int _center = Shader.PropertyToID("_Center");
        private static readonly int _scale = Shader.PropertyToID("_Scale");
        [SerializeField] private RawImage image; // 使用 FlashScreen.shader 的材质实例
        private Material flashMat => image.material;
        [SerializeField] private float deadAnimationDuration = 0.4f;
        [SerializeField] private float rebornAnimationDuration = 0.4f;
        private const float rebornMaxScale = 1.4f;

        public override void OnLoaded()
        {
            base.OnLoaded();
            image.material = new Material(image.material);
            flashMat.SetVector(_center, new Vector4(0.5f, 0.5f, 0f, 0f));
        }

        public void SetCenterFromPlayer(Vector3 playerPosWS)
        {
            var sp = Global.cameraSystem.mainCamera.WorldToScreenPoint(playerPosWS);
            Vector2 uv = new Vector2(sp.x / Screen.width, sp.y / Screen.height);
            flashMat.SetVector(_center, new Vector4(uv.x, uv.y, 0f, 0f));
        }


        [Sirenix.OdinInspector.Button]
        public async UniTask ShowDeadAnimation()
        {
            for (float t = 0; t < deadAnimationDuration; t += Time.deltaTime)
            {
                float p = t / deadAnimationDuration;
                flashMat.SetFloat(_scale, Mathf.Lerp(rebornMaxScale, 0f, p));
                await UniTask.Yield();
            }

            // flashMat.SetFloat(_scale, 0f);
        }

        [Sirenix.OdinInspector.Button]
        public async UniTask ShowRebornAnimation()
        {
            for (float t = 0; t < rebornAnimationDuration; t += Time.deltaTime)
            {
                float p = t / rebornAnimationDuration;
                flashMat.SetFloat(_scale, Mathf.Lerp(0f, rebornMaxScale, p));
                await UniTask.Yield();
            }

            // flashMat.SetFloat(_scale, rebornMaxScale);
        }

        public override void OnOpened()
        {
            base.OnOpened();
        }

        public override void OnClosed()
        {
            base.OnClosed();
        }

        public override void OnDispose()
        {
            base.OnDispose();
        }
    }
}