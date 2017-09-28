using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KerbalFuture;

namespace KerbalFuture
{
	class SpaceFolderData : MonoBehavior
	{
		public static string path()
		{
			return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
		public bool partContainsModule(string miPart, string moduleName)
		{
			List<partDatabase> = GameDatabase.GetConfigNodes("PART");
			partDatabase[] = GameDatabase.GetConfigNodes("");
			if (
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public ConfigNode CFGFinder(PartModule pM)
		{
			ConfigNode[] nodeGroup = GameDatabase.Instance.GetConfigNodes("PART");
			if(nodeGroup.Length == 0)
			{
				return new ConfigNode();
			}
			for (int i; i < nodeGroup.Length; i++)
			{
				if(nodeGroup[i].GetValues(pM.Part.partName).Length > 0)
				{
					return nodeGroup[i];
				}
			}
			return new ConfigNode();
		}
	}
}