﻿namespace UniModules.UniGame.UniBuild.Editor.ClientBuild.BuildConfiguration
{
    using System;
    using System.Collections.Generic;
    using UniModules.Editor;
    using Interfaces;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class BuildCommandStep<TUnityCommand,TSerializableCommand>
        where TUnityCommand : Object,IUnityBuildCommand
        where TSerializableCommand :class, IUnityBuildCommand
    {
        [Space]
#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
        [Sirenix.OdinInspector.ValueDropdown(nameof(GetPostBuildCommands))]
        [Sirenix.OdinInspector.HideIf(nameof(IsSerializedCommandInitialized))]
        [Sirenix.OdinInspector.FoldoutGroup("$GroupLabel")]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public TUnityCommand buildCommand;

        [Space] 
        [SerializeReference] 
#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.HideIf(nameof(IsUnityCommandInitialized))]
        [Sirenix.OdinInspector.FoldoutGroup("$GroupLabel")]
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.InlineProperty]
#endif
        public TSerializableCommand serializableCommand;

        public string GroupLabel => IsUnityCommandInitialized ? buildCommand.Name :
            IsSerializedCommandInitialized ? serializableCommand.Name : "command";
        
        public IEnumerable<IUnityBuildCommand> GetCommands()
        {
            if (buildCommand != null)
                yield return buildCommand;
            if (serializableCommand != null)
                yield return serializableCommand;
        }
        
        public bool IsUnityCommandInitialized => buildCommand != null;

        public bool IsSerializedCommandInitialized => serializableCommand != null;
 
        public IEnumerable<TUnityCommand> GetPostBuildCommands()
        {
            return AssetEditorTools.GetAssets<TUnityCommand>();
        }
    }
}