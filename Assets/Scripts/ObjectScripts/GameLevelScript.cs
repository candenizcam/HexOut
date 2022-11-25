﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Newtonsoft.Json.Linq;
using Punity;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DefaultNamespace
{
    public partial class GameLevelScript: WorldObject
    {
        public Action<Tween, float> AddTweenAction;
        public Action<LevelCompleteData> LevelDoneAction = (LevelCompleteData lcd) => {};
        public Action<int,int> CapsuleRemovedAction;
        public Action<string> PopUpTextAction;
        

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
        private LevelCompleteData _levelCompleteData = new LevelCompleteData(0,"");

        public Vector3 CapsulePosition(CapsuleData capsuleData)
        {
            if (capsuleData.Collapsed)
            {
                return _grid.TwoCoordsToWorld(capsuleData.FirstRow,capsuleData.FirstCol,_smallScale);
            }
            else
            {
                var lp = capsuleData.LastPoint();
                var v1 = _grid.TwoCoordsToWorld(capsuleData.FirstRow,capsuleData.FirstCol,_smallScale);
                var v2 = _grid.TwoCoordsToWorld(lp.LastRow,lp.LastCol,_smallScale);
                return  v1 * 0.5f + v2 * 0.5f;
                
            }
            
        }
        
        public void TouchBegan(Vector2 startPos)
        {
            _startPos = startPos;
            var touched = _capsules.Where(x => x.Touching(startPos) && x.MovementState==0);
            if (touched.Any())
            {
                _touchedGuy = touched.First();
            }
            else
            {
                _touchedGuy = null;
            }
        }

        public void DuringTouch()
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
            var otherData = OtherData(cs);
            
            var rf = RecursiveMovementCheck(cs.ThisCapsuleData, otherData, true);
            var rb = RecursiveMovementCheck(cs.ThisCapsuleData, otherData, false);
            return (rf.cd,rb.cd, rf.EndType, rb.EndType);

        }


        /** Used to obtain as far as capsule goes
         * 
         */
        private (CapsuleData cd, int EndType) RecursiveMovementCheck(CapsuleData cd, List<(int row, int col)> otherData, bool forward)
        {
            var newData = cd.DataFromFirst(forward ? 1:-1);
            if (!newData.WithinBounds(_row, _col)) return (cd,1); // out of bounds
            if (_obstacles.Any(x => newData.ObstaclesBy(x))) return (cd,2); //obstacle
            if (otherData.Any(x => newData.CollidesTo(x.row,x.col))) return (cd,3); // collision
            return RecursiveMovementCheck(newData, otherData, forward);
        }
        
        /** Always returns the next data based on the given data
         * 
         */
        private (CapsuleData cd, int EndType) SingleMovementCheck(CapsuleData cd, List<(int row, int col)> otherData, bool forward)
        {
            var newData = cd.DataFromFirst(forward ? 1:-1);
            if (!newData.WithinBounds(_row, _col)) return (newData,1); // out of bounds
            if (_obstacles.Any(x => newData.ObstaclesBy(x))) return (newData,2); //obstacle
            if (otherData.Any(x => newData.CollidesTo(x.row,x.col))) return (newData,3); // collision
            return (newData, 0);
            
        }
        

        private List<(int row, int col)> OtherData(CapsuleScript cs)
        {
            return _capsules
                .Where(x => x != cs)
                .SelectMany(x => x.Occupies())
                .Distinct()
                .ToList();
        }



        public int ProcessNewMovement(CapsuleScript capsuleScript)
        {
            var otherData = _capsules
                .Where(x => x != capsuleScript);

            var oldData = capsuleScript.ThisCapsuleData;
            CapsuleData newData;
            newData = oldData.Collapsed ? oldData.UnCollapse() : oldData.DataFromFirst(1);
            
            
            
            //var endType = 0;
            if (!newData.WithinBounds(_row, _col)) return 1; // out of bounds

            var obstacled = _obstacles.Where(x => newData.ObstaclesBy(x)).ToList();

            if (obstacled.Any())
            {
                var reflectors = obstacled.Where(x => newData.FirstRow == x.Row && newData.FirstCol == x.Col && x.Length == 2);
                ObstacleData clashing;
                var ret = 0;
                var reflecting = false;
                if (reflectors.Any())
                {
                    clashing = reflectors.First();
                    var newAngle = (newData.Angle + (newData.Angle == clashing.Direction ? 4:2))%6;
                    var newerData = new CapsuleData(newData.FirstRow, newData.FirstCol, newData.Length, newAngle,true); 
                    capsuleScript.NewTarget(newerData,CapsulePosition(newerData));
                    reflecting = true;
                }
                else
                {
                    clashing = obstacled.First();
                    if (oldData.Collapsed)
                    {
                        SetForObstacleCollision(capsuleScript,newData.DataFromFirst(1),true);
                        
                    }
                    else
                    {
                        SetForObstacleCollision(capsuleScript,newData,false);
                    }
                    ret = 2;
                }
                

                var thatTile = _grid.GetTile(clashing.Row, clashing.Col);
                var sr = clashing.Length==1 ? thatTile.obstacles[clashing.Direction]:thatTile.doubleObstacles[clashing.Direction];
                var initCol = sr.color;
                var initPos = sr.gameObject.transform.position;

                
                var u = newData.UnitDirection().ToVector3() * (_smallScale * 0.1f);
                
                NewTween(.3f,
                    startAction: () =>
                    {
                        if (reflecting)
                        {
                            PopupFunction(Constants.ObstacleReflectionText.PickRandom());
                        }
                    }
                    ,duringAction: (alpha) =>
                {
                    var a = Math.Clamp(alpha * (1f - alpha)*8f,0f,1f);
                    sr.color = new Color(
                        Math.Clamp(initCol.r + a * .4f,0f,1f),
                        Math.Clamp(initCol.g + a * .4f,0f,1f),
                        Math.Clamp(initCol.b + a * .4f,0f,1f));
                    sr.gameObject.transform.position = initPos + u*(float)Math.Sin(alpha * 3.141 * 2f);
                });
                
                return ret;
            }
            var otherGuys = otherData.Where(x => x.DataForCollision().Any(y=>y.CollidesWith(newData)));

            if (otherGuys.Any())
            {
                if (oldData.Collapsed)
                {
                    SetForCapsuleCollision(capsuleScript, newData.DataFromFirst(1), true,
                        otherCapsule: otherGuys.First());
                }
                else
                {
                    SetForCapsuleCollision(capsuleScript, newData, false, otherCapsule: otherGuys.First());
                }
                return 3;
            }
            
            capsuleScript.NewTarget(newData,CapsulePosition(newData));
            return 0;
        }

        private void SetForCapsuleCollision(CapsuleScript capsuleScript, CapsuleData newData,  bool andMove, CapsuleScript otherCapsule)
        {
            var thisPoint = _grid.TwoCoordsToWorld(capsuleScript.ThisCapsuleData.FirstRow,
                        capsuleScript.ThisCapsuleData.FirstCol, _smallScale);
            var otherPoint = _grid.TwoCoordsToWorld(newData.FirstRow, newData.FirstCol, _smallScale);
            capsuleScript.StopMovement();
            var targetPoint = capsuleScript.transform.position;
            var delta = ( otherPoint - thisPoint).ToVector3(z:targetPoint.z); // single tile length
            var oneTileDiff = delta.Magnitude2D();
            var initY = capsuleScript.capsuleRenderer.size.y;
            var target = _grid.TwoCoordsToWorld(newData.LastPoint(), _smallScale);
            var v2 = (Vector2)otherCapsule.capsuleRenderer.gameObject.transform.position - target;
            var v1 = v2.x * capsuleScript.UnitVector.x + v2.y * capsuleScript.UnitVector.y;
            
            float md2;
            if (Math.Abs(v1) > .9f) // head to head
            {
                md2 = oneTileDiff- initY*0.15f;
            }else if (v1 < 0) // from inside
            {
                md2 = oneTileDiff- initY*0.5f;
            }
            else // from outside
            {
                md2 = oneTileDiff- initY*0.25f;
            }
            var travelTime =  CollisionTween(md2, capsuleScript, oneTileDiff, delta, initY, andMove,Constants.CapsuleCollisionText.PickRandom());
            var ud = newData.UnitDirection().ToVector3() * (_smallScale * 0.1f);
            NewTween(travelTime*0.25f,startAction: () =>
            {
                
            },duringAction: alpha =>
            {
                try
                {
                    otherCapsule.gameObject.transform.position += ud*(float)Math.Sin(alpha * 3.141 * 2f);
                }
                catch (MissingReferenceException e)
                {
                    
                }
                
            },delay:travelTime*0.5f);
        }

        private void SetForObstacleCollision(CapsuleScript capsuleScript, CapsuleData newData, bool andMove)
        {
            var thisPoint = _grid.TwoCoordsToWorld(capsuleScript.ThisCapsuleData.FirstRow,
                        capsuleScript.ThisCapsuleData.FirstCol, _smallScale);
            var otherPoint = _grid.TwoCoordsToWorld(newData.FirstRow, newData.FirstCol, _smallScale);
            capsuleScript.StopMovement();
            var targetPoint = capsuleScript.transform.position;
            var delta = ( otherPoint - thisPoint).ToVector3(z:targetPoint.z); // single tile length
            var oneTileDiff = delta.Magnitude2D();
            var initY = capsuleScript.capsuleRenderer.size.y;
            var md = oneTileDiff / 2f;
            CollisionTween(md, capsuleScript, oneTileDiff, delta, initY, andMove,Constants.ObstacleCollisionText.PickRandom());
        }

        private float CollisionTween(float md, CapsuleScript capsuleScript, float oneTileDiff, Vector3 delta, float initY, bool andMove, string exitText)
        {
            var targetPoint = capsuleScript.transform.position;
            var initX = capsuleScript.capsuleRenderer.size.x;
            var side = initX / oneTileDiff * 0.5f * delta;
            var x1_0 = targetPoint + side;
            var x3_0 = targetPoint - side;
            var maxDistance = md; //whole distance to travel
            var travelSpeed = oneTileDiff / Constants.SprayMovementPerTile;
            var travelTime = maxDistance * 2f / travelSpeed;
            var cutoffTime = 0.25f*oneTileDiff / travelSpeed/travelTime;
            var relta = (maxDistance * 2f)/oneTileDiff*delta;
            NewTween(travelTime, duringAction: (alpha) =>
            {
                var x3 = alpha < .5f ? x3_0 + alpha * relta : x3_0 + (1f - alpha) * relta;
                
                var x1 = alpha < .5f-cutoffTime ? x1_0 + alpha * relta : 
                    alpha>.5f+cutoffTime ? x1_0 + (1f - alpha) * relta : x1_0 + ( .5f-cutoffTime)*relta;

                var x2 = x1 * 0.5f + x3 * 0.5f;
                capsuleScript.transform.position = x2;

                var newMagnitude = (x3-x1).Magnitude2D();
                var delX = (initX-newMagnitude)/initX;
                capsuleScript.capsuleRenderer.size = new Vector2(newMagnitude,initY+delX*0.5f);
                capsuleScript.innerCapsuleRenderer.size = new Vector2(newMagnitude,initY+delX*0.5f);

            }, exitAction: () =>
            {
                
                if (andMove)
                {
                    capsuleScript.ThisCapsuleData = capsuleScript.ThisCapsuleData.Revert();
                    capsuleScript.MovementState = 2;
                    
                }
                else
                {
                    
                }
            } ,callDuringWithStartFunction:true);
            
            NewTween(travelTime/2f,exitAction: () =>
            {
                PopupFunction(exitText);
            });
            
            return travelTime;
        }
        
        
        
        protected override void UpdateFunction()
        {
            var destroyList = new List<CapsuleScript>();
            
            foreach (var capsuleScript in _capsules)
            {
                if (capsuleScript.MovementState == 1)
                {
                    capsuleScript.DoThings();
                }else if (capsuleScript.MovementState == 2)
                {
                    var v = ProcessNewMovement(capsuleScript);
                    if (v == 1)
                    {
                        PopupFunction(Constants.ExitText.PickRandom());
                        destroyList.Add(capsuleScript);
                    }
                }
            }
            
            foreach (var capsuleScript in destroyList)
            {
                Destroy(capsuleScript.gameObject);
                _capsules.Remove(capsuleScript);
                var x = XPSystem.CapsuleXp(LevelDiff);
                CapsuleRemovedAction(_levelCompleteData.LevelXp,x);
                _levelCompleteData.LevelXp += x;
                
            }

            if (!_capsules.Any())
            {
                LevelDoneFunction();   
            }
        }

        private void LevelDoneFunction()
        {
            LevelDoneAction(_levelCompleteData);
        }

        private void PopupFunction(string s)
        {
            PopUpTextAction(s);
        }


        public void NewTween(float sec, Action startAction = null, Action exitAction = null,
            Action<float> duringAction = null, int repeat = 1, bool callDuringWithStartFunction = false,
            float delay = 0f)
        {
            AddTweenAction(new Tween(sec, startAction, exitAction, duringAction, repeat, callDuringWithStartFunction),
                delay);
        }
        
        public static GameLevelScript Instantiate()
        {
            var a = new GameObject("game level");
            var n = a.AddComponent<GameLevelScript>();
            return n;
        }
        
    }
}
