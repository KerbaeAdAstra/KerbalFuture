using System;
using System.IO;
using UnityEngine;

/*
 * Notes on usage of the changelog
 * 
 * Changelog MUST be titled Changelog.cfg
 * Changelog MUST be in the following format:
 * 
 * showChangelog = [true|false]
 * VERSION
 * {
 *		version = 1.1
 *		change = Added new parts.
 *		change = Removed most bugs.
 *	}
 *	VERSION
 *	{
 *		version = 1.0
 *		change = Release!!!
 *	}
 *	
 *	Treat it with the same level of respect as you do regular config files. 
 *	When there is a new release, change showChangelog to true. 
 *	After the changelog is viewed in-game, the value will be automatically set to false.
*/

namespace KerbalFuture.KFGUI
{
	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class Changelog : MonoBehaviour
	{
		// No creating new instances of this
		private Changelog()
		{

		}
		public string dllLoc { get; } = AssemblyLoader.GetPathByType(typeof(Changelog)).Replace("/PluginData/KerbalFuture", Path.DirectorySeparatorChar.ToString());
		bool showChangelog;
		Rect changelogRect = new Rect(100, 100, 300, 400);
		Vector2 changelogScrollPos = new Vector2();
		string changelog = string.Empty;

		private void Start()
		{
			LoadChangelog();
			Debug.Log("[KF] dllLoc = " + dllLoc);
		}
		private void OnGUI()
		{
			if (showChangelog)
			{
				changelogRect = GUILayout.Window(0, changelogRect, DrawChangelogWindow, "Kerbal Future Changelog", GUILayout.Width(300), GUILayout.Height(400));
			}
		}
		private void DrawChangelogWindow(int id)
		{
			GUI.DragWindow(new Rect(0, 0, 300, 20));
			changelogScrollPos = GUILayout.BeginScrollView(changelogScrollPos);
			GUILayout.Label(changelog);
			if(GUILayout.Button("Close changelog"))
			{
				showChangelog = false;
			}
			GUILayout.EndScrollView();
		}
		private void LoadChangelog()
		{
			Debug.Log("[KF] Loading changelog...");
			ConfigNode versionCfg = ConfigNode.Load(Path.Combine(dllLoc, "Changelog.cfg"));
			if(!versionCfg.HasValue("showChangelog"))
			{
				Debug.Log("[KF] Unable to find value showChangelog in Changelog.cfg");
				return;
			}
			if(!versionCfg.TryGetValue("showChangelog", ref showChangelog))
			{
				Debug.Log("[KF] Unable to load value from 'showChangelog'.");
				return;
			}
			if(!versionCfg.SetValue("showChangelog", false))
			{
				Debug.Log("[KF] Unable to set value 'showChangelog'.");
				return;
			}
			versionCfg.Save(Path.Combine(dllLoc, "Changelog.cfg"));
			if(!showChangelog)
			{
				Debug.Log("[KF] Changelog load aborted due to showChangelog being set to false.");
			}
			bool firstTimeThrough = true;
			foreach(ConfigNode cfg in versionCfg.GetNodes())
			{
				if(!firstTimeThrough)
				{
					changelog += Environment.NewLine;
				}
				float version = 0;
				cfg.TryGetValue("version", ref version);
				if (firstTimeThrough)
				{
					changelog = version.ToString();
				}
				else
				{
					changelog += Environment.NewLine + version;
				}
				foreach (string s in cfg.GetValues("change"))
				{
					changelog += Environment.NewLine + s;
				}
				firstTimeThrough = false;
			}
		}
	}
}