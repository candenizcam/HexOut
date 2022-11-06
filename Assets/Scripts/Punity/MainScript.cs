using System;
using Punity;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.Punity
{
    public class MainScript: MonoBehaviour
    {
        protected Camera MainCamera;
        protected UIDocument UIDocument;
        protected TweenHolder TweenHolder;
        
        private void Awake()
        {
            AdjustCamera();
            InitializeUi();
            TweenHolder = new TweenHolder();
            InitializeMain();
            
        }

        private void Update()
        {
            UpdateMain();
            
        }

        private void LateUpdate()
        {
            TweenHolder.Update(Time.deltaTime);
        }


        private void InitializeUi()
        {
            UIDocument = gameObject.GetComponent<UIDocument>();
            UIDocument.panelSettings.referenceResolution = new Vector2Int((int)Constants.UiWidth, (int)Constants.UiHeight);
            UIDocument.panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            UIDocument.panelSettings.match = 1f;
        }

        protected void AdjustCamera()
        {
            MainCamera = Camera.main;
            MainCamera.orthographicSize = Constants.WorldHeight;
            MainCamera.backgroundColor = Color.black;
        }

        protected virtual void InitializeMain(){} // this function must be overriden in the main instead of unity awake
        protected virtual void UpdateMain(){}
        
    }
    
    
   
    
}