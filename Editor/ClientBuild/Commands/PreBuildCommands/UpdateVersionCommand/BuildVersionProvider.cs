﻿namespace UniModules.UniGame.UniBuild.Editor.UpdateVersionCommand
{
    using System.Text;
    using UnityEditor;

    public class BuildVersionProvider
    {

        public string GetBuildVersion(BuildTarget buildTarget,string bundleVersion, int buildNumber, string branch = null) {
            
            var versionLenght = GetVersionLength(buildTarget);
            var versionPoints = bundleVersion.Split('.');
            
            var versionBuilder = new StringBuilder();

            for (var i = 0; i < versionLenght; i++) {
                versionBuilder.Append(versionPoints.Length > i ? versionPoints[i] : "0");
                versionBuilder.Append(".");
            }
                        
            versionBuilder.Append(buildNumber);

            if (buildTarget != BuildTarget.Android || string.IsNullOrEmpty(branch)) 
                return versionBuilder.ToString();
            
            var shortBranch = GetShortBranch(branch);
            if (string.IsNullOrEmpty(shortBranch)) 
                return versionBuilder.ToString();
            versionBuilder.Append(" ");
            versionBuilder.Append(shortBranch);

            return versionBuilder.ToString();
            
        }
        
        public int GetActiveBuildNumber(BuildTarget target) {

            var activeVersion = 0;
            
            switch (target) {
                case BuildTarget.Android:
                    activeVersion = PlayerSettings.Android.bundleVersionCode;
                    break;
                case BuildTarget.iOS:
                    if(int.TryParse(PlayerSettings.iOS.buildNumber, out var iosBuildNumber))
                        activeVersion = iosBuildNumber;
                    break;
                default:
                    if(int.TryParse(PlayerSettings.bundleVersion, out var standaloneBuildNumber))
                        activeVersion = standaloneBuildNumber;
                    break;
            }

            return activeVersion;

        }

        public int GetVersionLength(BuildTarget buildTarget) {
            var length =  buildTarget == BuildTarget.iOS ? 2 : 3;
            return length;
        }
    
        private static string GetShortBranch(string branch) {
            if (branch == "master") {
                return string.Empty;
            } 
            if (branch == "develop") {
                return "develop";
            }

            if (branch.StartsWith("feature/")) {
                return branch.Substring("feature/".Length);
            }
            
            return branch;
        }
    }
}
