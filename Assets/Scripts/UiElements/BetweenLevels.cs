using System;
using Classes;
using DefaultNamespace.GameData;
using Newtonsoft.Json.Linq;
using Punity.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class BetweenLevels: VisualElement
    {
        public Action LeftButtonAction = () =>{ };
        public Action MiddleButtonAction = () =>{ };
        public Action RightButtonAction = () =>{ };

        private VisualElement _progressBg;
        private ButtonClickable _leftButton;
        private ButtonClickable _rightButton;
        private ButtonClickable _midButton;
        private VisualElement _gzText;
        private VisualElement _levelUpText;
        private VisualElement _leftBlock;
        private VisualElement _rightText;
        private Label _levelNoTextLabel;
        private ProgressBar _progressBar;

        private float _oldFill;
        private float _newFill;
        private int _levels;
        private int _levelNo;

        private float _midButtonTop = 592f;
        private float _gzTextTop = -175;
        private float _sideButtonBottom = -180f;
        private float _leftTextLeft = 60f;

        private bool _newSkinButtonActive;
        private bool _otherButtonActive;
        private Action _reSkin;
        


        public BetweenLevels(int levelNoText=0, string newSkinText="", string rightText="", float filler = 0f, float oldFiller =0f, int levels=0, bool newSkin=false, bool newSkinActive = false, bool otherButtonActive=false)
        {
            this.StretchToParentSize();
            
            //var textColor = GameDataBase.TextColour();
            _oldFill = oldFiller;
            _newFill = filler;
            _levels = levels;
            _newSkinButtonActive = newSkinActive;
            _otherButtonActive = otherButtonActive; 
            
            
            style.justifyContent = Justify.Center;
            style.alignItems = Align.Center;

            var stuffBar = new VisualElement
            {
                style =
                {
                    height = 624f,
                    justifyContent = Justify.Center,
                    alignItems = Align.Center
                }
            };
            stuffBar.StretchToParentWidth();
            Add(stuffBar);
            
            
            _progressBg = new VisualElement
            {
                style =
                {
                    width = 1000f,
                    height = 624f,
                    backgroundImage = new StyleBackground(QuickAccess.LoadSprite(GameDataBase.ProgressBGPath()))
                }
            };



            InitializeButtons();

            _gzText = new Label("SUCCESS!")
            {
                style =
                {
                    width = 1000f,
                    height = 134,
                    top = _gzTextTop,
                    left = (Constants.UiWidth-1000f)*0.5f,
                    position = Position.Absolute,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 96f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = GameDataBase.TextColour()
                    
                }
            };
            
            _levelUpText = new Label(newSkin? "New look unlocked!" :"Level Up!")
            {
                style =
                {
                    width = 1000f,
                    height = 134,
                    top = _gzTextTop,
                    left = (Constants.UiWidth-1000f)*0.5f,
                    position = Position.Absolute,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 96f + (newSkin ? 12f :48f),
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = GameDataBase.TextColour()
                    
                }
            };
            _levelUpText.style.opacity = 0f;

            
            
            

            _progressBar = new ProgressBar(GameDataBase.BarBGPath(),GameDataBase.BarPath(),440f,60f, 9f,7f)
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = 0f,
                        top = 347f
                    }
                };
            _progressBar.Refill(_oldFill);


            _leftBlock = new VisualElement()
            {
                style =
                {
                    position = Position.Absolute,
                    left = _leftTextLeft,
                    height = 624f,
                    width = 440f
                }
            };
            
            

            var levelProgress = new Label("Level Progress")
            {
                style =
                {
                    width = 440f,
                    height = 68f,
                    top = 146f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/DuzYazıFontu"),
                    fontSize = 48f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = GameDataBase.TextColour()
                }
            };

            _levelNo = levelNoText;
            _levelNoTextLabel = new Label($"{levelNoText}")
            {
                style =
                {
                    width = 440f,
                    height = 140f,
                    position = Position.Absolute,
                    top = 213f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/BaslikFontu"),
                    fontSize = 96f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = GameDataBase.TextColour()
                }
            };
            
            var newLookCounter = new Label(newSkinText)
            {
                style =
                {
                    width = 440f,
                    height = 68f,
                    position = Position.Absolute,
                    top = 434f,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/DuzYazıFontu"),
                    fontSize = 32f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = GameDataBase.TextColour()
                }
            };
            
            
            _rightText = new Label(rightText)
            {
                style =
                {
                    width = 200f,
                    height = 138f,
                    position = Position.Absolute,
                    top = 243f,
                    right = 150,
                    unityFontDefinition = QuickAccess.LoadFont("fonts/DuzYazıFontu"),
                    fontSize = 32f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    color = GameDataBase.TextColour()
                }
            };


            

            _reSkin += () =>
            {
                _progressBg.style.backgroundImage =
                    new StyleBackground(QuickAccess.LoadSprite(GameDataBase.ProgressBGPath()));
                
                _gzText.style.color = GameDataBase.TextColour();
                _levelUpText.style.color = GameDataBase.TextColour();
                var v = _progressBar.Fill;
                _leftBlock.Remove(_progressBar);
                _progressBar = new ProgressBar(GameDataBase.BarBGPath(),GameDataBase.BarPath(),440f,60f, 9f,7f)
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = 0f,
                        top = 347f
                    }
                };
                _progressBar.Refill(v);
                _leftBlock.Add(_progressBar);

                levelProgress.style.color = GameDataBase.TextColour();
                _levelNoTextLabel.style.color = GameDataBase.TextColour();
                newLookCounter.style.color = GameDataBase.TextColour();
                _rightText.style.color = GameDataBase.TextColour();

            };


            
            stuffBar.Add(_gzText);
            stuffBar.Add(_levelUpText);
            _progressBg.Add(_leftBlock);
            _leftBlock.Add(_progressBar);
            _leftBlock.Add(levelProgress);
            _leftBlock.Add(_levelNoTextLabel);
            _leftBlock.Add(newLookCounter);
            _progressBg.Add(_rightText);
            
            stuffBar.Add(_progressBg);
            //Debug.Log($"{_oldFill}, {_newFill}");
            
            _leftButton.Disable(!_newSkinButtonActive);
            _rightButton.Disable(!_otherButtonActive);
            _leftButton.style.opacity = 0f;
            _leftButton.style.bottom = 0f;
            _rightButton.style.opacity = 0f;
            _rightButton.style.bottom = 0f;
        }

        private void LeftButtonFunction()
        {
            LeftButtonAction();
        }

        private void MiddleButtonFunction()
        {
            MiddleButtonAction();
        }

        private void RightButtonFunction()
        {
            RightButtonAction();
        }

        public void InitializationAnimation(float alpha)
        {
            var phaseNo = 5f;
            var alpha1 = Math.Clamp(alpha * phaseNo, 0f, 1f);
            var alpha2 = Math.Clamp(alpha * phaseNo-1f, 0f, 1f);
            var alpha3 = Math.Clamp(alpha * phaseNo-2f, 0f, 1f);
            var alpha4 = Math.Clamp(alpha * phaseNo-3f, 0f, 1f);
            var alpha5 = Math.Clamp(alpha * phaseNo-4f, 0f, 1f);

            
            _gzText.style.opacity = alpha1;
            _gzText.style.top = (1f-alpha1)*100f+_gzTextTop;
            
            _progressBg.style.opacity = alpha2;

            

            var a3 = Math.Clamp(alpha * phaseNo - 2f, 0f, _newFill - _oldFill);
            _progressBar.Refill(_oldFill + a3);
            _midButton.style.opacity = alpha3;
            _midButton.style.top = -(1f - alpha3) * 100f + _midButtonTop;

            if (_newSkinButtonActive)
            {
                _leftButton.style.opacity = alpha4;
                _leftButton.style.bottom = (1f - alpha4) * 100f+_sideButtonBottom;
            }

            if (_otherButtonActive)
            {
                _rightButton.style.opacity = alpha4;
                _rightButton.style.bottom = (1f - alpha4) * 100f+_sideButtonBottom;
            }
            
            

            _leftBlock.style.left = _leftTextLeft * alpha5 + (1000f - 440f) * 0.5f * (1f - alpha5);

            _rightText.style.opacity = alpha5;
        }

        public void LevelUpAnimation(float alpha)
        {
            var phaseNo = 5f;
            var alpha1 = Math.Clamp(alpha * phaseNo, 0f, 1f);
            var alpha2 = Math.Clamp(alpha * phaseNo-1f, 0f, 1f);
            var alpha3 = Math.Clamp(alpha * phaseNo-2f, 0f, 1f);
            var alpha4 = Math.Clamp(alpha * phaseNo-3f, 0f, 1f);
            var alpha5 = Math.Clamp(alpha * phaseNo-4f, 0f, 1f);
            
            _gzText.style.opacity = alpha1;
            _gzText.style.top = (1f-alpha1)*100f+_gzTextTop;
            
            _progressBg.style.opacity = alpha2;


            var a3 = Math.Clamp(alpha * phaseNo - 2f, 0f,_levels + _newFill - _oldFill);
            _progressBar.Refill((_oldFill + a3)%1f);

            _levelNoTextLabel.text = $"{(int) (_oldFill + a3)+_levelNo}"; 

            _levelUpText.style.opacity = alpha3;
            _levelUpText.style.top = (1f-alpha3)*100f+_gzTextTop;
            _gzText.style.top = _gzTextTop - alpha3 * 120f;
            
            _midButton.style.opacity = alpha4;
            _midButton.style.top = -(1f - alpha4) * 100f + _midButtonTop;
            
            
            
            if (_newSkinButtonActive)
            {
                _leftButton.style.opacity = alpha4;
                _leftButton.style.bottom = (1f - alpha4) * 100f+_sideButtonBottom;
                
            }

            
            if (_otherButtonActive)
            {
                _rightButton.style.opacity = alpha4;
                _rightButton.style.bottom = (1f - alpha4) * 100f+_sideButtonBottom;
            }
            
            
            
            
            _leftBlock.style.left = _leftTextLeft * alpha5 + (1000f - 440f) * 0.5f * (1f - alpha5);

            _rightText.style.opacity = alpha5;
        }


        private void InitializeButtons()
        {
            _leftButton = new ButtonClickable(imagePath:GameDataBase.SkinsPath(),Color.gray,LeftButtonFunction)
            {
                style =
                {
                    width = 264f,
                    height = 164f,
                    position = Position.Absolute,
                    bottom = _sideButtonBottom,
                    left = 0f
                }
            };
            
            
            
            _midButton = new ButtonClickable(imagePath:GameDataBase.NextPath(),Color.gray,MiddleButtonFunction)
            {
                style =
                {
                    width = 440f,
                    height = 259f,
                    position = Position.Absolute,
                    top = 592f,
                    left = 280f,
                }
            };
            
            
            
            _rightButton = new ButtonClickable(imagePath:GameDataBase.DoubleXPPath(),Color.gray,RightButtonFunction)
            {
                style =
                {
                    width = 264f,
                    height = 164f,
                    position = Position.Absolute,
                    bottom = _sideButtonBottom,
                    right = 0f,
                }
            };
            
            _reSkin += () =>
            {
                _leftButton.ChangeImage(GameDataBase.SkinsPath());
                _midButton.ChangeImage(GameDataBase.NextPath());
                _rightButton.ChangeImage(GameDataBase.DoubleXPPath());
            };
            _progressBg.Add(_leftButton);
            _progressBg.Add(_midButton);
            _progressBg.Add(_rightButton);
        }

        public void ReSkin()
        {
            _reSkin();
        }
    }
}