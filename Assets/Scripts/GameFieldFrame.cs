using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class GameFieldFrame : VisualElement
    {
        public GameFieldFrame(float left, float top, float width, float height)
        {
            this.StretchToParentSize();
            var s = QuickAccess.LoadSprite("UI/SmallSquare");
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
        }
        
    }
}