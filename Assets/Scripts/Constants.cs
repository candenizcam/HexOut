using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace DefaultNamespace
{
    public static class Constants
    {
        public const bool DeployMode = true; // true before deploy 
        
        
        public const float WorldHeight = 2532f / 200f; // orthographic size for the camera
         
        
        public static float UiWidth => 2532f / (float)Screen.height* (float)Screen.width;
        public const float UiHeight = 2532f;
        
        public static float UnsafeTopUi => (Screen.height - Screen.safeArea.yMax)/ Screen.height * Constants.UiHeight;
        public static float UnsafeBottomUi => Screen.safeArea.yMin/ Screen.height * Constants.UiHeight;
        public static float UnsafeLeftUi => Screen.safeArea.xMin/ Screen.width * Constants.UiWidth;
        public static float UnsafeRightUi => (Screen.width -  Screen.safeArea.xMax)/ Screen.width * Constants.UiWidth;

        public const float WorldPickDistance = 1f;

        public static readonly Color ObstacleColour = new Color(62f/255f, 0f/255f, 160f/255f);
        public static readonly Color[] CapsuleColours = new Color[]
        {
            new Color(254f/255f, 1f/255f, 74f/255f),
            new Color(255f/255f, 109f/255f, 36f/255f),
            new Color(255f/255f, 243f/255f, 18f/255f),
            new Color(10f/255f, 232f/255f, 105f/255f),
            new Color(0f/255f, 183f/255f, 255f/255f),
            new Color(74f/255f, 0f/255f, 255f/255f),
        };
    }
}