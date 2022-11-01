using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Unity.VisualScripting;
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
        private CapsuleScript _touchedGuy;
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
                _touchedGuy = touched.First();
            }
            else
            {
                _touchedGuy = null;
            }
            
            

        }

        public void DuringTouch(Vector2 duringPos)
        {
            if (_touchedGuy is not null)
            {
                var allowed = AllowedMoves(_touchedGuy);

                (int r , int c) terminalFw = allowed.forward.LastPoint();
                (int r , int c) terminalBw = (allowed.backward.FirstRow,allowed.backward.FirstCol);

                var worldFw = _grid.TwoCoordsToWorld(terminalFw.r, terminalFw.c, _smallScale);
                var worldBw = _grid.TwoCoordsToWorld(terminalBw.r, terminalBw.c, _smallScale);

                var m = (worldFw - worldBw).magnitude + _grid.SideLength*_smallScale;
                var c = worldFw * .5f + worldBw * .5f;
                _touchedGuy.ActivateTrackShadow(
                    new Vector2(c.x, c.y),
                    new Vector3(m/_smallScale, 1f, 1f));
            }
        }

        public void TouchEnded(Vector2 endPos)
        {
            if (_touchedGuy is not null)
            {
                _touchedGuy.DeactivateTrackShadow();
                var m = TouchMagnitude(_startPos, endPos, _touchedGuy.UnitVector);
                if (Math.Abs(m) > .2f)
                {
                    _touchedGuy.SetToMovement(m>0);
                    //MoveCapsule(_touchedGuy,m>0);
                }
            }

            _startPos = new Vector2(-1f, -1f);
        }

        




        private static float TouchMagnitude(Vector2 startPos, Vector2 endPos, Vector2 touchedUnitVector)
        {
            var delta = (endPos - startPos);
            var du = delta.normalized;
            var dotProd = touchedUnitVector.x * du.x + touchedUnitVector.y * du.y;
            return  dotProd < -.2f ? -delta.magnitude : dotProd > .2f ? delta.magnitude : 0f;
        }


        private (CapsuleData forward, CapsuleData backward, int forwardEnd, int backwardEnd) AllowedMoves(CapsuleScript cs)
        {
            var otherData = _capsules
                .Where(x => x != cs)
                .SelectMany(x => x.Occupies())
                .Distinct()
                .ToList();


            var rf = RecursiveMovementCheck(cs.ThisCapsuleData, otherData, true);
            var rb = RecursiveMovementCheck(cs.ThisCapsuleData, otherData, false);
            return (rf.cd,rb.cd, rf.EndType, rb.EndType
                );

        }


        private (CapsuleData cd, int EndType) RecursiveMovementCheck(CapsuleData cd, List<(int row, int col)> otherData, bool forward)
        {
            var newData = cd.DataFromFirst(forward ? 1:-1);

            if (!newData.WithinBounds(_row, _col)) return (cd,1); // out of bounds
            if (_obstacles.Any(x => newData.ObstaclesBy(x))) return (cd,2); //obstacle
            if (otherData.Any(x => newData.CollidesTo(x.row,x.col))) return (cd,3); // collision
            return RecursiveMovementCheck(newData, otherData, forward);
        }
        
        
        
        
        


        private CapsuleData MoveCapsule(CapsuleScript cs, bool forward)
        {
            var otherData = _capsules.Where(x => x != cs).Select(x => x.ThisCapsuleData);
            
            
            CapsuleData oldData = null;
            for (int i = 1; i < 100; i++)
            {
                CapsuleData targetData;
                if (forward)
                {
                    targetData = cs.TranslateData(i);
                }
                else
                {
                    targetData = cs.TranslateData(-i);
                }

                if (!targetData.WithinBounds(_row,_col))
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
            return oldData;
        }
        
        protected override void UpdateFunction()
        {
            foreach (var capsuleScript in _capsules)
            {
                if (capsuleScript.MovementState == 1)
                {
                    capsuleScript.DoMovement();
                }else if (capsuleScript.MovementState == 2)
                {
                    //capsuleScript.MovesForward
                    var allowed = AllowedMoves(capsuleScript);
                    var newData = capsuleScript.MovesForward ? allowed.forward : allowed.backward;
                    var endType = capsuleScript.MovesForward ? allowed.forwardEnd : allowed.backwardEnd;
                    if (newData.SameData(capsuleScript.ThisCapsuleData))
                    {
                        if (endType == 1)
                        {
                            Destroy(capsuleScript.gameObject);
                            _capsules.Remove(capsuleScript);
                        }
                        else
                        {
                            capsuleScript.StopMovement();
                        }
                        
                    }
                    else
                    {
                        
                        
                        
                        capsuleScript.NewTarget(newData,CapsulePosition(newData));
                    }
                }
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
