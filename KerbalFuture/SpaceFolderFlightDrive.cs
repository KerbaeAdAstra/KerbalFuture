using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;

namespace KerbalFuture
{
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
}
