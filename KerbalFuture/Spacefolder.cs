using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;

namespace KerbalFuture
{
	class SpacefolderData : MonoBehavior
	{
		public static string path()
		{
			return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
	class FlightDrive : VesselModule
	{
		Vector3 vslObtVel;
		public void FixedUpdate()
		{
			if(HighLogic.LoadedSceneIsFlight)
			{
				vslObtVel = Vessel.GetObtVelocity();
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
