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
			ConfigNode.Load
		}
	}
	class SpaceFolderVslChecks : MonoBehavior
	{
		public bool SpaceFolderWarpCheck()
		{
			List<Part> vslParts = this.Vessel.Parts;
			for(vslParts[i])
			{
				if(vslParts[i].Contains("SFD"+*)
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
