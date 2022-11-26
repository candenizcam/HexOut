using DefaultNamespace.GameData;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class HudIndicator : VisualElement
    {
        private Label _bigText;
        private Label _smallText;

        public HudIndicator()
        {
            style.width = 220f;
            style.height = 190f;
            style.position = Position.Absolute;
            style.top = 132f;
            style.left = 63f;
            style.backgroundImage = QuickAccess.LoadSpriteBg(GameDataBase.LevelIndicatorPath());
            
            _smallText = new Label()
            {
                style=
                {
                    position = Position.Absolute,
                    top = 24,
                    unityTextAlign = TextAnchor.UpperCenter,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 28f,
                    color = GameDataBase.TextColour()
                },
                text = "level"
            };
            _smallText.StretchToParentWidth();
            
            _bigText = new Label()
            {
                style=
                {
                    position = Position.Absolute,
                    top = 50,
                    unityTextAlign = TextAnchor.UpperCenter,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 72f,
                    color = GameDataBase.TextColour()
                },
                text = "1293"
            };
            _bigText.StretchToParentWidth();
            Add(_bigText);
            Add(_smallText);
        }

        public void UpdateText(string bigText, string smallText)
        {
            _bigText.text = bigText;
            _smallText.text = smallText;
        }
    }
}