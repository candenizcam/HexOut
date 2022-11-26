using System;
using Classes;
using DefaultNamespace.GameData;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class SkinPickerElement : VisualElement
    {
        private ButtonClickable _pickButton;
        private Label _pickText;
        public Action<SkinType> SkinPickerAction;
        public SkinPickerElement(int index, float width, float height)
        {
            style.position = Position.Absolute;
            style.width = width;
            style.height = height;
            
            if (index / 2 == 0)
            {
                style.top = 0f;
            }
            else
            {
                style.bottom = 0f;
            }
                
            if (index % 2 == 0)
            {
                style.left = 0f;
            }
            else
            {
                style.right = 0f;
            }
        }

        /**
         * state: 0 normal, 1 pressed, 2 inactive
         */
        public void SetFace(SkinType st, int state) 
        {
            Clear();
            var enabled = (state is 0 or 1);
            _pickButton = new ButtonClickable(GameDataBase.SkinSelectorFaceFromState(state,st),Color.gray, () =>
                {
                    SkinButtonFunction(st);
                    
                        
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
            _pickButton.Disable(!enabled);
            
            _pickText = new Label
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
                text = enabled ? GameDataBase.SkinName( st ): "???"
            };
            Add(_pickButton);
            Add(_pickText);
            
        }

        public void ReSkin()
        {
            
        }

        private void SkinButtonFunction(SkinType st)
        {
            SkinPickerAction(st);
        }
    }
}