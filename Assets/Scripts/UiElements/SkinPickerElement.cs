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
        private VisualElement _selectedElement;
        public Action<SkinType> SkinPickerAction;
        
        private SkinType? _thisType = null;
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

        public void SetPicked(SkinType st)
        {
            var b = st == _thisType;
            _selectedElement.visible = b;
            _pickButton.style.unityBackgroundImageTintColor = b?new Color(252f / 255f, 228f / 255f, 109f / 255f):Color.white;
        }
        
        /**
         * state: 0 normal, 1 pressed, 2 inactive
         */
        public void SetFace(SkinType st, int state) 
        {
            Clear();
            _thisType = st;
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

            _selectedElement = new VisualElement()
            {
                style =
                {
                    width = 440f,
                    height = 380f,
                    position = Position.Absolute,
                    top = 0f,
                    left = 0f,
                    backgroundImage = QuickAccess.LoadSpriteBg("UI/SkinSelection/Select")
                }
            };
            if (state == 1)
            {
                _pickButton.style.unityBackgroundImageTintColor = new Color(252f / 255f, 228f / 255f, 109f / 255f);
                
            }
            else
            {
                _selectedElement.visible = false;
            }
            
                
            _pickButton.Add(_selectedElement);
            
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
            _pickText.style.color = GameDataBase.TextColour();
        }

        private void SkinButtonFunction(SkinType st)
        {
            
            
            
            SkinPickerAction(st);
            
            
            
        }
    }
}