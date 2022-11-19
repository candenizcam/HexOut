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
            
            //ActivateLevel(GameDataBase.LevelSeedDatas[levelIndex]);
            //UIDocument.rootVisualElement.Add(_activeLevel.FieldFrame);
            Proceduraler();

        }



        void Proceduraler()
        {
            var v1 = 0f;
            var v2 = 0f;
            var s = "";
            var r = new Random();

            for (int totalObs = 2; totalObs < 5; totalObs++)
            {
                var totalObstacles = totalObs * testCol;
                for (int doubleObs = 1; doubleObs <= totalObs - 2; doubleObs++)
                {

                    var doubleObstacles = doubleObs * testCol;
                    var singleObstacles = totalObstacles - doubleObstacles;
                    for (int os = 0; os < 6; os++)
                    {
                        var obstacleSeed = totalObs * 1000000 + doubleObs * 10000 + r.Next(0,9999);
                        var editorSeed = new LevelSeedData("testLevel", testRow, testCol, 0, obstacleSeed, 0,
                            singleObstacles, doubleObstacles, LevelSeedData.SeedType.FrameLevel, 1);
                        var obstacleOnly = LevelGenerator.GenerateSeededLevel(editorSeed); //0.00123

                        var possible = ((testRow - 2) * (testCol - 2)) / 2;

                        var pCount = 0;
                        for (int p = 0; p < possible / 2; p++)
                        {
                            var capsuleNo = possible - 2 * p;
                            if(capsuleNo< ((testRow - 2) * (testCol - 2)) / 5) break;

                            var n = 0;
                            for (int i2 = 0; i2 < 100; i2++)
                            {
                                var seed = r.Next();
                                //0.001
                                var t1 = Time.realtimeSinceStartup;
                                var d = LevelGenerator.GenerateFrameLevelWithNewObstacles(obstacleOnly, seed,
                                    capsuleNo); //0.000362
                                if (d.ld.CapsuleDatas.Count() < capsuleNo)
                                {
                                    continue;
                                }
                                
                                
                                
                                var b = LevelTester.TestLevel(d.ld); //0.00065
                                var t2 = Time.realtimeSinceStartup;
                                v1 += t2-t1;
                                v2 += 1f;
                                if (b >= 0)
                                {
                                    n += 1;
                                    s += editorSeed.RecordMe(name: $"\"{obstacleSeed}_{capsuleNo}_{seed}\"",
                                        capsuleSeed: seed, capsuleNumber: capsuleNo, difficulty: b);
                                }


                                if (n > 2)
                                {
                                    break;
                                }
                            }

                            if (n > 0)
                            {
                                pCount += 1;
                            }
                        }

                        if (pCount > 3)
                        {
                            break;
                        }
                    }
                }
            }
            
            
            Debug.Log(s);
            Debug.Log($"mean is: {v1/v2}");




        }

        void TestLevels()
        {
            //var obstacleSeed = testObstacleSeed;
            

            var v1 = 0f;
            var v2 = 0f;
            
            var s = "";
            for (int totalObs = 2; totalObs < 5; totalObs++)
            {
                var totalObstacles = totalObs * testCol;
                for (int doubleObs = 1; doubleObs <= totalObs-2; doubleObs++)
                {
                    
                    var doubleObstacles = doubleObs * testCol;
                    var singleObstacles = totalObstacles-doubleObstacles;

                    for (int os = 0; os < 6; os++)
                    {
                        //0.23
                        var t1 = Time.realtimeSinceStartup;
                        var obstacleSeed = totalObs * 10000 + doubleObs * 100 + os;
                        var editorSeed = new LevelSeedData("testLevel",testRow, testCol, 0, obstacleSeed, 0,
                            singleObstacles,doubleObstacles,LevelSeedData.SeedType.FrameLevel,1);
                        
                        
                        var obstacleOnly = LevelGenerator.GenerateSeededLevel(editorSeed); //0.00123
                        
                        for (int cn = 9; cn >= 0; cn--)
                        {
                            var capsuleNo = 2 * cn + 3;
                
                            var n = 0;
                            for (int i = 0; i < 100; i++)
                            {
                                //0.002
                                var d = LevelGenerator.GenerateFrameLevelWithNewObstacles(obstacleOnly, i, capsuleNo); //0.001098
                                
                                
                                if (d.ld.CapsuleDatas.Count()<capsuleNo)
                                {
                                    continue;
                                }
                                
                                var b = LevelTester.TestLevel(d.ld); //0.003644377
                                
                                
                                if (b>=0)
                                {
                                    n += 1;
                                    s+=editorSeed.RecordMe(name:$"\"{obstacleSeed}_{capsuleNo}_{i}\"",capsuleSeed: i,capsuleNumber:capsuleNo,difficulty:b);
                                }
                                

                                if (n > 2)
                                {
                                    break;
                                }
                            }
                
                            

                            if (n == 0)
                            {
                                Debug.Log($"too many capsules {cn}");
                                break; //too many capsules
                            }
            
            
                        }
                        
                        var t2 = Time.realtimeSinceStartup;
                        v1 += t2 - t1;
                        v2 += 1f;


                    }
                    
                    
                    
                }
                
                
            }
            Debug.Log(s);
            Debug.Log($"mean is: {v1/v2}");
            
            

            
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
                UIDocument.rootVisualElement.Add(_activeLevel.FieldFrame);
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
