using Classes;
using DefaultNamespace.GameData;
using Punity.ui;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class HomeMenuElement: VisualElement
    {
        public HomeMenuElement()
        {
            this.StretchToParentSize();
            style.backgroundColor = new StyleColor(GameDataBase.BackgroundColour().WithAlpha(.5f));
            
            
            // kod buraya
            var back = new ButtonClickable(imagePath:GameDataBase.BackPath(),Color.gray,BackFunction)
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
            
            var replay = new ButtonClickable(imagePath:GameDataBase.ReplayPath(),Color.gray,ReplayFunction)
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
            
            var skip = new ButtonClickable(imagePath:GameDataBase.SkipPath(),Color.gray,SkipFunction)
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
            
            var replayText = new Label("Replay")
            {
                style =
                {
                    width = 138f,
                    height = 84f,
                    position = Position.Absolute,
                    bottom = 257f,
                    left = 221f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 64f,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    color = GameDataBase.TextColour()
                    
                }
            };
            
            var returnText = new Label("Return")
            {
                style =
                {
                    width = 221f,
                    height = 89f,
                    position = Position.Absolute,
                    bottom = 117f,
                    left = 221f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 64f,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    color = GameDataBase.TextColour()
                    
                }
            };

            Add(back);
            Add(skip);
            Add(skipText);
            Add(replay);
            Add(replayText);
            Add(returnText);
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