using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameLevelScript: WorldObject
    {
        private HexGridScript _grid;
        private List<CapsuleScript> _capsules = new();
        private float _smallScale = 1f;
        public float SmallScale => _smallScale;
        private Vector2 _gridCentre = new Vector2();
        private int _col = 7;
        private int _row = 7;
        private Vector2 _startPos = new Vector2(-1f,-1f);
        
        
        public void SetGrid(Camera c)
        {
            _grid = HexGridScript.Instantiate(gameObject.transform);
            _grid.InitializeGrid(_row,_col);
            
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
                var res = Resources.Load<GameObject>(capsuleData.Path());
                var q = Instantiate(res,gameObject.transform);
                var cs = q.GetComponent<CapsuleScript>();

                var v3 = CapsulePosition(capsuleData);
                
                
                q.transform.position = new Vector3(v3.x, v3.y, -2f);
                q.transform.rotation = Quaternion.Euler(0f,0f,capsuleData.Degrees());
                
                cs.Paint(new Color((float)r.NextDouble(),(float)r.NextDouble(),(float)r.NextDouble()));
                cs.ThisCapsuleData = capsuleData;
                _capsules.Add(cs);
            }
            
            
            
        }


        public Vector3 CapsulePosition(CapsuleData capsuleData)
        {
            var lp = capsuleData.LastPoint();
            var v1 = _grid.TwoCoordsToWorld(capsuleData.FirstRow,capsuleData.FirstCol,_smallScale);
            var v2 = _grid.TwoCoordsToWorld(lp.LastRow,lp.LastCol,_smallScale);

            return  v1 * 0.5f + v2 * 0.5f;
        }
        
        
        
        
        public static GameLevelScript Instantiate()
        {
            var a = new GameObject("game level");
            var n = a.AddComponent<GameLevelScript>();
            return n;
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
                
                //var angle = Math.Atan2(delta.x, delta.y);
                
                //Debug.Log($"diff {fa -angle}");
                
            }

            
            
            
            
            _startPos = new Vector2(-1f, -1f);
        }


        private void MoveCapsule(CapsuleScript cs, bool forward)
        {
            CapsuleData oldData = null;
            for (int i = 1; i < 100; i++)
            {
                //var target = cs.ThisCapsuleData.PointFromFirst(forward ? i+thisLength: -i);

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
                    if (oldData is not null)
                    {
                        var nd = oldData;
                        var v3 = CapsulePosition(nd);
                        cs.gameObject.transform.position = new Vector3(v3.x, v3.y, -2f);
                        cs.ThisCapsuleData = oldData;
                        
                    }
                    break;
                    
                }
                
                var targetWorld = _grid.TwoCoordsToWorld(target.LastRow, target.LastCol,_smallScale);
                var collision = _capsules.Any(x => x.Touching(targetWorld)&&x!=cs);
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
        
        
        
        
    }
}