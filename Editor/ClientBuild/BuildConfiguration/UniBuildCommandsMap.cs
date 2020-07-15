﻿namespace UniModules.UniGame.UniBuild.Editor.ClientBuild.BuildConfiguration
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Commands.PostBuildCommands;
    using Commands.PreBuildCommands;
    using Interfaces;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/UniBuild/UniBuildConfiguration", fileName = nameof(UniBuildCommandsMap))]
    public class UniBuildCommandsMap : ScriptableObject, IUniBuildCommandsMap
    {

#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty()]
        [Sirenix.OdinInspector.HideLabel()]
#endif
        [SerializeField]
        private UniBuildConfigurationData _buildData = new UniBuildConfigurationData();

        [Space]
#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        [SerializeField]
        private List<UnityPreBuildCommand> _preBuildCommands = new List<UnityPreBuildCommand>();
        
#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        [SerializeField]
        private List<UnityPostBuildCommand> _postBuildCommands = new List<UnityPostBuildCommand>();

        #region public properties

        public IUniBuildConfigurationData BuildData => _buildData;
        
        public IReadOnlyList<IUnityPreBuildCommand> PreBuildCommands => _preBuildCommands;

        public IReadOnlyList<IUnityPostBuildCommand> PostBuildCommands => _postBuildCommands;
        
        public string ItemName => name;
        
        #endregion

        public List<IEditorAssetResource> LoadCommands<T>(Func<T,bool> filter = null)
            where T : IUnityBuildCommand 
        {
            var result = new List<IEditorAssetResource>();
            
            var commandsBuffer = ClassPool.Spawn<List<UnityBuildCommand>>();
            commandsBuffer.AddRange(_preBuildCommands);
            commandsBuffer.AddRange(_postBuildCommands);
            
            foreach (var command in commandsBuffer) {
                if (command is T targetCommand) {
                    
                    if(filter!=null && !filter(targetCommand))
                        continue;
                    
                    result.Add(new EditorAssetResource().Initialize(command));
                }
            }

            commandsBuffer.Despawn();

            return result;
        }

        public bool Validate(IUniBuilderConfiguration config)
        {
            var buildParameters = config.BuildParameters;

            if (BuildData.BuildTarget != buildParameters.BuildTarget)
                return false;

            if (BuildData.BuildTargetGroup!=buildParameters.BuildTargetGroup)
                return false;

            var isUnderCloud = false;
            
#if UNITY_CLOUD_BUILD
            isUnderCloud = true;
#endif
            
            return ValidatePlatform(config);
        }

#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button("Execute")]
#endif
        public void ExecuteBuild()
        {
            UniBuildTool.ExecuteBuild(this);
        }
        
        protected virtual bool ValidatePlatform(IUniBuilderConfiguration config)
        {
            return true;
        }

    }
}