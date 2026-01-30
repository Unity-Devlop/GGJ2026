using Cysharp.Threading.Tasks;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GamingMgr : MonoSingleton<GamingMgr>
    {
        public bool isGameOver;

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }

        public void StartGame()
        {
            var gamePlayPanel = UIRoot.Singleton.OpenPanel<GamePlayPanel>();
            Global.localSave.Get<PlayerData>(out var playerData);
            gamePlayPanel.Bind(playerData);
        }


        private async UniTask X()
        {
        }

        public void EndGame()
        {
            UIRoot.Singleton.ClosePanel<GamePlayPanel>();
        }
    }
}