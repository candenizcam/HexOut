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
            var otherData = _capsules
                .Where(x => x != cs)
                .SelectMany(x => x.Occupies())
                .Distinct()
                .ToList();
            
            var rf = RecursiveMovementCheck(cs.ThisCapsuleData, otherData, true);
            var rb = RecursiveMovementCheck(cs.ThisCapsuleData, otherData, false);
            return (rf.cd,rb.cd, rf.EndType, rb.EndType);

        }


        private (CapsuleData cd, int EndType) RecursiveMovementCheck(CapsuleData cd, List<(int row, int col)> otherData, bool forward)
        {
            var newData = cd.DataFromFirst(forward ? 1:-1);
            if (!newData.WithinBounds(_row, _col)) return (cd,1); // out of bounds
            if (_obstacles.Any(x => newData.ObstaclesBy(x))) return (cd,2); //obstacle
            if (otherData.Any(x => newData.CollidesTo(x.row,x.col))) return (cd,3); // collision
            return RecursiveMovementCheck(newData, otherData, forward);
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
                    var allowed = AllowedMoves(capsuleScript);
                    var newData = capsuleScript.MovesForward ? allowed.forward : allowed.backward;
                    var endType = capsuleScript.MovesForward ? allowed.forwardEnd : allowed.backwardEnd;
                    if (newData.SameData(capsuleScript.ThisCapsuleData))
                    {
                        if (endType == 1)
                        {
                            destroyList.Add(capsuleScript);
                        }
                        else
                        {
                            (int row, int col) terminal = capsuleScript.MovesForward
                                ? newData.LastPoint()
                                : (newData.FirstRow, newData.FirstCol);
                            var worldPoint = _grid.TwoCoordsToWorld(terminal.row, terminal.col, _smallScale);
                            capsuleScript.StopMovement();
                            var p1 = capsuleScript.transform.position;
                            var p2 = new Vector3(worldPoint.x,worldPoint.y,p1.z);
                            NewTween(.2f,duringAction: (alpha) =>
                            {
                                var a = (float)Math.Sin(alpha * 3.141);
                                capsuleScript.transform.position = p2 * a + p1 * (1f - a);
                            });
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
