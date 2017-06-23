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
	}
	class SpaceFolderVslChecks : MonoBehavior
	{
		List<Part> Vessel.Parts = new List<Part> Vessel.Parts;
		public bool SpaceFolderWarpCheck()
		{
			if (Vessel.
		}
	}
	class FlightDrive : VesselModule
	{
		Vector3 vslObtVel;
		public void FixedUpdate()
		{
			//need a part list here...
			if((HighLogic.LoadedSceneIsFlight) && (SpaceFolderWarpCheck()))
			{
				vslObtVel = Vessel.GetObtVelocity();
				if (SpacefolderWarpCheck(true))
			}
		}
	}
	class SpaceFolderEngine : PartModule
	{
		double xMaxLength;
		double yMaxLength;
		double 
	}
}
