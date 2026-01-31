namespace GGJ2026.GamePlay
{
    using UnityEngine;
    using TMPro;
    using DG.Tweening;
    using Cysharp.Threading.Tasks;

    public class CharBounceEffect : MonoBehaviour
    {
        private TMP_Text textComponent;

        void Awake() => textComponent = GetComponent<TMP_Text>();

        /// <summary>
        /// 让已经显示的文字持续波浪跳动
        /// </summary>
        [Sirenix.OdinInspector.Button]
        public async UniTask PlayWaveEffect(System.Threading.CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                textComponent.ForceMeshUpdate();
                var textInfo = textComponent.textInfo;

                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    var charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible) continue;

                    // 计算每个字符的偏移量（基于时间的正弦波）
                    float offset = Mathf.Sin(Time.time * 10f + i * 0.5f) * 5f;

                    // 修改该字符对应的 4 个顶点坐标
                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;
                    Vector3[] sourceVertices = textInfo.meshInfo[materialIndex].vertices;

                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
                    for (int j = 0; j < 4; j++)
                    {
                        destinationVertices[vertexIndex + j] =
                            sourceVertices[vertexIndex + j] + new Vector3(0, offset, 0);
                    }
                }

                // 更新网格
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                await UniTask.Yield();
            }
        }
    }
}