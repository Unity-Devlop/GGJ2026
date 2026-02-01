using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
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
        public bool isGameWin;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private PlayerData playerData;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private EnemyData enemyData;

        private GamePlayPanel gamePlayPanel;

        public PlayerTag playerTag { get; private set; }
        public EnemyTag enemyTag { get; private set; }

        private void Update()
        {
            // 将鼠标屏幕坐标转为世界坐标
            Vector2 mousePos = Global.cameraSystem.mainCamera.ScreenToWorldPoint(Pointer.current.position.value);

            // 发射一条长度极短的射线（或者直接点检测）
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            playerTag = null;
            enemyTag = null;
            if (hit.collider == null) return;
            if (hit.collider.TryGetComponent<PlayerTag>(out var sr))
            {
                Debug.Log("找到PlayerTag");
                playerTag = sr;
            }
            else if (hit.collider.TryGetComponent<EnemyTag>(out var er))
            {
                Debug.Log("找到EnemyTag");
                enemyTag = er;
            }
        }

        private async UniTask GameFlow()
        {
            isGameOver = false;
            isGameWin = false;
            Debug.Log("游戏开始".Color(Color.aquamarine));
            currentGamingState = GamingState.GameStart;
            await Global.Event.Invoke<GamingState, UniTask>(currentGamingState);


            Global.localSave.Get<GameData>(out var gameData);

            var currentLevel = gameData.lastCompletedLevel;
            playerData = Global.refHolder.levelConfig.levelPlayerData[currentLevel].DeepCopy();
            enemyData = Global.refHolder.levelConfig.levelEnemyData[currentLevel].DeepCopy();


            playerController.Bind(playerData);
            enemyController.Bind(enemyData);


            UIRoot.Singleton.DisposeAll();
            gamePlayPanel = UIRoot.Singleton.OpenPanel<GamePlayPanel>();

            gamePlayPanel.Bind(playerData);

            // await UniTask.Delay(TimeSpan.FromSeconds(1));
            currentGamingState = GamingState.PlayerRound;
            await Global.Event.Invoke<GamingState, UniTask>(currentGamingState);

            await playerController.TurnStart();
            await enemyController.TurnEnd();
            await playerController.SwitchMask(MaskEnum.本我);


            isGameOver = enemyController.IsDead() || playerController.IsDead();
            isGameWin = enemyController.IsDead() && !playerController.IsDead();


            while (true)
            {
                isGameOver = playerController.IsDead() || enemyController.IsDead();
                isGameWin = enemyController.IsDead() && !playerController.IsDead();
                if (isGameOver) break;

                if (currentGamingState == GamingState.PlayerRound)
                {
                    bool endPlayerRound = false;
                    while (playerOperationQueue.Count > 0)
                    {
                        var operation = playerOperationQueue.Dequeue();
                        if (operation is UseCardOperation useCardOperation)
                        {
                            var cardData = useCardOperation.cardData;
                            await playerController.UseCard(cardData);
                            // 结算伤害
                            bool endRound = await CardEffects.ExecuteCardEffects(cardData, playerController,
                                enemyController);

                            if (endRound)
                            {
                                endPlayerRound = true;
                                break;
                            }
                        }
                        else if (operation is SwitchMaskOperation switchMask)
                        {
                            await playerController.SwitchMask(switchMask.id);
                            if (switchMask.endRoundRightAfter)
                            {
                                endPlayerRound = true;
                                break;
                            }
                        }
                        else if (operation is EndTurnOperation)
                        {
                            endPlayerRound = true;
                            break;
                        }
                    }

                    if (endPlayerRound)
                    {
                        currentGamingState = GamingState.EnemyRound;
                        await Global.Event.Invoke<GamingState, UniTask>(currentGamingState);
                        Assert.IsTrue(playerOperationQueue.Count == 0,
                            "结束回合操作执行时，玩家操作队列不为空");
                        await playerController.TurnEnd();
                        await enemyController.TurnStart();
                    }
                }
                else if (currentGamingState == GamingState.EnemyRound)
                {
                    await enemyController.StartThinking();

                    while (true)
                    {
                        var operation = await enemyController.GetNextOperation();
                        Debug.Log($"敌人执行操作：{operation}");
                        if (operation is UseCardOperation useCardOperation)
                        {
                            var cardData = useCardOperation.cardData;
                            await enemyController.TakeCard(cardData);
                            // 结算伤害
                            bool endRound = await CardEffects.ExecuteCardEffects(cardData, enemyController,
                                playerController);
                            if (endRound)
                            {
                                break;
                            }
                        }
                    }

                    currentGamingState = GamingState.PlayerRound;
                    await Global.Event.Invoke<GamingState, UniTask>(currentGamingState);
                    await enemyController.TurnEnd();
                    await playerController.TurnStart();
                }

                await UniTask.Yield();
            }

            isGameWin = enemyController.IsDead() && !playerController.IsDead();
            currentGamingState = GamingState.GameOver;
            await Global.Event.Invoke<GamingState, UniTask>(currentGamingState);
        }

        public void ExitGame()
        {
            isGameOver = false;
            Global.localSave.Get<GameData>(out var gameData);
            currentGamingState = GamingState.GameOver;


            if (playerController.IsDead())
            {
            }
            else if (enemyController.IsDead())
            {
                if (gameData.lastCompletedLevel == EnumHelper<GameLevelEnum>.keys[^1])
                {
                    Debug.Log("通关最后一关，回到第一关");
                    gameData.lastCompletedLevel = GameLevelEnum.第一关;
                }
                else
                {
                    Debug.Log("提升关卡");
                    gameData.lastCompletedLevel += 1;
                }
            }

            gamePlayPanel.UnBind();
            playerController.UnBind();
            enemyController.UnBind();
            UIRoot.Singleton.Dispose<GamePlayPanel>();
        }


        public bool PushPlayerOperation(IOperation operation)
        {
            if (playerOperationQueue.Contains(operation))
            {
                Debug.LogWarning("操作队列中已存在相同操作，无法重复添加");
                return false;
            }

            if (playerOperationQueue.Count > 0)
            {
                var first = playerOperationQueue.Peek();
                if (first is UseCardOperation useCardOperation && useCardOperation.cardData.config.CanEndRound)
                {
                    Debug.LogWarning("当前有结束回合的操作在队列中，无法添加新的操作");
                    return false;
                }
            }

            playerOperationQueue.Enqueue(operation);
            return currentGamingState == GamingState.PlayerRound;
        }

        public async UniTask LocalPlayerDrawCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (playerData.randomDrawCard)
                {
                    var cardId = playerData.candidateCards.RandomTakeWithoutRemove();
                    await gamePlayPanel.DrawCard(new CardData(cardId));
                }
                else
                {
                    var currentIndex = playerData.currentDrawIndex;
                    if (currentIndex >= playerData.candidateCards.Count)
                    {
                        currentIndex = 0;
                    }
                    else if (currentIndex < 0)
                    {
                        currentIndex = 0;
                    }

                    var cardId = playerData.candidateCards[currentIndex];
                    playerData.currentDrawIndex = currentIndex + 1;
                    await gamePlayPanel.DrawCard(new CardData(cardId));
                }
            }
        }

        public IEntityController GetEnemyEntity(IEntityController whoAreYou)
        {
            if (ReferenceEquals(whoAreYou, playerController))
            {
                return enemyController;
            }

            if (ReferenceEquals(whoAreYou, enemyController))
            {
                return playerController;
            }

            throw new Exception("无法识别的实体控制器");
        }

        public IEntityController GetLocalPlayer()
        {
            return playerController;
        }
    }
}