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

        //public static readonly Vector2 Frame = new Vector2(946,2048);
        //public static readonly Vector2 FrameInner = new Vector2(150,510);
        public static readonly float FrameSolid = 50f;
        public static readonly float FrameSlice = 100f;

        public static float GameFieldSide => FrameSolid;
        public static float GameFieldTop => FrameSolid+300f;
        public static float GameFieldBottom => FrameSolid + 300f;
        
        public static float UnsafeTopUi => (Screen.height - Screen.safeArea.yMax)/ Screen.height * Constants.UiHeight;
        public static float UnsafeBottomUi => Screen.safeArea.yMin/ Screen.height * Constants.UiHeight;
        public static float UnsafeLeftUi => Screen.safeArea.xMin/ Screen.width * Constants.UiWidth;
        public static float UnsafeRightUi => (Screen.width -  Screen.safeArea.xMax)/ Screen.width * Constants.UiWidth;

        public const float WorldPickDistance = 1f;
        public const float SprayMovementPerTile = .2f;
        public const float BetweenLevelsTime = 2.5f;

        public static readonly Color ObstacleColour = new Color(62f/255f, 0f/255f, 160f/255f);
        public static readonly Color[] CapsuleColours = new Color[]
        {
            new Color(0.996f, 0.004f, 0.29f),
            new Color(1f, 0.427f, 0.141f ),
            new Color(1f, 0.953f, 0.071f),
            new Color(0.039f, 0.91f, 0.412f),
            new Color(0f, 0.718f, 1f),
            new Color(0.29f, 0f, 1f),
            new Color(0.243f, 0f, 0.627f ),
        };

        public static readonly Color[] CapsuleDarkColours = new Color[]
        {
            new Color(0.808f, 0f, 0.271f),
            new Color(0.949f, 0.29f, 0f),
            new Color(0.878f, 0.722f, 0.071f),
            new Color(0.039f, 0.757f, 0.329f),
            new Color(0.008f, 0.58f, 0.757f),
            new Color(0.137f, 0f, 0.71f),
            new Color(0.145f, 0f, 0.427f),
        };
        /**
         * Red: light: 0.996f, 0.004f, 0.29f dark: 0.808f, 0f, 0.271f
            Orange: light: 1, 0.427, 0.141 dark: 0.949, 0.29, 0
Yellow: light: 1, 0.953, 0.071 dark: 0.878, 0.722, 0.071
Green: light: 0.039, 0.91, 0.412 dark: 0.039, 0.757, 0.329
Blue: light: 0, 0.718, 1 dark: 0.008, 0.58, 0.757
Navy: light: 0.29, 0, 1 dark: 0.137, 0, 0.71
Purple: light: 0.243, 0, 0.627 dark: 0.145, 0, 0.427
         */
        
        

        public static readonly Color BackgroundColour = new Color(217f / 255f, 221f / 255f, 232f / 255f);
    }
}