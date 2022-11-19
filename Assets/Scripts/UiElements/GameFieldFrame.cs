using System;
using Classes;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class GameFieldFrame : VisualElement
    {
        public Action HomeButtonAction = () => { };

        public GameFieldFrame(float left, float top, float width, float height)
        {
            this.StretchToParentSize();
            var s = QuickAccess.LoadSprite("UI/LevelBack");
            var frameWidth = width + Constants.FrameSolid * 2f;
            var frameHeight = height + Constants.FrameSolid * 2f;
            var frameLeft = left - Constants.FrameSolid;
            var frameTop = top - 50f;
            
            
            var frame = new VisualElement
            {
                style =
                {
                    backgroundImage = new StyleBackground(s),
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
                    backgroundColor = new StyleColor(Constants.BackgroundColour)
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
                    backgroundColor = new StyleColor(Constants.BackgroundColour)
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
                    backgroundColor = new StyleColor(Constants.BackgroundColour)
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
                    backgroundColor = new StyleColor(Constants.BackgroundColour)
                }
            };
            
            
            Add(frame);
            Add(topPart);
            Add(bottomPart);
            Add(rightPart);
            Add(leftPart);


            var homeButton = new ButtonClickable("UI/BetweenLevels/HomeButton",Color.gray,HomeButtonFunction)
            {
                style =
                {
                    position = Position.Absolute,
                    left = 100f,
                    bottom = 100f,
                    width = 140f,
                    height = 140f,
                    backgroundImage = QuickAccess.LoadSpriteBg("UI/BetweenLevels/HomeButton")
                }
            };
            
            Add(homeButton);

            var progressBar = new ProgressBar("UI/BetweenLevels/LevelProgressBarBG",
                "UI/BetweenLevels/LevelProgressBar", 900f, 100f, 14f, 10f);
            progressBar.style.position = Position.Absolute;
            progressBar.style.top = 164f;
            progressBar.style.left = 228f;
            
            
            Add(progressBar);


            var indicator = new VisualElement
            {
                style =
                {
                    width = 220f,
                    height = 190f,
                    position = Position.Absolute,
                    top = 132f,
                    left = 85f,
                    backgroundImage = QuickAccess.LoadSpriteBg("UI/BetweenLevels/LevelIndicator")
                }
            };
            Add(indicator);
        }


        private void HomeButtonFunction()
        {
            HomeButtonAction();
        }
        
    }
}