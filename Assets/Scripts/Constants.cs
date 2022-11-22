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
        public static readonly float FrameSolid = 112f;
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
        

        public static Color UiTextColour()
        {
            return new Color(70f/255f,70f/255f,70f/255f);
        }
        

        public static readonly Color BackgroundColour = new Color(217f / 255f, 221f / 255f, 232f / 255f);
    }
}