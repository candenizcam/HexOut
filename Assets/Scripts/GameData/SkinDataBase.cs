using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.GameData
{
    public static partial class GameDataBase
    {
        
        
        
        public static string SkinName(SkinType st)
        {
            return st switch
            {
                SkinType.Simple => "Light",
                SkinType.PungoDark => "Dark Pungo",
                SkinType.Retro => "Retro",
                _ => throw new ArgumentOutOfRangeException(nameof(st), st, null)
            };
        }


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
                SkinType.Retro => (
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
    }
}