using System.Collections.Generic;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestMainScript : MainScript
    {
        private GameLevelScript _activeLevel;
        public int testRow;
        public int testCol;
        public int testObstacleSeed;
        public int testCapsuleSeed;
        public int testCapsuleNumber;
        public int testObstacleNumber;
        
        
        protected override void InitializeMain()
        {
            UIDocument.rootVisualElement.style.paddingBottom = Constants.UnsafeBottomUi;
            UIDocument.rootVisualElement.style.paddingTop = Constants.UnsafeTopUi;
            Application.targetFrameRate = 60;


            _activeLevel = GameLevelScript.Instantiate();
            
            
            
            MainCamera.backgroundColor = Color.white;


            var r = testRow;
            var c = testCol;
            var capsuleSeed = testCapsuleSeed;
            var obstacleSeed = testObstacleSeed;
            var capsuleNumber = testCapsuleNumber;
            var obstacleNumber = testObstacleNumber;
            
            
            var d = LevelGenerator.GenerateFrameLevel(capsuleSeed,obstacleSeed,r,c,capsuleNumber,obstacleNumber);
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