using System;
using System.IO;
using UnityEngine;

namespace KerbalFuture.Utils
{
	public class GUIUtils
	{
		public static string dllLoc { get; } = AssemblyLoader.GetPathByType(typeof(GUIUtils)).Replace("/PluginData/KerbalFuture", Path.DirectorySeparatorChar.ToString());
		// Use to attempt loading of a value from the given config node, name of the value, and location to put the value
		public static bool TryLoadConstant(ConfigNode cn, string nodeName, ref double loc)
		{
			if (!cn.HasValue(nodeName))
			{
				return false;
			}
			if (!cn.TryGetValue(nodeName, ref loc))
			{
				return false;
			}
			return true;
		}
		public static bool SaveConstant(ConfigNode cn, string valName, double newVal, string fileName)
		{
			if(!cn.SetValue(valName, newVal, "Edited by GUIUtils at " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year))
			{
				Debug.Log("[KF] Failed to set value of " + valName + " to file " + fileName);
				return false;
			}
			cn.Save(Path.Combine(dllLoc, fileName));
			return true;
		}
	}
}