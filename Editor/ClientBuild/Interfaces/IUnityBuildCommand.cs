﻿namespace UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces
{
    using global::UniGame.UniBuild.Editor.ClientBuild.Interfaces;

    public interface IUnityBuildCommand : IUnityBuildCommandValidator, IUnityBuildCommandInfo
    {
        void Execute(IUniBuilderConfiguration configuration);
    }
}
