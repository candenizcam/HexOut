using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Punity;
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
        public List<SpriteRenderer> doubleObstacles;
        private (int direction, int length)[] _activeObstacles;
        [NonSerialized] public int R = 0;
        [NonSerialized] public int C = 0;


        public void ActivateObstacles((int direction, int length)[] actives)
        {
            SkinType st  =SkinType.Simple;
            Serializer.Apply<SerialHexOutData>(sgd =>
            {
                st = sgd.activeSkin;
            });
            _activeObstacles = actives;
            

            var o = GameDataBase.ObstacleColour(st);
            for (int i = 0; i < 6; i++)
            {
                obstacles[i].enabled = actives.Any(x => x.direction == i && x.length==1);
                obstacles[i].color = o;
                doubleObstacles[i].enabled = actives.Any(x =>x.direction == i && x.length==2);
                doubleObstacles[i].color = o;
            }
            
        }
        
        
        
        
        public void Paint(Color c)
        {
            hexRenderer.color = c;
        }
        
        protected override void AwakeFunction()
        {
            
            ActivateObstacles(new (int direction, int length)[]{});
            
        }
    }
}