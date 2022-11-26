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
    public partial class TestMainScript : MainScript
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
        public bool unlockAllSkins;
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

            if (Application.isEditor && unlockAllSkins)
            {
                Serializer.Apply<SerialHexOutData>(sgd =>
                {
                    sgd.activeSkins = Enum.GetValues(typeof(SkinType)).Cast<SkinType>().ToList();
                });
            }

            
           
            Serializer.Apply<SerialHexOutData>(sgd =>
            {
                GameDataBase.GetSkinType();

                var d = GameDataBase.FirstLevelData(sgd.playedLevels);

                if (d is null)
                {
                    var f = XPSystem.DrawGameLevelFromNo(sgd.playerLevel,sgd.playedLevels);
                    ActivateLevel(f.data); 
                    levelIndex = f.index;
                    levelId = f.data.Name;
                    levelDiff = f.data.LevelDifficulty;
                }
                else
                {
                    var ld = (LevelData) d;
                    ActivateLevel(ld,true);
                }
                
                
            });
           

            //var le = new LandingElement();
            //le.StretchToParentSize();
            //UIDocument.rootVisualElement.Add(le);

            MainCamera.backgroundColor = GameDataBase.BackgroundColour();
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
                var se = new SkinSelectionElement(sgdn.activeSkins,sgdn.activeSkin);


                se.LeftButtonAction = (activeFour) =>
                {
                    Serializer.Apply<SerialHexOutData>(sgd =>
                    {
                        if (activeFour <= 0) return;
                        se.ChangePickableSkins(activeFour-1,sgd.activeSkins,sgd.activeSkin);
                    });
                };
                se.RightButtonAction = (activeFour) =>
                {
                    
                    Serializer.Apply<SerialHexOutData>(sgd =>
                    {
                        if (activeFour >= 5/4) return;
                        se.ChangePickableSkins(activeFour+1,sgd.activeSkins,sgd.activeSkin);
                    });
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
