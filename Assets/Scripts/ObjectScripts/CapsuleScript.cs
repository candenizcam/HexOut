using System;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class CapsuleScript: WorldObject
    {
        public SpriteRenderer capsuleRenderer;
        public BoxCollider2D boxCollider;
        public Vector2 UnitVector => ThisCapsuleData.UnitDirection();
        [NonSerialized]public CapsuleData ThisCapsuleData;
        
        public void Paint(Color c)
        {
            capsuleRenderer.color = c;
        }

        public bool Touching(Vector2 p)
        {
            return boxCollider.OverlapPoint(p);
        }

        public CapsuleData NewData(int r, int c)
        {
            return new CapsuleData(r,c,ThisCapsuleData.Length,ThisCapsuleData.Angle);
        }

        public CapsuleData TranslateData(int d)
        {
            var p = ThisCapsuleData.PointFromFirst(d);
            return NewData(p.LastRow,p.LastCol);

        }
        
    }
}