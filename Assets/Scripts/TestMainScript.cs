using DefaultNamespace.Punity;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestMainScript : MainScript
    {
        private HexGridScript _grid; 
        
        
        protected override void InitializeMain()
        {
            UIDocument.rootVisualElement.style.paddingBottom = Constants.UnsafeBottomUi;
            UIDocument.rootVisualElement.style.paddingTop = Constants.UnsafeTopUi;
            Application.targetFrameRate = 60;
            _grid = HexGridScript.Instantiate();
            _grid.InitializeGrid(5,5);
            
            
            
            _grid.SetSize(new Vector2(0f,0f), 
                MainCamera.orthographicSize*MainCamera.aspect*2f,
                MainCamera.orthographicSize*2f);
        }

        protected override void UpdateMain()
        {
            

        }
        
    }
}