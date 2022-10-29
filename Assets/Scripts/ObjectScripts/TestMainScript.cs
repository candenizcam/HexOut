using System.Collections.Generic;
using DefaultNamespace.GameData;
using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestMainScript : MainScript
    {
        
        
        
        protected override void InitializeMain()
        {
            UIDocument.rootVisualElement.style.paddingBottom = Constants.UnsafeBottomUi;
            UIDocument.rootVisualElement.style.paddingTop = Constants.UnsafeTopUi;
            Application.targetFrameRate = 60;


            var gls = GameLevelScript.Instantiate();
            
            
            
            MainCamera.backgroundColor = Color.white;

            
            
            gls.SetGrid(MainCamera);
            gls.SetCapsules(new CapsuleData[]
            {
                new CapsuleData(1,3,2,1),
                new CapsuleData(2,3,2,3),
                new CapsuleData(4,2,2,1),
                //new CapsuleData(2,2,2,5),
                //new CapsuleData(4,3,2,1),
            });


        }

        protected override void UpdateMain()
        {
            

        }
        
    }
}