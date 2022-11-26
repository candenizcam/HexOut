
using System;
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

        public Firework(Vector2 origin, Vector2 explosionTarget, UnityEngine.Color colour)
        {
            var width = 60f;
            var height = 24f;
            
            _origin = origin - new Vector2(width*0.5f,height*0.5f);
            _explosionTarget = explosionTarget- new Vector2(width*0.5f,height*0.5f);
            _colour = colour;
            this.StretchToParentSize();

            

            var d = explosionTarget - origin;
            _bullet = new VisualElement()
            {
                style=
                {
                    width = width,
                    height = height,
                    rotate = new StyleRotate(new Rotate(d.Angle())),
                    
                    backgroundImage = QuickAccess.LoadSpriteBg("UI/Capsule"),
                    unityBackgroundImageTintColor = new StyleColor(_colour),
                    unityBackgroundScaleMode = ScaleMode.StretchToFill
                    
                }
            };
            Add(_bullet);
            
        }

        public void Fly(float alpha)
        {
            var p = _explosionTarget * alpha + _origin * (1f - alpha);
            Debug.Log(p);
            _bullet.style.left = p.x;
            _bullet.style.top = p.y;
        }

        public void Explode(float alpha)
        {
            
        }

    }
}