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

            
            
            _activeLevel.SetGrid(MainCamera);
            _activeLevel.SetCapsules(new CapsuleData[]
            {
                new CapsuleData(2,3,2,1),
                new CapsuleData(3,3,2,3),
                new CapsuleData(5,2,2,1),
                new CapsuleData(4,3,2,2),
                //new CapsuleData(4,3,2,1),
            });


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