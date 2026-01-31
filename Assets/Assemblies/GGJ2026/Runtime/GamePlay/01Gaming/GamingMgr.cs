using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
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
            Global.Event.Invoke(currentGamingState);

            isGameOver = false;
            var gamePlayPanel = UIRoot.Singleton.OpenPanel<GamePlayPanel>();
            Global.localSave.Get<GameData>(out var gameData);
            gamePlayPanel.Bind(gameData.playerData);

            playerController.Bind(playerData);
            enemyController.Bind(enemyData);


            currentGamingState = GamingState.PlayerRound;
            Global.Event.Invoke(currentGamingState);
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
                            var cardData = useCardOperation.cardData;
                            await playerController.UseCard(cardData);
                            // 结算伤害
                            await CardEffects.ExecuteCardEffects(cardData, playerController,
                                enemyController);

                            if (cardData.config.EndRoundWhenUse)
                            {
                                currentGamingState = GamingState.EnemyRound;
                                Global.Event.Invoke(currentGamingState);
                                Assert.IsTrue(playerOperationQueue.Count == 0,
                                    "结束回合操作执行时，玩家操作队列不为空");
                                break;
                            }
                        }
                    }
                }
                else if (currentGamingState == GamingState.EnemyRound)
                {
                    await enemyController.StartThinking();

                    while (enemyController.wantedOperation)
                    {
                        var operation = await enemyController.GetNextOperation();
                        if (operation is UseCardOperation useCardOperation)
                        {
                            var cardData = useCardOperation.cardData;
                            await enemyController.TakeCard(cardData);
                            // 结算伤害
                            await CardEffects.ExecuteCardEffects(cardData, enemyController,
                                playerController);
                        }
                    }

                    currentGamingState = GamingState.PlayerRound;
                    Global.Event.Invoke(currentGamingState);
                }

                await UniTask.Yield();
            }

            playerController.UnBind();
            enemyController.UnBind();
            currentGamingState = GamingState.GameOver;
            Global.Event.Invoke(currentGamingState);
        }


        public void EndGame()
        {
            isGameOver = true;
            UIRoot.Singleton.ClosePanel<GamePlayPanel>();
        }


        public bool PushPlayerOperation(IOperation operation)
        {
            if (playerOperationQueue.Count > 0)
            {
                var first = playerOperationQueue.Peek();
                if (first is UseCardOperation useCardOperation && useCardOperation.cardData.config.EndRoundWhenUse)
                {
                    Debug.LogWarning("当前有结束回合的操作在队列中，无法添加新的操作");
                    return false;
                }
            }

            playerOperationQueue.Enqueue(operation);
            return currentGamingState == GamingState.PlayerRound;
        }
    }
}