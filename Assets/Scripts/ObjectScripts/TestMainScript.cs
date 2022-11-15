using System.Collections.Generic;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Punity;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class TestMainScript : MainScript
    {
        private GameLevelScript _activeLevel;
        public int testRow;
        public int testCol;
        public int testObstacleSeed;
        public int testCapsuleSeed;
        public int testCapsuleNumber;
        public int testObstacleNumber;
        public int testDoubleObstacleNumber;
        private GameState _gameState;
        
        
        protected override void InitializeMain()
        {
            _gameState = GameState.Standby;
            UIDocument.rootVisualElement.style.paddingBottom = Constants.UnsafeBottomUi;
            UIDocument.rootVisualElement.style.paddingTop = Constants.UnsafeTopUi;
            Application.targetFrameRate = 60;
            
            MainCamera.backgroundColor = Color.white;

            Serializer.Apply<SerialHexOutData>((shd) =>
            {
                Debug.Log($"xp: {shd.playerXp}");
                
            });
            
            //_activeLevel.StartAnimation();
            var editorSeed = new LevelSeedData(testRow, testCol, testCapsuleSeed, testObstacleSeed, testCapsuleNumber,
                testObstacleNumber,testDoubleObstacleNumber,LevelSeedData.SeedType.FrameLevel,1);
            
            ActivateLevel(editorSeed);
            

            UIDocument.rootVisualElement.Add(_activeLevel.FieldFrame);
            

        }


        private void ActivateBetweenLevels(SerialHexOutData sgd, LevelCompleteData lcd)
        {
            var oldLevel = sgd.playerLevel;
            var oldXP = sgd.playerXp;
            var n = XPSystem.AddXP(sgd.playerLevel, sgd.playerXp, lcd.LevelXp);
            if (n.newLevel > oldLevel)
            {
                Debug.Log("level up");
            }
            sgd.playerXp = n.newXp;
            sgd.playerLevel = n.newLevel;
            
            
            var playerLevel = $"{sgd.playerLevel}";
            var levelXP = (float)XPSystem.LevelXp(sgd.playerLevel);
            var thisXP = (float)sgd.playerXp;
            
            var betweenLevels = new BetweenLevels(playerLevel,filler: thisXP/ levelXP);
            
            
            betweenLevels.LeftButtonAction = () =>
            {

            };
            betweenLevels.MiddleButtonAction = () =>
            {
                ActivateLevel(new LevelSeedData(testRow, testCol, testCapsuleSeed, testObstacleSeed, testCapsuleNumber,
                    testObstacleNumber,testDoubleObstacleNumber,LevelSeedData.SeedType.FrameLevel,1));
                UIDocument.rootVisualElement.Remove(betweenLevels);
            };
            betweenLevels.RightButtonAction = () =>
            {

            };
            UIDocument.rootVisualElement.Add(betweenLevels);
            
            
        }



        /** This one starts from a seed and generates the relevant level
         * seed contains all the relevant data, including row & col
         * grid is initiated here too
         */
        private void ActivateLevel(LevelSeedData seed)
        {
            var d = LevelGenerator.GenerateSeededLevel(seed);
            _activeLevel = GameLevelScript.Instantiate();
            
            //var d = LevelGenerator.GenerateRawFrame(seed,2);
            _activeLevel.SetGameLevelInfo(1);
            _activeLevel.SetGrid(MainCamera,d.Row,d.Col, d.ObstacleDatas);
            _activeLevel.SetCapsules(d.CapsuleDatas);
            _activeLevel.AddTweenAction = (tween, delay) =>
            {
                TweenHolder.NewTween(tween, delay);
            }; 

            TweenHolder.NewTween(0.5f,duringAction: (alpha) =>
            {
                _activeLevel.StartAnimation(alpha);
            }, exitAction: () =>
            {
                _activeLevel.StartAnimation(1f);
                _gameState = GameState.Game;
            }, delay:.4f);
            _activeLevel.LevelDoneAction = (lcd) =>
            {
                Serializer.Apply<SerialHexOutData>( sgd =>
                {
                    ActivateBetweenLevels(sgd,lcd);
                });
                
                
                UIDocument.rootVisualElement.Remove(_activeLevel.FieldFrame);
                Destroy(_activeLevel.gameObject);

            };

        }
        
        
        
        


        private void HandleTouch()
        {
            if (Input.touches.Length > 0)
            {
                var tp = Input.touches[0].phase;
                if (tp == TouchPhase.Began)
                {
                    var wp = MainCamera.ScreenToWorldPoint(Input.touches[0].position);
                    _activeLevel.TouchBegan(wp);
                }else if (tp == TouchPhase.Ended)
                {
                    var wp = MainCamera.ScreenToWorldPoint(Input.touches[0].position);
                    _activeLevel.TouchEnded(wp);
                }
                else
                {
                    _activeLevel.DuringTouch();
                }
            }
        }
        
        
        
        
        

        protected override void UpdateMain()
        {
            if (_gameState == GameState.Game)
            {
                HandleTouch();
            }

            
            

        }


        private enum GameState
        {
            Game, Standby
        }
        
    }
}
