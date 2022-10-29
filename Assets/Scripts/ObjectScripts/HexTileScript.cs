using System;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class HexTileScript: WorldObject
    {
        public SpriteRenderer hexRenderer;
        public static readonly float SideLength = 2f;
        public static float Height = SideLength * 2f;
        public static float Width = SideLength * .866f;
        [NonSerialized] public int R = 0;
        [NonSerialized] public int C = 0;


        public void Paint(Color c)
        {
            hexRenderer.color = c;
        }
        
        protected override void AwakeFunction()
        {
            
        }
    }
}