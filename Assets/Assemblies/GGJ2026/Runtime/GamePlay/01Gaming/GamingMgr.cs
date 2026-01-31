using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GamingMgr : MonoSingleton<GamingMgr>
    {
        public enum GamingState
        {
            GameStart,
            PlayerRound,
            EnemyRound,
            GameOver,
        }

        public bool isGameOver;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private EnemyData enemyData;


        [SerializeField] private PlayerController playerController;
        [SerializeField] private EnemyController enemyController;


        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public Queue<IOperation> playerOperationQueue = new Queue<IOperation>();

        public GamingState currentGamingState { get; private set; }


        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }

        public void StartGame()
        {
            GameFlow().Forget();
        }

        private bool playerOperationInProgress = false;

        private async UniTask GameFlow()
        {
            currentGamingState = GamingState.GameStart;
            isGameOver = false;
            var gamePlayPanel = UIRoot.Singleton.OpenPanel<GamePlayPanel>();
            Global.localSave.Get<GameData>(out var gameData);
            gamePlayPanel.Bind(gameData.playerData);

            playerController.Bind(playerData);
            enemyController.Bind(enemyData);


            while (true)
            {
                isGameOver = playerController.IsDead() || enemyController.IsDead();
                if (isGameOver) break;

                if (currentGamingState == GamingState.PlayerRound)
                {
                    while (playerOperationQueue.Count > 0)
                    {
                        var operation = playerOperationQueue.Dequeue();
                        if (operation is UseCardOperation useCardOperation)
                        {
                            await playerController.UseCard(useCardOperation.cardData);
                            // 结算伤害
                            CardEffects.ExecuteCardEffects(useCardOperation.cardData, playerController,
                                enemyController);
                        }
                    }
                }
                else if (currentGamingState == GamingState.EnemyRound)
                {
                }

                await UniTask.Yield();
            }

            playerController.UnBind();
            enemyController.UnBind();
            currentGamingState = GamingState.GameOver;
        }


        public void EndGame()
        {
            isGameOver = true;
            UIRoot.Singleton.ClosePanel<GamePlayPanel>();
        }


        public bool PushPlayerOperation(IOperation useCardOperation)
        {
            playerOperationQueue.Enqueue(useCardOperation);
            return currentGamingState == GamingState.PlayerRound;
        }
    }
}