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
        public Action<int> LeftButtonAction;
        public Action<int> RightButtonAction;
        public Action<SkinType> SkinButtonAction;
        public Action ExitButtonAction;
        private  List<SkinPickerElement> _pickerList = new List<SkinPickerElement>();
        private Action _reSkin = ()=>{};
        private int _activeFour = 0;
        
        
        
        
        public SkinSelectionElement(List<SkinType> activeSkins, SkinType activeSkin)
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


            var singleWidth = (uiFrameWidth - 32f) / 2f;
            for (var i = 0; i < 4; i++)
            {
                var n = new SkinPickerElement(i, singleWidth, 468f);
                _pickerList.Add(n);
                _reSkin += () =>
                {
                    n.ReSkin();
                };
                frame.Add(n);
            }



            
            var activeIndex = allSkins.IndexOf(activeSkin);
            var pickableFours = activeIndex/4;
            
            ChangePickableSkins(pickableFours,activeSkins,activeSkin);


            Add(frame);

        }


        public void ChangePickableSkins(int pickableFours, List<SkinType> enabledSkins, SkinType activeSkin)
        {
            var allSkins = Enum.GetValues(typeof(SkinType)).Cast<SkinType>().ToList();
            _activeFour = pickableFours;
            for (var i = 0; i < 4; i++)
            {
                var ind = pickableFours * 4 + i;
                if (ind >= allSkins.Count)
                {
                    _pickerList[i].Clear();
                    continue;
                }

                var thisSkin = allSkins[ind];
                var state = thisSkin == activeSkin ? 0 : enabledSkins.Contains(thisSkin) ? 1 : 2;
                _pickerList[i].SetFace(thisSkin,state);
                _pickerList[i].SkinPickerAction = (st) =>
                {
                    SkinButtonFunction(thisSkin);
                };
            }
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
            LeftButtonAction(_activeFour);
        }

        public void RightButtonFunction()
        {
            RightButtonAction(_activeFour);
        }
    }
}