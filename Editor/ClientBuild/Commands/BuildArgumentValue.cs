﻿namespace UniModules.UniGame.UniBuild
{
    using System;
    using UnityEngine;

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public class BuildArgumentValue
    {
#if ODIN_INSPECTOR
        [VerticalGroup]
#endif
        public string Value;
        
#if ODIN_INSPECTOR
        [VerticalGroup]
#endif
        [Tooltip("override outer arguments with this value")]
        public bool Override;
    }
}