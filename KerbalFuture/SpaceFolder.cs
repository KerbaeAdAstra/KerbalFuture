using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;

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
	class SpaceFolderVslChecks : MonoBehavior
	{
		public bool SpaceFolderWarpCheck()
		{
			List<Part> vslParts = this.Vessel.Parts;
			for(vslParts[i])
			{
				if(partContainsModule(vslParts[i], "SpaceFolderDrive"))
				{
					
				}
			}
		}
	}
	class FlightDrive : VesselModule
	{
		Vector3 vslObtVel;
		public void FixedUpdate()
		{
			if((HighLogic.LoadedSceneIsFlight) && (SpaceFolderWarpCheck()))
			{
				vslObtVel = Vessel.GetObtVelocity();
				if (SpacefolderWarpCheck())
				{
					
				}
			}
		}
	}
	class SpaceFolderEngine : PartModule
	{
		double holeDiameter;
	}
}