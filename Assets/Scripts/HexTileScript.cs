using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class HexTileScript: WorldObject
    {
        public SpriteRenderer hexRenderer;
        public static readonly float SideLength = 1f;
        public static float Height = SideLength * 2f;
        public static float Width = SideLength * .866f;


        public void Paint(Color c)
        {
            hexRenderer.color = c;
        }
        
        protected override void AwakeFunction()
        {
            
        }
    }
}