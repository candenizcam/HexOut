﻿using System;
using Classes;
using DefaultNamespace.GameData;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class GameFieldFrame : VisualElement
    {
        public Action HomeButtonAction = () => { };
        private ProgressBar _progressBar;
        private float _top;
        private float _width;
        private float _height;
        private HudIndicator _hudIndicator;
        private VisualElement _hudElement;
        private Label _tutorialElement;
        private VisualElement _tutorialBg;
        private ButtonClickable _homeButton;
       
        
        
        public GameFieldFrame(float left, float top, float width, float height)
        {
            _top = top;
            _width = width;
            _height = height;
            this.StretchToParentSize();
            style.flexDirection = FlexDirection.Column;
            style.justifyContent = Justify.Center;
            style.alignItems = Align.Center;
            var frameWidth = width + Constants.FrameSolid * 2f;
            var frameHeight = height + Constants.FrameSolid * 2f;
            var frameLeft = left - Constants.FrameSolid;
            var frameTop = top - Constants.FrameSolid;
            
            
            
                
            
            
            
            var frame = new VisualElement
            {
                style =
                {
                    backgroundImage = QuickAccess.LoadSpriteBg(GameDataBase.LevelBackPath()),
                    width = frameWidth,
                    height = frameHeight,
                    position = Position.Absolute,
                    left = frameLeft,
                    top = frameTop
                }
            };

            var topPart = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0f,
                    height = frameTop+10f,
                    backgroundColor = new StyleColor(GameDataBase.BackgroundColour())
                }
            };
            topPart.StretchToParentWidth();
            
            var bottomPart = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    bottom = 0f,
                    height = Constants.UiHeight -frameTop - frameHeight +10f,
                    backgroundColor = new StyleColor(GameDataBase.BackgroundColour())
                }
            };
            bottomPart.StretchToParentWidth();

            var leftPart = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    left = 0f,
                    height = frameHeight,
                    width = frameLeft+10f,
                    top = frameTop,
                    backgroundColor = new StyleColor(GameDataBase.BackgroundColour())
                }
            };
            
            var rightPart = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    right = 0f,
                    height = frameHeight,
                    top = frameTop,
                    width = frameLeft+10f,
                    backgroundColor = new StyleColor(GameDataBase.BackgroundColour())
                }
            };
            
            
            Add(frame);
            Add(topPart);
            Add(bottomPart);
            Add(rightPart);
            Add(leftPart);


            _hudElement = new VisualElement
            {
                style =
                {
                    width = 1170f,
                    height = 2532
                }
            };
            Add(_hudElement);
            


            _homeButton = new ButtonClickable(GameDataBase.HomeButtonPath(),Color.gray,HomeButtonFunction)
            {
                style =
                {
                    position = Position.Absolute,
                    left = 100f,
                    bottom = 100f,
                    width = 140f,
                    height = 120f,
                    //backgroundImage = QuickAccess.LoadSpriteBg("UI/Game/HomeButton")
                }
            };
            
            _hudElement.Add(_homeButton);

            _progressBar = new ProgressBar(GameDataBase.LevelProgressBarBGPath(),
                GameDataBase.LevelProgressBarPath(), 900f, 100f, 14f, 10f)
            {
                style =
                {
                    position = Position.Absolute,
                    top = 164f,
                    left = 206f
                }
            };


            _hudElement.Add(_progressBar);


            _hudIndicator = new HudIndicator();
            _hudElement.Add(_hudIndicator);

            _tutorialBg = new VisualElement();
            _tutorialBg.StretchToParentSize();
            _tutorialBg.style.backgroundColor = new StyleColor(GameDataBase.BackgroundColour());
            Add(_tutorialBg);
            
            _tutorialElement = new Label("TutorialText")
            {
                style=
                {
                    width = 960f,
                    height = 200f,
                    position = Position.Absolute,
                    top = Constants.UiHeight*0.5f-100f,
                    left = (Constants.UiWidth-960f)*0.5f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/DuzYazıFontu"),
                    fontSize = 76f,
                    unityTextAlign = TextAnchor.UpperCenter,
                    color = GameDataBase.TextColour(),
                    whiteSpace = WhiteSpace.Normal
                }
            };
            Add(_tutorialElement);
            
            
            


        }

        public void SetTutorial(bool isTutorial, string tutorialText = "")
        {
            _hudElement.visible = !isTutorial;
            _tutorialElement.visible = isTutorial;
            _tutorialElement.text = tutorialText;
            _tutorialBg.visible = isTutorial;
        }

        public void TutorialTextAnimation(float alpha)
        {
            var phaseNo = 3f;
            var alpha1 = Math.Clamp(alpha * phaseNo, 0f, 1f);
            var alpha2 = Math.Clamp(alpha * 2f-.5f, 0f, 1f);
            var alpha3 = Math.Clamp(alpha * phaseNo-2f, 0f, 1f);
            
            
            _tutorialElement.style.top =
                (1f - alpha2) * Constants.UiHeight * 0.5f - 100f + (_top + _height + 150f) * alpha2;
            
            _tutorialBg.style.opacity = 1f-alpha3;
        }

        public void SetBar(float f)
        {
            _progressBar.Refill(f);
        }

        private void HomeButtonFunction()
        {
            HomeButtonAction();
        }

        public void SetIndicatorText(string bigText="", string smallText="level", bool levelUp=false)
        {
            _hudIndicator.UpdateText(levelUp ? "UP!" : bigText, smallText);
        }

        public (VisualElement ve, float top) TextPopup(string text)
        {
            var top = _top - 100f;
            
            
            
            var n = new Label(text)
            {
                style=
                {
                    position = Position.Absolute,
                    top = top,
                    unityTextAlign = TextAnchor.UpperCenter,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 72f,
                    color = GameDataBase.TextColour()
                },
            };
            n.StretchToParentWidth();
            Add(n);
            return (n,top);
        }
        
        public void DisableHome(bool b)
        {
            _homeButton.Disable(b,Color.clear);
        }

    }
}