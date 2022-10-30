using System.Collections.Generic;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestMainScript : MainScript
    {
        private GameLevelScript _activeLevel;
        
        
        protected override void InitializeMain()
        {
            UIDocument.rootVisualElement.style.paddingBottom = Constants.UnsafeBottomUi;
            UIDocument.rootVisualElement.style.paddingTop = Constants.UnsafeTopUi;
            Application.targetFrameRate = 60;


            _activeLevel = GameLevelScript.Instantiate();
            
            
            
            MainCamera.backgroundColor = Color.white;


            var r = 7;
            var c = 5;
            
            
            
            
            var d = LevelGenerator.GenerateFrameLevel(2003,2001,r,c,3,10);
            _activeLevel.SetGrid(MainCamera,d.Row,d.Col, d.ObstacleDatas);
            _activeLevel.SetCapsules(d.CapsuleDatas);
            

        }


        private void HandleTouch()
        {
            if (Input.touches.Length > 0)
            {
                var tp = Input.touches[0].phase;
                if (tp == TouchPhase.Began)
                {

                    var wp = MainCamera.ScreenToWorldPoint(Input.touches[0].position);
                    
                    _activeLevel.TouchBegan(wp);
                    
                    
                    
                }else if (tp == TouchPhase.Ended)
                {
                    var wp = MainCamera.ScreenToWorldPoint(Input.touches[0].position);
                    _activeLevel.TouchEnded(wp);
                }
                else
                {
                    
                }


                

            }
        }
        
        
        
        
        

        protected override void UpdateMain()
        {
            HandleTouch();

        }
        
    }
}