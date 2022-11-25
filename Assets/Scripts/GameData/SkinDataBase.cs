using System;
using System.Collections.Generic;
using Punity;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.GameData
{
    public static partial class GameDataBase
    {
        private static SkinType? _activeSkin = null;

        public static void SetSkinType(SkinType st)
        {
            _activeSkin = st;
            Serializer.Apply<SerialHexOutData>(sgd =>
            {
                sgd.activeSkin = st;
            });
            
        }

        public static SkinType GetSkinType()
        {
            if (_activeSkin is not null) return (SkinType)_activeSkin;

            var sgd = Serializer.Load<SerialHexOutData>();
            _activeSkin = sgd.activeSkin;
            return (SkinType)_activeSkin;
        }
        
        
        // bu fonksyonu kopyala
        public static string SkinName(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "Light",
                SkinType.PungoDark => "Dark Pungo",
                SkinType.Monochrome => "Monochrome",
                SkinType.Desert => "Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string HexTilePath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "gamefield/Hex",
                SkinType.PungoDark => "gamefield/Hex_PungoDark",
                SkinType.Monochrome => "gamefield/Hex_Monochrome",
                SkinType.Desert => "gamefield/Hex_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        
        //Bi de Hex.png'ler var ama prefablardan değişecek izel yapmadı
        
        public static string LevelBackPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            
            return st2 switch
            {
                SkinType.Simple => "UI/LevelBack",
                SkinType.PungoDark => "UI/LevelBack_PungoDark",
                SkinType.Monochrome => "UI/LevelBack_Monochrome",
                SkinType.Desert => "UI/LevelBack_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        //Game
        public static string HomeButtonPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/Game/HomeButton",
                SkinType.PungoDark => "UI/Game/HomeButton_PungoDark",
                SkinType.Monochrome => "UI/Game/HomeButton_Monochrome",
                SkinType.Desert => "UI/Game/HomeButton_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string LevelIndicatorPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/Game/LevelIndicator",
                SkinType.PungoDark => "UI/Game/LevelIndicator_PungoDark",
                SkinType.Monochrome => "UI/Game/LevelIndicator_Monochrome",
                SkinType.Desert => "UI/Game/LevelIndicator_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string LevelProgressBarPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/Game/LevelProgressBar",
                SkinType.PungoDark => "UI/Game/LevelProgressBar_PungoDark",
                SkinType.Monochrome => "UI/Game/LevelProgressBar_Monochrome",
                SkinType.Desert => "UI/Game/LevelProgressBar_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string LevelProgressBarBGPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/Game/LevelProgressBarBG",
                SkinType.PungoDark => "UI/Game/LevelProgressBarBG_PungoDark",
                SkinType.Monochrome => "UI/Game/LevelProgressBarBG_Monochrome",
                SkinType.Desert => "UI/Game/LevelProgressBarBG_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        //BetweenLevels
        public static string BarPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/BetweenLevels/Bar",
                SkinType.PungoDark => "UI/BetweenLevels/Bar_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/Bar_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/Bar_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string BarBGPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/BetweenLevels/BarBG",
                SkinType.PungoDark => "UI/BetweenLevels/BarBG_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/BarBG_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/BarBG_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string DoubleXPPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/BetweenLevels/DoubleXP",
                SkinType.PungoDark => "UI/BetweenLevels/DoubleXP_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/DoubleXP_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/DoubleXP_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string NextPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/BetweenLevels/Next",
                SkinType.PungoDark => "UI/BetweenLevels/Next_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/Next_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/Next_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string ProgressBGPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/BetweenLevels/ProgressBG",
                SkinType.PungoDark => "UI/BetweenLevels/ProgressBG_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/ProgressBG_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/ProgressBG_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string SkinsPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/BetweenLevels/Skins",
                SkinType.PungoDark => "UI/BetweenLevels/Skins_PungoDark",
                SkinType.Monochrome => "UI/BetweenLevels/Skins_Monochrome",
                SkinType.Desert => "UI/BetweenLevels/Skins_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        //Skin Selection
        public static string ArrowLeftPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/SkinSelection/ArrowLeft",
                SkinType.PungoDark => "UI/SkinSelection/ArrowLeft_PungoDark",
                SkinType.Monochrome => "UI/SkinSelection/ArrowLeft_Monochrome",
                SkinType.Desert => "UI/SkinSelection/ArrowLeft_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        public static string ArrowRightPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/SkinSelection/ArrowRight",
                SkinType.PungoDark => "UI/SkinSelection/ArrowRight_PungoDark",
                SkinType.Monochrome => "UI/SkinSelection/ArrowRight_Monochrome",
                SkinType.Desert => "UI/SkinSelection/ArrowRight_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static string IndicatorOnPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/SkinSelection/IndicatorOn",
                SkinType.PungoDark => "UI/SkinSelection/IndicatorOn_PungoDark",
                SkinType.Monochrome => "UI/SkinSelection/IndicatorOn_Monochrome",
                SkinType.Desert => "UI/SkinSelection/IndicatorOn_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        public static string IndicatorOffPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/SkinSelection/IndicatorOff",
                SkinType.PungoDark => "UI/SkinSelection/IndicatorOff_PungoDark",
                SkinType.Monochrome => "UI/SkinSelection/IndicatorOff_Monochrome",
                SkinType.Desert => "UI/SkinSelection/IndicatorOff_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        public static string ExitPath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "UI/SkinSelection/Exit",
                SkinType.PungoDark => "UI/SkinSelection/Exit_PungoDark",
                SkinType.Monochrome => "UI/SkinSelection/Exit_Monochrome",
                SkinType.Desert => "UI/SkinSelection/Exit_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        // bura gerçeğiyle değişicek
        public static string SkinSelectorFacePath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "gamefield/Hex",
                SkinType.PungoDark => "gamefield/Hex_PungoDark",
                SkinType.Monochrome => "gamefield/Hex_Monochrome",
                SkinType.Desert => "gamefield/Hex_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        // bura kapalı gerçeğiyle değişicek
        public static string SkinSelectorOffFacePath(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple => "gamefield/Hex",
                SkinType.PungoDark => "gamefield/Hex_PungoDark",
                SkinType.Monochrome => "gamefield/Hex_Monochrome",
                SkinType.Desert => "gamefield/Hex_Desert",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }


        /** Only works up to the limits of capsule colours
         * if this game ever reaches a point where we change visuals, good for u chum
         * also put where this is called into an if, and write paths to the alternatives there, dont alter this
         */
        public static (Color[] inside, Color[] outside) CapsuleColours(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
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
                        new Color(247f/255f, 127f/255f, 93f/255f),
                        new Color(249f/255f, 225f/255f, 120f/255f),
                        new Color(66f/255f, 214f/255f, 178f/255f),
                    },
                    outside: new Color[]
                    {
                        new Color(215f/255f, 82f/255f, 45f/255f),
                        new Color(244f/255f, 205f/255f, 39f/255f),
                        new Color(26f/255f, 123f/255f, 98f/255f),
                    }),
                SkinType.Monochrome => (
                    inside: new Color[]
                    {
                        new Color(140f/255f, 192f/255f, 173f/255f),
                        new Color(85f/255f, 119f/255f, 105f/255f),
                        new Color(57f/255f, 82f/255f, 71f/255f),
                        new Color(46f/255f, 67f/255f, 57f/255f),
                    },
                    outside: new Color[]
                    {
                        new Color(46f/255f, 67f/255f, 57f/255f),
                        new Color(40f/255f, 60f/255f, 51f/255f),
                        new Color(35f/255f, 52f/255f, 44f/255f),
                        new Color(29f/255f, 45f/255f, 37f/255f),
                    }),
                SkinType.Desert => (
                    inside: new Color[]
                    {
                        new Color(217f/255f, 164f/255f, 145f/255f),
                        new Color(189f/255f, 75f/255f, 70f/255f),
                        new Color(114f/255f, 168f/255f, 153f/255f),
                        new Color(107f/255f, 155f/255f, 179f/255f),
                        new Color(87f/255f, 120f/255f, 161f/255f),
                        new Color(85f/255f, 85f/255f, 130f/255f),
                    },
                    outside: new Color[]
                    {
                        new Color(186f/255f, 105f/255f, 75f/255f),
                        new Color(130f/255f, 23f/255f, 16f/255f),
                        new Color(98f/255f, 148f/255f, 127f/255f),
                        new Color(65f/255f, 119f/255f, 135f/255f),
                        new Color(43f/255f, 76f/255f, 108f/255f),
                        new Color(45f/255f, 45f/255f, 68f/255f),
                    }),
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }

        public static Color ObstacleColour(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple =>  new Color(62f/255f, 0f/255f, 160f/255f),
                SkinType.PungoDark =>  new Color(196f/255f, 177f/255f, 209f/255f),
                SkinType.Monochrome =>  new Color(29f/255f, 45f/255f, 37f/255f),
                SkinType.Desert => new Color(37f/255f, 35f/255f, 54f/255f),
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        public static Color TextColour(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple =>  new Color(127f/255f, 88f/255f, 189f/255f),
                SkinType.PungoDark =>  new Color(78f/255f, 49f/255f, 119f/255f),
                SkinType.Monochrome =>  new Color(29f/255f, 45f/255f, 37f/255f),
                SkinType.Desert => new Color(37f/255f, 35f/255f, 54f/255f),
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
        
        
        public static Color BackgroundColour(SkinType? st=null)
        {
            var st2 = st ??= GetSkinType();
            return st2 switch
            {
                SkinType.Simple =>  new Color(217f/255f, 221f/255f, 232f/255f),
                SkinType.PungoDark =>  new Color(50f/255f, 24f/255f, 69f/255f),
                SkinType.Monochrome =>  new Color(213f/255f, 247f/255f, 234f/255f),
                SkinType.Desert => new Color(209f/255f, 190f/255f, 174f/255f),
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }
    }
}