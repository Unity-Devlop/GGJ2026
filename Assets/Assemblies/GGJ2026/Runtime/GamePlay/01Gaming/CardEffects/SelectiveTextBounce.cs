namespace GGJ2026.GamePlay
{
    using UnityEngine;
    using TMPro;
    using Cysharp.Threading.Tasks;
    using System.Threading;

    public class SelectiveTextBounce : MonoBehaviour
    {
        private TMP_Text _textComponent;

        [Header("设置")] public float bounceSpeed = 10f; // 跳动速度
        public float bounceHeight = 5f; // 跳动高度
        public float characterDelay = 0.5f; // 每个字之间的相位差（形成波浪感）

        void Awake() => _textComponent = GetComponent<TMP_Text>();

        /// <summary>
        /// 开启特定范围文字的跳动
        /// </summary>
        /// <param name="startIndex">开始跳动的字符索引</param>
        /// <param name="length">跳动的字数</param>
        public async UniTask PlayPartialBounce(int startIndex, int length, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _textComponent.ForceMeshUpdate();
                var textInfo = _textComponent.textInfo;

                // 确保索引不越界
                int end = Mathf.Min(startIndex + length, textInfo.characterCount);

                for (int i = startIndex; i < end; i++)
                {
                    var charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible) continue;

                    // 计算当前字符的 y 轴偏移量
                    float offset = Mathf.Sin(Time.time * bounceSpeed + i * characterDelay) * bounceHeight;

                    // 获取该字符对应的顶点数据
                    int matIndex = charInfo.materialReferenceIndex;
                    int vIndex = charInfo.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[matIndex].vertices;

                    // 修改该字符的全部 4 个顶点
                    vertices[vIndex + 0].y += offset; // Bottom Left
                    vertices[vIndex + 1].y += offset; // Top Left
                    vertices[vIndex + 2].y += offset; // Top Right
                    vertices[vIndex + 3].y += offset; // Bottom Right
                }

                // 将修改后的顶点推送回 Mesh
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    _textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }
}