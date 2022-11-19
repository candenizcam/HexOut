using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Punity;
using Punity.ui;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

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
        public bool resetSaves;
        public int levelIndex;
        
        private GameState _gameState;
        
        
        protected override void InitializeMain()
        {
            _gameState = GameState.Standby;
            UIDocument.rootVisualElement.style.paddingBottom = Constants.UnsafeBottomUi;
            UIDocument.rootVisualElement.style.paddingTop = Constants.UnsafeTopUi;
            Application.targetFrameRate = 60;
            
            MainCamera.backgroundColor = Constants.BackgroundColour;

            

            if (Application.isEditor && resetSaves)
            {
                Serializer.Apply<SerialHexOutData>((sgd) =>
                {
                    sgd.playerLevel = 0;
                    sgd.playerXp = 0;

                });
            }

            //TestLevels();
            
            //var editorSeed2 = new LevelSeedData("test level",testRow, testCol, testCapsuleSeed, testObstacleSeed, testCapsuleNumber,
           //     testObstacleNumber,testDoubleObstacleNumber,LevelSeedData.SeedType.FrameLevel,1);
           ;
            
            ActivateLevel(GameDataBase.LevelSeedDatas[levelIndex]);
            
            
            //LevelGenerator.ProceduralBatch(testRow, testCol);

        }



        


        private void ActivateBetweenLevels(SerialHexOutData sgd, LevelCompleteData lcd)
        {
            var oldLevel = sgd.playerLevel;
            var oldXPForBar =(float) sgd.playerXp;
            var n = XPSystem.AddXP(sgd.playerLevel, sgd.playerXp, lcd.LevelXp);
            
            var levelUp = n.newLevel > oldLevel; 
            
            sgd.playerXp = n.newXp;
            sgd.playerLevel = n.newLevel;
            
            
            var levelXP = (float)XPSystem.LevelXp(sgd.playerLevel);
            var thisXP = (float)sgd.playerXp;

            var rightText = $"+{lcd.LevelXp}\n{levelXP-thisXP}/{levelXP}\nto next level";
            
            var betweenLevels = new BetweenLevels(oldLevel,
                filler: thisXP/ levelXP,
                oldFiller:oldXPForBar/levelXP,
                rightText:rightText,
                levels:n.newLevel-oldLevel,newSkin:n.newSkin>0);
            
            
            betweenLevels.LeftButtonAction = () =>
            {

            };
            betweenLevels.MiddleButtonAction = () =>
            {
                levelIndex += 1;
                ActivateLevel(GameDataBase.LevelSeedDatas[levelIndex]);
                UIDocument.rootVisualElement.Remove(betweenLevels);
            };
            betweenLevels.RightButtonAction = () =>
            {

            };
            UIDocument.rootVisualElement.Add(betweenLevels);

            if (levelUp)
            {
                betweenLevels.LevelUpAnimation(0f);
                TweenHolder.NewTween(Constants.BetweenLevelsTime,duringAction: (alpha) =>
                {
                    betweenLevels.LevelUpAnimation(alpha);
                });
            }
            else
            {
                betweenLevels.InitializationAnimation(0f);
                TweenHolder.NewTween(Constants.BetweenLevelsTime,duringAction: (alpha) =>
                {
                    betweenLevels.InitializationAnimation(alpha);
                });
                
            }
            
        }



        /** This one starts from a seed and generates the relevant level
         * seed contains all the relevant data, including row & col
         * grid is initiated here too
         */
        private void ActivateLevel(LevelSeedData seed)
        {
            var d = LevelGenerator.GenerateSeededLevel(seed);
            d.Name = seed.Name;
            _activeLevel = GameLevelScript.Instantiate();
            
            //var d = LevelGenerator.GenerateRawFrame(seed,2);
            _activeLevel.SetGameLevelInfo(d);
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
            _activeLevel.CapsuleRemovedAction = (oldXP, newXP) =>
            {
                Serializer.Apply<SerialHexOutData>( sgd =>
                {
                    var oldN = XPSystem.AddXP(sgd.playerLevel, sgd.playerXp, oldXP);
                    var newN = XPSystem.AddXP(sgd.playerLevel, sgd.playerXp, newXP+oldXP);
                    
                    var oldLevel = oldN.newLevel;
                    var oldXPForBar =(float) oldN.newXp;
                    
                    var levelXP = (float)XPSystem.LevelXp(sgd.playerLevel);
                    var levelUp = newN.newLevel > sgd.playerLevel; // if it ever increases in level, change here
                    var levelUpWasTheCase = oldLevel > sgd.playerLevel; // if it ever increases in level, change here
                    if (levelUpWasTheCase)
                    {
                        _activeLevel.FieldFrame.SetIndicatorText(bigText:$"{newN.newLevel}",levelUp:true);
                    }
                    else
                    {
                        TweenHolder.NewTween(0.15f,duringAction: (alpha) =>
                        {
                            if (levelUp)
                            {
                                _activeLevel.FieldFrame.SetBar(oldXPForBar*(1f-alpha)/levelXP + alpha );
                            
                            }
                            else
                            {
                                _activeLevel.FieldFrame.SetBar((oldXPForBar*(1f-alpha) + (float)alpha*newN.newXp)/levelXP );
                            }
                        
                        },exitAction: () =>
                        {
                            _activeLevel.FieldFrame.SetIndicatorText(bigText:$"{newN.newLevel}",levelUp:levelUp);
                        });
                    }
                });

            };
            UIDocument.rootVisualElement.Add(_activeLevel.FieldFrame);
            Serializer.Apply<SerialHexOutData>(sgd =>
                {
                    var levelXP = (float)XPSystem.LevelXp(sgd.playerLevel);
                    _activeLevel.FieldFrame.SetBar(sgd.playerXp/levelXP);
                    _activeLevel.FieldFrame.SetIndicatorText(bigText:$"{sgd.playerLevel}");
                }
            );
            

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
