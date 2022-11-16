using System;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class ProgressBar: VisualElement
    {
        private float _inWidth;
        private float _fill;
        private VisualElement _inBar;
        
        public ProgressBar(string outPath, string inPath, float outWidth, float outHeight, float horizontalDiff, float verticalDiff,float fill = 0f)
        {
            var outBar = new VisualElement()
            {
                style =
                {
                    backgroundImage = QuickAccess.LoadSpriteBg(outPath),
                    width = outWidth,
                    height = outHeight

                }
            };

            _inWidth = outWidth - horizontalDiff * 2f;
            _fill = fill;
            _inBar = new VisualElement()
            {
                style =
                {
                    backgroundImage = QuickAccess.LoadSpriteBg(inPath),
                    width = _inWidth*_fill,
                    height = outHeight - verticalDiff*2f,
                    position = Position.Absolute,
                    left = horizontalDiff,
                    top = verticalDiff
                }
            };
            
            Add(outBar);
            outBar.Add(_inBar);
        }

        public void Refill(float fill)
        {
            _fill = Math.Clamp(fill,0f,1f);
            if (fill is < 0f or > 1f)
            {
                Debug.LogWarning("refill is out of range");
            }
            _inBar.style.width = _inWidth * _fill;

        }
        
        
    }
}