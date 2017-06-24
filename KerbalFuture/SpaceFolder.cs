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
			partDatabase[] = GameDatabase.GetConfigNodes("
			if(
			{
				return true;
			}
			else
			{
				return false;
			}
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
				if (SpacefolderWarpCheck(true))
			}
		}
	}
	class SpaceFolderEngine : PartModule
	{
		double holeDiameter;
	}
}
