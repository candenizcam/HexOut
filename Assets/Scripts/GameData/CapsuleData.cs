using System;
using UnityEngine;

namespace DefaultNamespace.GameData
{
    public class CapsuleData
    {
        public int FirstRow;
        public int FirstCol;
        public int Length;
        public int Angle;
        
        /** r is first row
         * c is first col
         * length is a the number of tiles capsule sits on
         * angle starts from top right and goes clockwise up to 6
         */
        public CapsuleData(int r, int c, int length, int angle)
        {
            FirstRow = r;
            FirstCol = c;
            Length = length;
            Angle = angle%6;




        }

        public string Path()
        {
            if (Length == 2)
            {
                return "prefabs/CapsuleTwo";
            }
            else
            {
                return "prefabs/CapsuleTwo";
            }
        }

        public (int LastRow, int LastCol) LastPoint()
        {
            

            return PointFromFirst(Length - 1);
        }
        
        public (int LastRow, int LastCol) PointFromFirst(int len)
        {
            var a = len<0 ?  (Angle + 3) % 6:Angle;
            var l = (int) Math.Abs(len);
            
            switch (a)
            {
                case 0:
                    return (FirstRow-l, FirstCol + (l+(FirstRow+1)%2) / 2);
                case 1:
                    return (FirstRow, FirstCol+l);
                case 2:
                    return (FirstRow+l, FirstCol + (l+(FirstRow+1)%2) / 2);
                case 3:
                    return (FirstRow+l, FirstCol- (l+(FirstRow)%2 )/2);
                case 4:
                    return (FirstRow, FirstCol-l);
                default:
                    return (FirstRow-l, FirstCol- (l+(FirstRow)%2 )/2);
            }
        }
        
        

        public float Degrees()
        {
            switch (Angle)
            {
                case 0:
                    return 60f;
                case 1:
                    return 0f;
                case 2:
                    return 120f;
                case 3:
                    return 60f;
                case 4:
                    return 0f;
                default:
                    return 120f;
            }
        }

        public Vector2 UnitDirection()
        {
            switch (Angle)
            {
                case 0:
                    return new Vector2(.366f,.633f);
                case 1:
                    return new Vector2(1f,0f);
                case 2:
                    return new Vector2(.366f,-.633f);
                case 3:
                    return new Vector2(-.366f,-.633f);
                case 4:
                    return new Vector2(-1f,0f);
                default:
                    return new Vector2(-.366f,.633f);
            }
        }
        
        
    }
    
    
    
    
    
    
}