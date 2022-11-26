using DefaultNamespace.GameData;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

namespace DefaultNamespace
{
    public class LandingElement: VisualElement
    {
        public LandingElement()
        {
            //the code'u buraya yazıcaksın
            style.backgroundColor = GameDataBase.BackgroundColour();
            
            // bu bi yazı
            var aText = new Label("SUCCESS!")
            {
                style =
                {
                    // ebat
                    width = 1000f,
                    height = 134,
                    // pozisyon bilgisi
                    position = Position.Absolute,
                    top = 500f,
                    left = (Constants.UiWidth-1000f)*0.5f,
                    //yazı bilgisi
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 96f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    //color yazı, backgroundColor bg rengi, its dumb, i blame css
                    color = GameDataBase.TextColour()
                    
                }
            };
            
            
            Add(aText);
            
            
            var aVisual = new VisualElement
            {
                style =
                {
                    width = 1000f,
                    height = 624f,
                    backgroundImage = new StyleBackground(QuickAccess.LoadSprite(GameDataBase.ProgressBGPath()))
                }
            };
            
            Add(aVisual);

            

        }
        
    }
}