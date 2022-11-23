using System;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DefaultNamespace.GameData;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class SkinSelectionElement: VisualElement
    {
        public Action LeftButtonAction;
        public Action RightButtonAction;
        public Action<SkinType> SkinButtonAction;
        public Action ExitButtonAction;

        private Action _reSkin = ()=>{};
        
        
        
        public SkinSelectionElement(SkinType[] activeSkins, SkinType activeSkin)
        {
            this.StretchToParentSize();
            var allSkins = Enum.GetValues(typeof(SkinType)).Cast<SkinType>().ToList();
            var leftRightVisible = allSkins.Count > 4;
            
            

            if (leftRightVisible)
            {
                var leftButton = new ButtonClickable(GameDataBase.ArrowLeftPath(),Color.gray,LeftButtonFunction)
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = 32f,
                        top = (Constants.UiHeight - 190f) / 2f
                    }
                };

                var rightButton = new ButtonClickable(GameDataBase.ArrowRightPath(),Color.gray,RightButtonFunction)
                {
                    style =
                    {
                        position = Position.Absolute,
                        right = 32f,
                        top = (Constants.UiHeight - 190f) / 2f
                    }
                };
                Add(leftButton);
                Add(rightButton);

                _reSkin += () =>
                {
                    leftButton.ChangeImage(GameDataBase.ArrowLeftPath());
                    rightButton.ChangeImage(GameDataBase.ArrowRightPath());

                };

            }
            

            var uiFrameLeft = 72f + 32f + 32f;
            var uiFrameWidth = Constants.UiWidth - 2f * uiFrameLeft;
            var uiFrameHeight = 1000f;
            var uiFrameTop = (Constants.UiHeight - uiFrameHeight) / 2f;

            var exitButton = new ButtonClickable(GameDataBase.ExitPath(),Color.gray,ExitButtonFunction)
            {
                style =
                {
                    position = Position.Absolute,
                    right = 120f,
                    top = 140f
                }
            };
            Add(exitButton);
            
            
            var frame = new VisualElement()
            {
                style =
                {
                    width = uiFrameWidth,
                    height = uiFrameHeight,
                    position = Position.Absolute,
                    left = uiFrameLeft,
                    top = uiFrameTop
                }

            };

            var topText = new Label("Change the look")
            {
                style =
                {
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 96f,
                    width = uiFrameWidth,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = GameDataBase.TextColour(),
                    height = 150f,
                    top = -150f,
                    left = 0f,
                    position = Position.Absolute
                }
            };
            frame.Add(topText);
            
            _reSkin += () =>
            {
                topText.style.color = GameDataBase.TextColour();
                exitButton.ChangeImage(GameDataBase.ExitPath());

            };

            

            var activeIndex = allSkins.IndexOf(activeSkin);


            var singleWidth = (uiFrameWidth - 32f) / 2f;
            var l = new List<ButtonClickable>();
            var s = activeIndex - (activeIndex % 4);
            for (int i = 0; i < 4; i++)
            {
                if(s+i>=allSkins.Count) break;
                var thisSkin = allSkins[s + i];

                var n = new VisualElement()
                {
                    style=
                    {
                        width = singleWidth,
                        height = 468f,
                        position = Position.Absolute,
                        
                    }
                };

                if (i / 2 == 0)
                {
                    n.style.top = 0f;
                }
                else
                {
                    n.style.bottom = 0f;
                }
                
                if (i % 2 == 0)
                {
                    n.style.left = 0f;
                }
                else
                {
                    n.style.right = 0f;
                }
                


                var pickButton = new ButtonClickable(GameDataBase.HexTilePath(thisSkin),Color.gray, () =>
                    {
                        SkinButtonFunction(thisSkin);
                        
                    }) // change here to actual skin path
                {
                    style =
                    {
                        width = 440f,
                        height = 380f,
                        position = Position.Absolute,
                        top = 0f,
                        left = 0f,
                        
                    }
                };
                pickButton.ClickAction += () =>
                {
                    pickButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(0.5f,0.6f,0.55f));
                };
                l.Add(pickButton);

                // warning, following is placeholder
                if (thisSkin == activeSkin)
                {
                    // active
                    pickButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(0.5f,0.6f,0.55f));
                }else if (!activeSkins.Contains(thisSkin))
                {
                    // locked
                    pickButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(0.05f,0.1f,0.05f));
                    pickButton.Disable(true);
                }
                else
                {
                    
                }


                var pickText = new Label
                {
                    style =
                    {
                        unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                        fontSize = 48f,
                        unityTextAlign = TextAnchor.MiddleCenter,
                        color = GameDataBase.TextColour(),
                        height = 60f,
                        bottom = 0f,
                        left = 0f,
                        position = Position.Absolute
                    },
                    text = GameDataBase.SkinName(thisSkin)
                };
                
                _reSkin += () =>
                {
                    pickText.style.color = GameDataBase.TextColour();
                    foreach (var buttonClickable in l)
                    {
                        if (!buttonClickable.DisableButton)
                        {
                            buttonClickable.style.unityBackgroundImageTintColor = Color.white;
                        }
                    }
                };
                
                pickText.StretchToParentWidth();
                n.Add(pickButton);
                n.Add(pickText);


                frame.Add(n);
            }
            
            Add(frame);

        }

        public void ReSkin()
        {
            _reSkin();
        }

        private void ExitButtonFunction()
        {
            ExitButtonAction();
        }

        private void SkinButtonFunction(SkinType st)
        {
            SkinButtonAction(st);
        }


        private void LeftButtonFunction()
        {
            LeftButtonAction();
        }

        public void RightButtonFunction()
        {
            RightButtonAction();
        }
    }
}