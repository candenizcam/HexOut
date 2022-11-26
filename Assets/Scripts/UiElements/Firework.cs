
using System;
using System.Collections.Generic;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    
    
    public class Firework: VisualElement
    {
        private Vector2 _origin;
        private Vector2 _explosionTarget; // not relative
        private VisualElement _bullet;
        private Color _colour;
        private float _width = 60f;
        private float _height = 24f;
        private List<VisualElement> _fragments = new List<VisualElement>();
        private List<Vector2> _fragmentTargets =new List<Vector2>();

        public Firework(Vector2 origin, Vector2 explosionTarget, UnityEngine.Color colour)
        {
            var r = new System.Random();
            var cols = GameDataBase.CapsuleColours();
            var col = cols.inside[r.Next(0, cols.inside.Length)];
            _origin = origin - new Vector2(_width*0.5f,_height*0.5f);
            _explosionTarget = explosionTarget- new Vector2(_width*0.5f,_height*0.5f);
            _colour = colour;
            this.StretchToParentSize();
            

            var d = explosionTarget - origin;
            _bullet = new VisualElement()
            {
                style=
                {
                    width = _width,
                    height = _height,
                    rotate = new StyleRotate(new Rotate(d.Angle())),
                    
                    backgroundImage = QuickAccess.LoadSpriteBg("UI/Capsule"),
                    unityBackgroundImageTintColor = new StyleColor(col),
                    unityBackgroundScaleMode = ScaleMode.StretchToFill
                    
                }
            };
            Add(_bullet);


            
            
            for (var i = 0; i < r.Next(15, 30); i++)
            {
                var v = new VisualElement()
                {
                    style=
                    {
                        width = 12f,
                        height = 30f,
                        position = Position.Absolute,
                        top = explosionTarget.y,
                        left = explosionTarget.x,
                        //rotate = new StyleRotate(new Rotate(r.Next(0,360))),
                        backgroundImage = QuickAccess.LoadSpriteBg("UI/Hex_Blank"),
                        unityBackgroundScaleMode = ScaleMode.StretchToFill,
                        unityBackgroundImageTintColor = new StyleColor(col),
                        
                    },
                    visible = false
                };
                _fragments.Add(v);
                Add(v);

                var a = (float)r.NextDouble()*6.282;
                var l = (float)r.Next(80, 240);
                var x = (float)Math.Sin(a)*l;
                var y = (float)Math.Cos(a)*l;
                _fragmentTargets.Add(new Vector2(x,y));
                
            }
            
        }

        public void Fly(float alpha)
        {
            var p = _explosionTarget * alpha + _origin * (1f - alpha);
            _bullet.style.left = p.x;
            _bullet.style.top = p.y;
            _bullet.style.width = _width * (1f - 0.6f * alpha);
        }

        public void Explode(float alpha)
        {
            _bullet.visible = false;
            for (int i=0;i<_fragments.Count;i++)
            {
                var a = Math.Clamp(alpha * 2f - (float) i / (float) _fragments.Count, 0f, 1f);
                
                _fragments[i].visible = a<1f;
                _fragments[i].style.left = _explosionTarget.x + a*_fragmentTargets[i].x;
                _fragments[i].style.top = _explosionTarget.y + a*_fragmentTargets[i].y;
                
            }
            
        }

    }
}