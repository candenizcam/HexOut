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
        private ProgressBar _progressBar;
        private Label _bigText;
        private Label _smallText;
        private float _top;
        private float _width;
       
        
        
        public GameFieldFrame(float left, float top, float width, float height)
        {
            _top = top;
            _width = width;
            this.StretchToParentSize();
            style.flexDirection = FlexDirection.Column;
            style.justifyContent = Justify.Center;
            style.alignItems = Align.Center;
            var s = QuickAccess.LoadSprite("UI/LevelBack");
            var frameWidth = width + Constants.FrameSolid * 2f;
            var frameHeight = height + Constants.FrameSolid * 2f;
            var frameLeft = left - Constants.FrameSolid;
            var frameTop = top - Constants.FrameSolid;
            
            
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


            var hudFrame = new VisualElement();
            hudFrame.style.width = 1170f;
            hudFrame.style.height = 2532;
            Add(hudFrame);
            


            var homeButton = new ButtonClickable("UI/Game/HomeButton",Color.gray,HomeButtonFunction)
            {
                style =
                {
                    position = Position.Absolute,
                    left = 100f,
                    bottom = 100f,
                    width = 140f,
                    height = 140f,
                    backgroundImage = QuickAccess.LoadSpriteBg("UI/Game/HomeButton")
                }
            };
            
            hudFrame.Add(homeButton);

            _progressBar = new ProgressBar("UI/Game/LevelProgressBarBG",
                "UI/Game/LevelProgressBar", 900f, 100f, 14f, 10f)
            {
                style =
                {
                    position = Position.Absolute,
                    top = 164f,
                    left = 206f
                }
            };


            hudFrame.Add(_progressBar);


            var indicator = new VisualElement
            {
                style =
                {
                    width = 220f,
                    height = 190f,
                    position = Position.Absolute,
                    top = 132f,
                    left = 63f,
                    backgroundImage = QuickAccess.LoadSpriteBg("UI/Game/LevelIndicator")
                }
            };

            _smallText = new Label()
            {
                style=
                {
                    position = Position.Absolute,
                    top = 24,
                    unityTextAlign = TextAnchor.UpperCenter,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 28f,
                    color = Constants.UiTextColour()
                },
                text = "level"
            };
            _smallText.StretchToParentWidth();
            
            _bigText = new Label()
            {
                style=
                {
                    position = Position.Absolute,
                    top = 50,
                    unityTextAlign = TextAnchor.UpperCenter,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 72f,
                    color = Constants.UiTextColour()
                },
                text = "1293"
            };
            _bigText.StretchToParentWidth();
            indicator.Add(_bigText);
            indicator.Add(_smallText);
            
            hudFrame.Add(indicator);
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
            if (levelUp)
            {
                _smallText.text = smallText;
                _bigText.text = "UP!";
            }
            else
            {
                _bigText.text = bigText;
                _smallText.text = smallText;
            }
            
            

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
                    color = Constants.UiTextColour()
                },
            };
            n.StretchToParentWidth();
            Add(n);
            return (n,top);
        }

    }
}