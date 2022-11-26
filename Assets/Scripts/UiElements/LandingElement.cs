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
            var swipeTitle = new Label("Swipe the capsules!")
            {
                style =
                {
                    // ebat
                    width = 956f,
                    height = 134,
                    // pozisyon bilgisi
                    position = Position.Absolute,
                    top = 1200f,
                    left = (Constants.UiWidth-956f)*0.5f,
                    //yazı bilgisi
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 96f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    //color yazı, backgroundColor bg rengi, its dumb, i blame css
                    color = GameDataBase.TextColour()
                    
                }
            };
            
            
            Add(swipeTitle);
            
            var swipeText = new Label("Clear the area by sending \n capsules away from the screen. \n Beware of the obstacles!")
            {
                style =
                {
                    // ebat
                    width = 834f,
                    height = 209,
                    // pozisyon bilgisi
                    position = Position.Absolute,
                    top = 1333f,
                    left = (Constants.UiWidth-834f)*0.5f,
                    //yazı bilgisi
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 48f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    //color yazı, backgroundColor bg rengi, its dumb, i blame css
                    color = GameDataBase.TextColour()
                    
                }
            };
            
            Add(swipeText);
            
            
            var aCapsule = new VisualElement
            {
                style =
                {
                    width = 264f,
                    height = 90f,
                    backgroundImage = new StyleBackground(QuickAccess.LoadSprite(GameDataBase.LandingCapsule()))
                }
            };
            
            Add(aCapsule);


        }
        
    }
}