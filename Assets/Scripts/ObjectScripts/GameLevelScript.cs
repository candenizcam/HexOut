using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DefaultNamespace
{
    public class GameLevelScript: WorldObject
    {
        private HexGridScript _grid;
        private List<CapsuleScript> _capsules = new();
        private float _smallScale = 1f;
        public float SmallScale => _smallScale;
        private Vector2 _gridCentre = new Vector2();
        private int _col = 0;
        private int _row = 0;
        private Vector2 _startPos = new Vector2(-1f,-1f);
        private  ObstacleData[] _obstacles;
        
        public void SetGrid(Camera c, int row, int col, ObstacleData[] obstacles)
        {
            _row = row;
            _col = col;
            _grid = HexGridScript.Instantiate(gameObject.transform);
            _grid.InitializeGrid(_row,_col, obstacles);
            _obstacles = obstacles;
            
            SetSize(new Vector2(0f,0f), 
                c.orthographicSize*c.aspect*2f,
                c.orthographicSize*2f);
            //transform.localScale = new Vector3(_grid.SmallScale, _grid.SmallScale,1f);
        }

        
        
        /** This function sets the size to a given field in world space
         * importantly, you give this the limits, and it fits the tiles accordingly
         */
        public void SetSize(Vector2 centre, float width, float height)
        {
            var baseWidth = HexTileScript.Width * _col;
            var baseHeight = HexTileScript.Height * .375f * (_row-1) + HexTileScript.SideLength*.5f;

            

            var widthScale = width / baseWidth;
            var heightScale = height / baseHeight;
            _smallScale = Math.Min(widthScale, heightScale);
            _gridCentre = centre;
            
            var v = new Vector3(_smallScale, _smallScale,1f);
            transform.localScale = v;
            //gameObject.transform.localScale = v;
            transform.position = new Vector3(centre.x, centre.y, -1f);


        }
        
        

        public void SetCapsules(CapsuleData[] capsuleDataList)
        {
            var r = new System.Random();
            foreach (var capsuleData in capsuleDataList)
            {
                var c = Constants.CapsuleColours.OrderBy(x => r.NextDouble()).First();
                SetCapsule(capsuleData,c);
            }
        }

        
        

        public void SetCapsule(CapsuleData capsuleData, Color? col = null)
        {
            var res = Resources.Load<GameObject>(capsuleData.Path());
            var q = Instantiate(res,gameObject.transform);
            var cs = q.GetComponent<CapsuleScript>();

            var v3 = CapsulePosition(capsuleData);
                
                
            q.transform.position = new Vector3(v3.x, v3.y, -2f);
            q.transform.rotation = Quaternion.Euler(0f,0f,capsuleData.Degrees());
            q.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
            
            cs.Paint(col ??= Color.white);
            cs.ThisCapsuleData = capsuleData;
            _capsules.Add(cs);
        }


        public void StartAnimation(float alpha)
        {
            
            var capsuleSize = Easing.OutBack(alpha);
            foreach (var capsuleScript in _capsules)
            {
                capsuleScript.transform.localScale = new Vector3(capsuleSize, capsuleSize, 1f);
            }
        }


        public Vector3 CapsulePosition(CapsuleData capsuleData)
        {
            var lp = capsuleData.LastPoint();
            var v1 = _grid.TwoCoordsToWorld(capsuleData.FirstRow,capsuleData.FirstCol,_smallScale);
            var v2 = _grid.TwoCoordsToWorld(lp.LastRow,lp.LastCol,_smallScale);

            return  v1 * 0.5f + v2 * 0.5f;
        }
        
        
        
        
        


        public void TouchBegan(Vector2 startPos)
        {
            _startPos = startPos;

            var touched = _capsules.Where(x => x.Touching(startPos));
            if (touched.Any())
            {
                var f = touched.First();
                
            }
            
            

        }

        public void TouchEnded(Vector2 endPos)
        {
            var touched = _capsules.Where(x => x.Touching(_startPos));
            if (touched.Any())
            {
                var f = touched.First();
                var fu = f.UnitVector;
                //var fa = f.Atan2();
                var delta = endPos - _startPos;
                var du = delta.normalized;


                var dotProd = fu.x * du.x + fu.y * du.y;

                if (Math.Abs(dotProd) > .2f)
                {
                    MoveCapsule(f,dotProd>0);
                }
                
            }
            _startPos = new Vector2(-1f, -1f);
        }


        private void MoveCapsule(CapsuleScript cs, bool forward)
        {
            var otherData = _capsules.Where(x => x != cs).Select(x => x.ThisCapsuleData);
            
            
            CapsuleData oldData = null;
            for (int i = 1; i < 100; i++)
            {
                CapsuleData targetData;
                (int LastRow, int LastCol) target; 
                if (forward)
                {
                    targetData = cs.TranslateData(i);
                    target = targetData.LastPoint();   
                }
                else
                {
                    targetData = cs.TranslateData(-i);
                    target = (targetData.FirstRow, targetData.FirstCol);
                }

                
                if (target.LastCol > _col || target.LastCol < 1 || target.LastRow < 1 || target.LastRow > _row)
                {
                    // escape
                    if (oldData is not null)
                    {
                        //var nd = oldData;
                        //var v3 = CapsulePosition(nd);
                        //cs.gameObject.transform.position = new Vector3(v3.x, v3.y, -2f);
                        //cs.ThisCapsuleData = oldData;
                        Destroy(cs.gameObject);
                        _capsules.Remove(cs);
                    }
                    break;
                }
                
                //targetData.CollidesWith()

                var obstacled = _obstacles.Any(x => targetData.ObstaclesBy(x));
                if (obstacled)
                {
                    if (oldData is not null)
                    {
                        var nd = oldData;
                        var v3 = CapsulePosition(nd);
                        cs.gameObject.transform.position = new Vector3(v3.x, v3.y, -2f);
                        cs.ThisCapsuleData = oldData;
                    }
                    break;
                }
                
                
                


                var collision = otherData.Any(x => x.CollidesWith(targetData));
                
                //var targetWorld = _grid.TwoCoordsToWorld(target.LastRow, target.LastCol,_smallScale);
                //var collision = _capsules.Any(x => x.Touching(targetWorld)&&x!=cs);
                if (collision)
                {
                    if (oldData is not null)
                    {
                        var nd = oldData;
                        var v3 = CapsulePosition(nd);
                        cs.gameObject.transform.position = new Vector3(v3.x, v3.y, -2f);
                        cs.ThisCapsuleData = oldData;
                    }
                    break;
                }
                oldData = targetData;
            }
        }
        
        
        public static GameLevelScript Instantiate()
        {
            var a = new GameObject("game level");
            var n = a.AddComponent<GameLevelScript>();
            return n;
        }
        
    }
}
