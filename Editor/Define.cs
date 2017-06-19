using System;
using UnityEngine;
using UnityEditor;


namespace EPPZ.Lines.Editor
{


	[InitializeOnLoad]
	public class Define
	{


		const string define = "EPPZ_LINES_IS_AVAILABLE";


		static Define()
		{ AddDefineIfNeeded(); }

		static void AddDefineIfNeeded()
		{
			BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			if (defines.Contains(define)) return; // Change only if needed
			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (defines + ";" + define));
			Debug.LogWarning("<b>"+define+"</b> added to <i>Scripting Define Symbols</i> for selected build target ("+EditorUserBuildSettings.activeBuildTarget.ToString()+").");
		}
	}
}
