using System;
using DefaultNamespace.GameData;
using Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public partial class TestMainScript
    {
        
        
        private void ActivateLevel(LevelSeedData seed)
        {
            var d = LevelGenerator.GenerateSeededLevel(seed);
            d.Name = seed.Name;
            ActivateLevel(d);
        }



        

        /** This one starts from a seed and generates the relevant level
         * seed contains all the relevant data, including row & col
         * grid is initiated here too
         */
        private void ActivateLevel(LevelData d, bool tutorialLevel=false)
        {
            _activeLevel = GameLevelScript.Instantiate();
            
            
            //var d = LevelGenerator.GenerateRawFrame(seed,2);
            _activeLevel.SetGameLevelInfo(d);
            _activeLevel.SetGrid(MainCamera,d.Row,d.Col, d.ObstacleDatas);
            
            
            
            _activeLevel.FieldFrame.SetTutorial(tutorialLevel,GameDataBase.TutorialText(d.Name));
            if (tutorialLevel)
            {
                TweenHolder.NewTween(2f, duringAction: (alpha) =>
                {
                    _activeLevel.FieldFrame.TutorialTextAnimation(alpha);
                });
            }

            _activeLevel.HomeButtonAction = () =>
            {
                var n = new HomeMenuElement();
                n.BackAction = () =>
                {
                    UIDocument.rootVisualElement.Remove(n);
                    _activeLevel.FieldFrame.DisableHome(false);
                };
                n.ReplayAction = () =>
                {
                    UIDocument.rootVisualElement.Remove(_activeLevel.FieldFrame);
                    Destroy(_activeLevel.gameObject);
                    UIDocument.rootVisualElement.Remove(n);
                    ActivateLevel(_activeLevel.ThisLevelData);
                    
                };
                n.SkipAction = () =>
                {
                    UIDocument.rootVisualElement.Remove(n);
                    UIDocument.rootVisualElement.Remove(_activeLevel.FieldFrame);
                    Destroy(_activeLevel.gameObject);
                    Serializer.Apply<SerialHexOutData>(sgd =>
                    {
                        var f = XPSystem.DrawGameLevelFromNo(sgd.playerLevel,sgd.playedLevels);
                        ActivateLevel(f.data);
                        levelIndex = f.index;
                        levelId = f.data.Name;
                        levelDiff = f.data.LevelDifficulty;
                    });
                    
                };


                UIDocument.rootVisualElement.Add(n);
                _activeLevel.FieldFrame.DisableHome(true);


                


            };
            _activeLevel.SetCapsules(d.CapsuleDatas);
            _activeLevel.AddTweenAction = (tween, delay) =>
            {
                TweenHolder.NewTween(tween, delay);
            };
            
            _activeLevel.PopUpTextAction = PopupTextFunction;
            

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
                TweenHolder.RemoveTween(_activePopup);
                _activePopup.ExitAction();
                if (tutorialLevel)
                {
                    TutorialLevelDoneFunction(lcd.LevelId);
                }
                else
                {
                    GameLevelDoneFunction(lcd);
                }
            };
            
            _activeLevel.CapsuleRemovedAction = CapsuleRemovedFunction;
            UIDocument.rootVisualElement.Add(_activeLevel.FieldFrame);
            Serializer.Apply<SerialHexOutData>(sgd =>
                {
                    var levelXP = (float)XPSystem.LevelXp(sgd.playerLevel);
                    _activeLevel.FieldFrame.SetBar(sgd.playerXp/levelXP);
                    _activeLevel.FieldFrame.SetIndicatorText(bigText:$"{sgd.playerLevel}");
                }
            );
            

        }
        
        
        
        private void PopupTextFunction(string s)
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
        }

        private void TutorialLevelDoneFunction(string playedId)
        {
            UIDocument.rootVisualElement.Remove(_activeLevel.FieldFrame);
            Destroy(_activeLevel.gameObject);
            Serializer.Apply<SerialHexOutData>( sgd =>
            {
                sgd.playedLevels.Add(playedId);
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
        }

        private void GameLevelDoneFunction(LevelCompleteData lcd)
        {
            Serializer.Apply<SerialHexOutData>( sgd =>
            {
                ActivateBetweenLevels(sgd,lcd);
            });
                
                
            UIDocument.rootVisualElement.Remove(_activeLevel.FieldFrame);
            Destroy(_activeLevel.gameObject);
        }

        private void CapsuleRemovedFunction(int oldXP, int newXP)
        {
            Serializer.Apply<SerialHexOutData>( sgd =>
            {
                var playerLevel = sgd.playerLevel;
                var oldN = XPSystem.AddXP(playerLevel, sgd.playerXp, oldXP);
                var newN = XPSystem.AddXP(playerLevel, sgd.playerXp, newXP+oldXP);
                    
                var oldLevel = oldN.newLevel;
                var oldXpForBar =(float) oldN.newXp;
                    
                var levelXp = (float)XPSystem.LevelXp(playerLevel);
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
                            _activeLevel.FieldFrame.SetBar(oldXpForBar*(1f-alpha)/levelXp + alpha );
                            
                        }
                        else
                        {
                            _activeLevel.FieldFrame.SetBar((oldXpForBar*(1f-alpha) + (float)alpha*newN.newXp)/levelXp );
                        }
                        
                    },exitAction: () =>
                    {
                        _activeLevel.FieldFrame.SetIndicatorText(bigText:$"{newN.newLevel}",levelUp:levelUp);
                    });
                }
            });
        }
    }
}