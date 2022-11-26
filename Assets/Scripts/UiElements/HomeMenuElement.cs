﻿using Classes;
using DefaultNamespace.GameData;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class HomeMenuElement: VisualElement
    {
        public HomeMenuElement()
        {
            // kod buraya
            var back = new ButtonClickable(imagePath:GameDataBase.SkinsPath(),Color.gray,BackFunction)
            {
                style =
                {
                    width = 140f,
                    height = 120f,
                    position = Position.Absolute,
                    bottom = 102f,
                    left = 64f
                }
            };
            
            var replay = new ButtonClickable(imagePath:GameDataBase.SkinsPath(),Color.gray,ReplayFunction)
            {
                style =
                {
                    width = 110f,
                    height = 95f,
                    position = Position.Absolute,
                    bottom = 381f,
                    left = 79f
                }
            };
            
            var skip = new ButtonClickable(imagePath:GameDataBase.SkinsPath(),Color.gray,SkipFunction)
            {
                style =
                {
                    width = 110f,
                    height = 95f,
                    position = Position.Absolute,
                    bottom = 254f,
                    left = 79f
                }
            };
            
            var skipText = new Label("Skip")
            {
                style =
                {
                    width = 138f,
                    height = 84f,
                    position = Position.Absolute,
                    bottom = 384f,
                    left = 221f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 64f,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    color = GameDataBase.TextColour()
                    
                }
            };

        }
        

        private void BackFunction()
        {
            
        }
        
        private void ReplayFunction()
        {
            
        }
        
        private void SkipFunction()
        {
            
        }
        
    }
}