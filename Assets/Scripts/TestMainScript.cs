using System;
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
        public string levelId;
        public int levelDiff;
        public SkinType skinType;
        private Tween _activePopup;
        private GameState _gameState;
        
        
        protected override void InitializeMain()
        {
            _gameState = GameState.Standby;
            UIDocument.rootVisualElement.style.paddingBottom = Constants.UnsafeBottomUi;
            UIDocument.rootVisualElement.style.paddingTop = Constants.UnsafeTopUi;
            Application.targetFrameRate = 60;

            

            

            if (Application.isEditor && resetSaves)
            {
                Serializer.Reset<SerialHexOutData>();
                
            }

            //TestLevels();
            //9_5_22_207
            
            
           
           Serializer.Apply<SerialHexOutData>(sgd =>
           {
               
               GameDataBase.GetSkinType();
               var f = XPSystem.DrawGameLevelFromNo(sgd.playerLevel,sgd.playedLevels);
               ActivateLevel(f.data);
               levelIndex = f.index;
               levelId = f.data.Name;
               levelDiff = f.data.LevelDifficulty;
               
           });
           

           

           MainCamera.backgroundColor = GameDataBase.BackgroundColour();

           //problematic: "9_5_3_17"

           //ActivateLevel(GameDataBase.LevelSeedDatas[levelIndex]);






           //var v2 = GameDataBase.LevelSeedDatas.Where(x => x.Col == 7 && x.Row == 13);

           //var v = v2.Sum(x => x.LevelDifficulty * x.CapsuleNumber)/v2.Count();


           //Debug.Log($"v: {v}");

        }

        private void LevelFromEditorData()
        {
            var editorSeed2 = new LevelSeedData("test level",testRow, testCol, testCapsuleSeed, testObstacleSeed, testCapsuleNumber,
            testObstacleNumber,testDoubleObstacleNumber,LevelSeedData.SeedType.FrameLevel,1);
            GameDataBase.SetSkinType(skinType);
            Debug.Log(editorSeed2.RecordMe());
            ActivateLevel(editorSeed2);
            levelId = editorSeed2.Name;
            levelDiff = editorSeed2.LevelDifficulty;
        }
        

        private void SortGameData()
        {
            var s = "";
            //LevelGenerator.ProceduralBatch(5, 9);
            GameDataBase.LevelSeedDatas
                .OrderBy(x => x.LevelDifficulty * x.CapsuleNumber)
                .Select(x => x.RecordMe()).ToList().ForEach(x => { s += x;});
                
            Debug.Log(s);
        }

        private void NewProcedural()
        {
            var obsArray = new (int Sin, int Doub)[]{(1,1),(1,2),(2,1),(2,2),(2,3),(3,2) };
            var r = 15;
            var c = 9;
            var seperates = 5;
            var res = "";
            var prods = "";
            for (int j = 0; j < obsArray.Length; j++)
            {
                for (int i = 0; i < seperates; i++)
                {
                    var q = LevelGenerator.TotalRandom(20,r,c,obsArray[j].Sin*c,obsArray[j].Doub*c,j*seperates+i+1);
                    res += q.res+"\n";
                    prods += q.produced;
                }
                
            }
            
            
            Debug.Log(prods);
            Debug.Log(res);
        }



        


        private void ActivateBetweenLevels(SerialHexOutData sgd, LevelCompleteData lcd)
        {
            var oldLevel = sgd.playerLevel;
            var oldXPForBar =(float) sgd.playerXp;
            var n = XPSystem.AddXP(sgd.playerLevel, sgd.playerXp, lcd.LevelXp);
            
            var levelUp = n.newLevel > oldLevel; 
            
            sgd.playerXp = n.newXp;
            
            sgd.playerLevel = n.newLevel;
            
            sgd.playedLevels.Add(lcd.LevelId);

            var fireNewSkins = sgd.UnlockNewSkins(n.newSkin);
            
            
            var levelXP = (float)XPSystem.LevelXp(sgd.playerLevel);
            var thisXP = (float)sgd.playerXp;

            var rightText = $"+{lcd.LevelXp}\n{levelXP-thisXP}/{levelXP}\nto next level";
            
            //Debug.Log($"old level {oldLevel}, oldxp {oldXPForBar}, new level{sgd.playerLevel}, new xp {sgd.playerXp}");
            
            
            
            
            
            
            var betweenLevels = new BetweenLevels(oldLevel,
                filler: thisXP/ levelXP,
                oldFiller:oldXPForBar/levelXP,
                rightText:rightText,
                levels:n.newLevel-oldLevel,
                newSkin:fireNewSkins,
                newSkinActive:sgd.SkinSelectionActive());
            
            
            betweenLevels.LeftButtonAction = () =>
            {

                UIDocument.rootVisualElement.Remove(betweenLevels);
                var sgdn = Serializer.Load<SerialHexOutData>();
                var se = new SkinSelectionElement(sgdn.activeSkins.ToArray(),sgdn.activeSkin)
                {
                    
                    
                    LeftButtonAction = ()=>
                    {
                        // at some point, maybe obsolete even then
                    },
                    RightButtonAction = ()=>
                    {
                        
                    }
                };

                se.ExitButtonAction = () =>
                {
                    UIDocument.rootVisualElement.Remove(se);
                    UIDocument.rootVisualElement.Add(betweenLevels);
                };
                se.SkinButtonAction = st =>
                {
                    GameDataBase.SetSkinType(st);
                    MainCamera.backgroundColor = GameDataBase.BackgroundColour();
                    se.ReSkin();
                    betweenLevels.ReSkin();

                };
                
                UIDocument.rootVisualElement.Add(se);
                

            };
            betweenLevels.MiddleButtonAction = () =>
            {
                Serializer.Apply<SerialHexOutData>(sgd =>
                {
                    var f = XPSystem.DrawGameLevelFromNo(sgd.playerLevel,sgd.playedLevels);
                    ActivateLevel(f.data);
                    levelIndex = f.index;
                    levelId = f.data.Name;
                    levelDiff = f.data.LevelDifficulty;
                });
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
            _activeLevel.PopUpTextAction = s =>
            {
                if (_activePopup is not null)
                {
                    
                    TweenHolder.RemoveTween(_activePopup);
                    _activePopup.ExitAction();
                }
                var n = _activeLevel.FieldFrame.TextPopup(s);
                n.ve.style.opacity = 0f;
                _activePopup = TweenHolder.NewTween(.6f,duringAction: (alpha) =>
                {
                    var a = Math.Clamp(alpha * 3f, 0f, 1f);
                    n.ve.style.opacity = a;
                    n.ve.style.top = n.top - a * 50f;
                },exitAction: () =>
                {
                    _activeLevel.FieldFrame.Remove(n.ve);
                    _activePopup = null;
                });

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
                    var playerLevel = sgd.playerLevel;
                    var oldN = XPSystem.AddXP(playerLevel, sgd.playerXp, oldXP);
                    var newN = XPSystem.AddXP(playerLevel, sgd.playerXp, newXP+oldXP);
                    
                    var oldLevel = oldN.newLevel;
                    var oldXPForBar =(float) oldN.newXp;
                    
                    var levelXP = (float)XPSystem.LevelXp(playerLevel);
                    var levelUp = newN.newLevel > playerLevel; // if it ever increases in level, change here
                    var levelUpWasTheCase = oldLevel > playerLevel; // if it ever increases in level, change here
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

                /*
                var n = _activeLevel.FieldFrame.TextPopup("boing");
                n.ve.style.opacity = 0f;
                TweenHolder.NewTween(.6f,duringAction: (alpha) =>
                {
                    var a = Math.Clamp(alpha * 3f, 0f, 1f);
                    n.ve.style.opacity = a;
                    n.ve.style.top = n.top - a * 50f;
                },exitAction: () =>
                {
                    _activeLevel.FieldFrame.Remove(n.ve);
                });
                */
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
