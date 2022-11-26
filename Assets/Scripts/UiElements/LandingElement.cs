using System;
using DefaultNamespace.GameData;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

namespace DefaultNamespace
{
    public class LandingElement: VisualElement
    {
        private VisualElement _capsuleVisual;
        
        
        
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
            
            
            _capsuleVisual = new VisualElement
            {
                style =
                {
                    width = 264f,
                    height = 90f,
                    position = Position.Absolute,
                    top = 1064f,
                    left = (Constants.UiWidth-264f)*0.5f,
                    backgroundImage = new StyleBackground(QuickAccess.LoadSprite(GameDataBase.LandingCapsule()))
                }
            };
            
            Add(_capsuleVisual);


        }


        /** 
         * alpha starts from 0 and goes to 1
         */
        public void Animator(float thatThingThatStartsFromZeroAndGoesToOne)
        {
            var phaseNo = 5f;
            var thatThingThatStartsFromZeroAndGoesToOne1 = Math.Clamp(thatThingThatStartsFromZeroAndGoesToOne * phaseNo, 0f, 1f);
            var thatThingThatStartsFromZeroAndGoesToOne2 = Math.Clamp(thatThingThatStartsFromZeroAndGoesToOne * phaseNo-1f, 0f, 1f);
            var thatThingThatStartsFromZeroAndGoesToOne3 = Math.Clamp(thatThingThatStartsFromZeroAndGoesToOne * phaseNo-2f, 0f, 1f);
            var thatThingThatStartsFromZeroAndGoesToOne4 = Math.Clamp(thatThingThatStartsFromZeroAndGoesToOne * phaseNo-3f, 0f, 1f);
            var thatThingThatStartsFromZeroAndGoesToOne5 = Math.Clamp(thatThingThatStartsFromZeroAndGoesToOne * phaseNo-4f, 0f, 1f);
            
            
            
            _capsuleVisual.style.left = (Constants.UiWidth - 264f) * 0.5f + 500f * thatThingThatStartsFromZeroAndGoesToOne3;

            //_capsuleVisual.style.opacity = 1f - alpha;
        }
        
    }
}