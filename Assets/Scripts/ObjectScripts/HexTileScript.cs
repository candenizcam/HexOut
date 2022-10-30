using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<SpriteRenderer> obstacles;
        private int[] _activeObstacles;
        [NonSerialized] public int R = 0;
        [NonSerialized] public int C = 0;


        public void ActivateObstacles(int[] actives)
        {
            _activeObstacles = actives;
            for (int i = 0; i < 6; i++)
            {
                obstacles[i].enabled = actives.Any(x => x == i);
                obstacles[i].color = Constants.ObstacleColour;
            }
            
        }
        
        
        public void Paint(Color c)
        {
            hexRenderer.color = c;
        }
        
        protected override void AwakeFunction()
        {
            
            ActivateObstacles(new int[]{});
            
        }
    }
}