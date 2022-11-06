using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public partial class GameLevelScript: WorldObject
    {
        public Action<Tween, float> AddTweenAction;
        
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
        
        
        protected override void UpdateFunction()
        {
            var destroyList = new List<CapsuleScript>();
            
            foreach (var capsuleScript in _capsules)
            {
                if (capsuleScript.MovementState == 1)
                {
                    capsuleScript.DoMovement();
                }else if (capsuleScript.MovementState == 2)
                {
                    
                    var otherData = OtherData(capsuleScript);
                    var newOut =  SingleMovementCheck(capsuleScript.ThisCapsuleData, otherData, capsuleScript.MovesForward);
                    var newData = newOut.cd;
                    var endType = newOut.EndType;
                    //var allowed = AllowedMoves(capsuleScript);
                    //var newData = capsuleScript.MovesForward ? allowed.forward : allowed.backward;
                    //var endType = capsuleScript.MovesForward ? allowed.forwardEnd : allowed.backwardEnd;
                    if (endType != 0)
                    {
                        if (endType == 1)
                        {
                            destroyList.Add(capsuleScript);
                        }
                        else
                        {
                            var thisPoint = _grid.TwoCoordsToWorld(capsuleScript.ThisCapsuleData.FirstRow,
                                capsuleScript.ThisCapsuleData.FirstCol, _smallScale);
                            var otherPoint = _grid.TwoCoordsToWorld(newData.FirstRow, newData.FirstCol, _smallScale);
                            
                            capsuleScript.StopMovement();
                            var targetPoint = capsuleScript.transform.position;
                            
                            var delta = ( otherPoint - thisPoint).ToVector3(z:targetPoint.z); // single tile length
                            
                            var oneTileDiff = delta.Magnitude2D();
                            
                            var initX = capsuleScript.capsuleRenderer.size.x;
                            
                            var initY = capsuleScript.capsuleRenderer.size.y;

                            var side = initX / oneTileDiff * 0.5f * delta;
                            var x1_0 = targetPoint + side;
                            var x3_0 = targetPoint - side;

                            var md = endType == 2 ? oneTileDiff / 2f : oneTileDiff-initY/2f;
                            

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

                            },callDuringWithStartFunction:true);
                            
                        }
                    }
                    else
                    {
                        capsuleScript.NewTarget(newData,CapsulePosition(newData));
                    }
                }
            }
            
            foreach (var capsuleScript in destroyList)
            {
                Destroy(capsuleScript.gameObject);
                _capsules.Remove(capsuleScript);
            }
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
