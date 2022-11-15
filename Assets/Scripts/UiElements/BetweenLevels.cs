using System;
using Classes;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class BetweenLevels: VisualElement
    {
        public Action LeftButtonAction = () =>{ };
        public Action MiddleButtonAction = () =>{ };
        public Action RightButtonAction = () =>{ };


        public BetweenLevels()
        {
            this.StretchToParentSize();

            style.justifyContent = Justify.Center;
            style.alignItems = Align.Center;
            
            var progressBg = new VisualElement();
            progressBg.style.width = 1000f;
            progressBg.style.height = 624f;
            progressBg.style.backgroundImage = new StyleBackground(QuickAccess.LoadSprite("UI/BetweenLevels/LevelBarBack"));

            var leftButton = new ButtonClickable(imagePath:"UI/BetweenLevels/Skins",Color.gray,LeftButtonFunction)
            {
                style =
                {
                    width = 264f,
                    height = 164f,
                    position = Position.Absolute,
                    bottom = -164f - 16f,
                    left = 0f
                }
            };
            
            var midButton = new ButtonClickable(imagePath:"UI/BetweenLevels/Next",Color.gray,MiddleButtonFunction)
            {
                style =
                {
                    width = 440f,
                    height = 259f,
                    position = Position.Absolute,
                    top = 592f,
                    left = 280f,
                }
            };
            
            var rightButton = new ButtonClickable(imagePath:"UI/BetweenLevels/DoubleXP",Color.gray,RightButtonFunction)
            {
                style =
                {
                    width = 264f,
                    height = 164f,
                    position = Position.Absolute,
                    bottom = -164f - 16f,
                    right = 0f,
                }
            };

            var congratulationsText = new VisualElement()
            {
                style =
                {
                    width = 1000f,
                    height = 134,
                    top = -175f,
                    left = 0f,
                    position = Position.Absolute,
                    backgroundImage = new StyleBackground(QuickAccess.LoadSprite("UI/BetweenLevels/CONGRATULATIONS!"))
                }
            };


            var progressBar = new ProgressBar("UI/BetweenLevels/LevelBarBack","UI/BetweenLevels/LevelBarFront",400f,60f, 9f,7f)
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = 80f,
                        top = 347f
                    }
                };
            progressBar.Refill(.9f);


            var textColor = new Color(112f/255f,112f/255f,112f/255f);

            var levelProgress = new Label("Level Progress")
            {
                style =
                {
                    width = 440f,
                    height = 68f,
                    position = Position.Absolute,
                    left = 60f,
                    top = 146f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/DuzYazıFontu"),
                    fontSize = 48f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = textColor
                }
            };
            
            var progressNo = new Label("142")
            {
                style =
                {
                    width = 440f,
                    height = 140f,
                    position = Position.Absolute,
                    left = 60f,
                    top = 213f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 96f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = textColor
                }
            };
            
            var newLookCounter = new Label("4 levels to new look")
            {
                style =
                {
                    width = 440f,
                    height = 68f,
                    position = Position.Absolute,
                    left = 60f,
                    top = 434f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/DuzYazıFontu"),
                    fontSize = 32f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = textColor
                }
            };
            
            
            var rightStuffText = new Label("+142 xp\n456/1000\nto next level")
            {
                style =
                {
                    width = 200f,
                    height = 138f,
                    position = Position.Absolute,
                    top = 243f,
                    right = 150,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/DuzYazıFontu"),
                    fontSize = 32f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = textColor
                }
            };


            progressBg.Add(leftButton);
            progressBg.Add(midButton);
            progressBg.Add(rightButton);
            progressBg.Add(congratulationsText);
            progressBg.Add(progressBar);
            progressBg.Add(levelProgress);
            progressBg.Add(progressNo);
            progressBg.Add(newLookCounter);
            progressBg.Add(rightStuffText);
            Add(progressBg);
        }

        private void LeftButtonFunction()
        {
            LeftButtonAction();
        }

        private void MiddleButtonFunction()
        {
            MiddleButtonAction();
        }

        private void RightButtonFunction()
        {
            RightButtonAction();
        }
    }
}