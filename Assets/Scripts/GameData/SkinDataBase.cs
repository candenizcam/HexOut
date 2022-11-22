using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.GameData
{
    public static partial class GameDataBase
    {
        
        
        // bu fonksyonu kopyala
        public static string SkinName(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "Light",
                SkinType.PungoDark => "Dark Pungo",
                SkinType.Monochrome => "Monochrome",
                SkinType.Desert => "Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        //Bi de Hex.png'ler var ama prefablardan değişecek izel yapmadı
        
        public static string LevelBackPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/LevelBack",
                SkinType.PungoDark => "UI/LevelBack_PungoDark",
                SkinType.Monochrome => "UI/LevelBack_Monochrome",
                SkinType.Desert => "UI/LevelBack_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        //Game
        public static string HomeButtonPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/Game/HomeButton",
                SkinType.PungoDark => "UI/Game/HomeButton_PungoDark",
                SkinType.Monochrome => "UI/Game/HomeButton_Monochrome",
                SkinType.Desert => "UI/Game/HomeButton_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string LevelIndicatorPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/Game/LevelIndicator",
                SkinType.PungoDark => "UI/Game/LevelIndicator_PungoDark",
                SkinType.Monochrome => "UI/Game/LevelIndicator_Monochrome",
                SkinType.Desert => "UI/Game/LevelIndicator_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string LevelProgressBarPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/Game/LevelProgressBar",
                SkinType.PungoDark => "UI/Game/LevelProgressBar_PungoDark",
                SkinType.Monochrome => "UI/Game/LevelProgressBar_Monochrome",
                SkinType.Desert => "UI/Game/LevelProgressBar_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string LevelProgressBarBGPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/Game/LevelProgressBarBG",
                SkinType.PungoDark => "UI/Game/LevelProgressBarBG_PungoDark",
                SkinType.Monochrome => "UI/Game/LevelProgressBarBG_Monochrome",
                SkinType.Desert => "UI/Game/LevelProgressBarBG_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        //BetweenLevels
        public static string BarPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/BetweenLevels/Bar",
                SkinType.PungoDark => "UI/BetweenLevels/Bar_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/Bar_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/Bar_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string BarBGPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/BetweenLevels/BarBG",
                SkinType.PungoDark => "UI/BetweenLevels/BarBG_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/BarBG_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/BarBG_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string DoubleXPPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/BetweenLevels/DoubleXP",
                SkinType.PungoDark => "UI/BetweenLevels/DoubleXP_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/DoubleXP_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/DoubleXP_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string NextPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/BetweenLevels/Next",
                SkinType.PungoDark => "UI/BetweenLevels/Next_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/Next_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/Next_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string ProgressBGPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/BetweenLevels/ProgressBG",
                SkinType.PungoDark => "UI/BetweenLevels/ProgressBG_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/ProgressBG_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/ProgressBG_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string SkinsPath(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "UI/BetweenLevels/Skins",
                SkinType.PungoDark => "UI/BetweenLevels/Skins_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/Skins_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/Skins_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        //Skin Selection
        


        /** Only works up to the limits of capsule colours
         * if this game ever reaches a point where we change visuals, good for u chum
         * also put where this is called into an if, and write paths to the alternatives there, dont alter this
         */
        public static (Color[] inside, Color[] outside) CapsuleColours(SkinType st)
        {
            return st switch
            {
                SkinType.Simple =>  (
                    inside: new Color[]
                {
                    new Color(0.996f, 0.004f, 0.29f),
                    new Color(1f, 0.427f, 0.141f ),
                    new Color(1f, 0.953f, 0.071f),
                    new Color(0.039f, 0.91f, 0.412f),
                    new Color(0f, 0.718f, 1f),
                    new Color(0.29f, 0f, 1f),
                },
                    outside: new Color[]
                {
                    new Color(0.808f, 0f, 0.271f),
                    new Color(0.949f, 0.29f, 0f),
                    new Color(0.878f, 0.722f, 0.071f),
                    new Color(0.039f, 0.757f, 0.329f),
                    new Color(0.008f, 0.58f, 0.757f),
                    new Color(0.137f, 0f, 0.71f),
                }),
                SkinType.PungoDark => (
                    inside: new Color[]
                    {
                        new Color(0.996f, 0.004f, 0.29f),
                        new Color(1f, 0.427f, 0.141f ),
                        new Color(1f, 0.953f, 0.071f),
                        new Color(0.039f, 0.91f, 0.412f),
                        new Color(0f, 0.718f, 1f),
                        new Color(0.29f, 0f, 1f),
                        new Color(0.243f, 0f, 0.627f ),
                    },
                    outside: new Color[]
                    {
                        new Color(0.808f, 0f, 0.271f),
                        new Color(0.949f, 0.29f, 0f),
                        new Color(0.878f, 0.722f, 0.071f),
                        new Color(0.039f, 0.757f, 0.329f),
                        new Color(0.008f, 0.58f, 0.757f),
                        new Color(0.137f, 0f, 0.71f),
                        new Color(0.145f, 0f, 0.427f),
                    }),
                SkinType.Monochrome => (
                    inside: new Color[]
                    {
                        new Color(0.996f, 0.004f, 0.29f),
                        new Color(1f, 0.427f, 0.141f ),
                        new Color(1f, 0.953f, 0.071f),
                        new Color(0.039f, 0.91f, 0.412f),
                        new Color(0f, 0.718f, 1f),
                        new Color(0.29f, 0f, 1f),
                        new Color(0.243f, 0f, 0.627f ),
                    },
                    outside: new Color[]
                    {
                        new Color(0.808f, 0f, 0.271f),
                        new Color(0.949f, 0.29f, 0f),
                        new Color(0.878f, 0.722f, 0.071f),
                        new Color(0.039f, 0.757f, 0.329f),
                        new Color(0.008f, 0.58f, 0.757f),
                        new Color(0.137f, 0f, 0.71f),
                        new Color(0.145f, 0f, 0.427f),
                    }),
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }

        public static Color ObstacleColour(SkinType st)
        {
            return st switch
            {
                SkinType.Simple =>  new Color(62f/255f, 0f/255f, 160f/255f),
                SkinType.PungoDark =>  new Color(62f/255f, 0f/255f, 160f/255f),
                SkinType.Monochrome =>  new Color(62f/255f, 0f/255f, 160f/255f),
                SkinType.Desert => new Color(62f/255f, 0f/255f, 160f/255f)
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
    }
}