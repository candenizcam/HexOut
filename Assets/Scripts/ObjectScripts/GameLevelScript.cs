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
        private int _row = 9;
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
                SetCapsule(capsuleData,new Color((float)r.NextDouble(),(float)r.NextDouble(),(float)r.NextDouble()));
            }
        }

        public void ProcedurallyGenerate(int seed, int sprayNumber )
        {
            var procedural = new System.Random(seed);

            var capsules = new List<CapsuleData>();

            var rList = Enumerable.Range(3, _row-4).ToList();
            var cList = Enumerable.Range(3, _col-4).ToList();
            var aList = Enumerable.Range(0, 6).ToList();
            var lList = Enumerable.Repeat(2, sprayNumber).ToList(); // this will make sure procedural generator works with multiple length types,
                                                                    // lList can be replaced by something else in the future
            
            
            for(int i=0;i<100;i++)
            {
                var r = rList.OrderBy(a => procedural.Next()).First();
                var c = cList.OrderBy(a => procedural.Next()).First();
                var l = lList.OrderBy(a => procedural.Next()).First();
                var a = aList.OrderBy(a => procedural.Next()).First();
                var d = new CapsuleData(r,c,l,a);


                if (d.TwoIndexTiles().Any(t => t.row < 1 || t.row > _row || t.col < 1 || t.col > _col))
                {
                    continue;
                }

                if (!capsules.Any(x => x.CollidesWith(d)))
                {
                    capsules.Add(d);
                    if (capsules.Count > sprayNumber)
                    {
                        Debug.Log($"done in {i}");
                        break;
                    }
                }
            }
            
            SetCapsules(capsules.ToArray());
            
            

        }
        

        public void SetCapsule(CapsuleData capsuleData, Color? col = null)
        {
            var res = Resources.Load<GameObject>(capsuleData.Path());
            var q = Instantiate(res,gameObject.transform);
            var cs = q.GetComponent<CapsuleScript>();

            var v3 = CapsulePosition(capsuleData);
                
                
            q.transform.position = new Vector3(v3.x, v3.y, -2f);
            q.transform.rotation = Quaternion.Euler(0f,0f,capsuleData.Degrees());
                
            
            cs.Paint(col ??= Color.white);
            cs.ThisCapsuleData = capsuleData;
            _capsules.Add(cs);
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
        
        
        public static GameLevelScript Instantiate()
        {
            var a = new GameObject("game level");
            var n = a.AddComponent<GameLevelScript>();
            return n;
        }
        
    }
}
