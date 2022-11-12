using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.GameData
{
    public class CapsuleData
    {
        public int FirstRow;
        public int FirstCol;
        public int Length;
        public int Angle;
        public bool Collapsed;
        
        /** r is first row
         * c is first col
         * length is a the number of tiles capsule sits on
         * angle starts from top right and goes clockwise up to 6
         */
        public CapsuleData(int r, int c, int length, int angle, bool collapsed)
        {
            FirstRow = r;
            FirstCol = c;
            Length = length;
            Angle = angle%6;
            Collapsed = collapsed;
        }

        public bool SameData(CapsuleData other)
        {
            return FirstRow == other.FirstRow && FirstCol == other.FirstCol && Length == other.Length && Angle == other.Angle;
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

        public CapsuleData DataFromFirst(int len)
        {
            var l = PointFromFirst(len);
            return new CapsuleData(l.LastRow, l.LastCol, Length, Angle,Collapsed);
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

        public List<(int row, int col)> TwoIndexTiles()
        {
            var n = new List<(int row, int col)>();

            n.Add((FirstRow,FirstCol));
            if (Collapsed) return n;
            for (int l=1;l<Length;l++)
            {
                var p = PointFromFirst(l);
                n.Add(p);
            }
            return n;
        }


        public bool CollidesTo(int row, int col)
        {
            return TwoIndexTiles().Any(x => x.row == row && x.col == col);
        }
        
        
        public bool CollidesWith(CapsuleData other)
        {
            
            
            var theseTiles = TwoIndexTiles();
            var otherTiles = other.TwoIndexTiles();
            return theseTiles.Any(x => otherTiles.Any(y => x.row == y.row && x.col == y.col));
        }

        public bool ObstaclesBy(ObstacleData obstacle)
        {
            var theseTiles = TwoIndexTiles();
            var first = theseTiles.Any(x => x.row == obstacle.Row && x.col == obstacle.Col);
            var obsOpsList = obstacle.Oppositions();
            
            
            var second = obsOpsList.Any(obsOps => theseTiles.Any(x => x.row == obsOps.OtherRow && x.col == obsOps.OtherCol));
            return first && second;

        }


        public bool WithinBounds(int maxRow, int maxCol, int minRow = 1, int minCol = 1)
        {
            return !TwoIndexTiles().Any(x => x.row < minRow || x.row > maxRow || x.col < minCol || x.col > maxCol);
        }

        public CapsuleData Collapse()
        {
            return new CapsuleData(FirstRow, FirstCol, Length, Angle, true);
        }

        public CapsuleData UnCollapse()
        {
            return new CapsuleData(FirstRow,FirstCol,Length,Angle,false);
        }

        public CapsuleData Revert()
        {
            if (Collapsed)
            {
                return new CapsuleData(FirstRow,FirstCol, Length,Angle+3,Collapsed);
            }
            else
            {
                var lp = LastPoint();
                return new CapsuleData(lp.LastRow,lp.LastCol, Length,Angle+3,Collapsed);
                
            }
            

        }
    }
    
    
    
    
    
    
}